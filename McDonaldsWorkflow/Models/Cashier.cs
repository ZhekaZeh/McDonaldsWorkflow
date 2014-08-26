using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using McDonaldsWorkflow.Models.Enums;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public class Cashier : Employee, ICashier
    {
        #region Private fields

        private readonly Dictionary<MealTypes, ICook> _cooks;
        private readonly Queue<Client> _line;
        private readonly Dictionary<MealTypes, int> _restOrder;
        private Client _currentClient;
        private double _takings;

        #endregion

        #region Properties

        public int Id { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="Cashier" /> class.
        /// </summary>
        /// <param name="id">Cashier's Id.</param>
        /// <param name="cooks">The list of McDonald's cooks.</param>
        public Cashier(int id, IEnumerable<ICook> cooks) : base(String.Format("Cashier #{0}", id))
        {
            Id = id;
            _cooks = cooks.ToDictionary(cook => cook.MealType);
            _line = new Queue<Client>();
            _takings = Constants.InitialTakings;
            _restOrder = new Dictionary<MealTypes, int>();
        }

        #endregion

        #region Private methods

        /// <summary>
        ///     Grabs all missing meals as soon as possible.
        /// </summary>
        private void GrabMissingMeals()
        {
            var changedEntries = new Dictionary<MealTypes, int>();

            while (_restOrder.Count > 0)
            {
                Thread.Sleep(Constants.CashierGrabMealRetryTimeoutMs);
                Console.WriteLine(@"Cashier {0} has pending meals. Retrying....", _employeeName);

                foreach (var orderEntry in _restOrder.Where(x => x.Value > 0))
                {
                    ICook cook = _cooks[orderEntry.Key];
                    int takenCount;
                    cook.TryGetMeals(orderEntry.Value, out takenCount);
                    changedEntries.Add(orderEntry.Key, orderEntry.Value - takenCount);
                }

                //clean _restOrder collection from outside to avoid CollectionChanged exception.
                foreach (var changedEntry in changedEntries)
                {
                    _restOrder[changedEntry.Key] = changedEntry.Value;
                    if (changedEntry.Value == 0)
                    {
                        _restOrder.Remove(changedEntry.Key);
                    }
                }
                changedEntries.Clear();
            }
            _restOrder.Clear();
            Console.WriteLine(@"Order for client {0} is completed.", _currentClient.ClientId);
        }

        /// <summary>
        ///     Takes cash from client and adds to takings.
        /// </summary>
        private void GetMoney()
        {
            double cash = _currentClient.Order.Sum(orderItems => (Constants.PriceList[orderItems.Key]*orderItems.Value));
            lock (_lockObj)
            {
                _takings += cash;
            }
            Console.WriteLine(@"{0} take money: cash = {1}, takings = {2}. $$$$$$$$$$$$$$$$$$$$$$$$", _employeeName,
                cash, _takings);
            _currentClient = null;
        }

        /// <summary>
        ///     Clients are going away.
        /// </summary>
        private void PutClientsOut()
        {
            lock (_lockObj)
            {
                _line.Clear();
            }
        }

        /// <summary>
        ///     Just waits until last client will not be serve.
        /// </summary>
        private void ServeLastClient()
        {
            while (_currentClient != null)
            {
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Tries to gather next client's order.
        /// </summary>
        public void TryToGatherOrder()
        {
            _currentClient = _line.Dequeue();

            foreach (var mealCountPair in _currentClient.Order)
            {
                int takenCount;

                ICook cook = _cooks[mealCountPair.Key];

                Console.WriteLine(@"{0} try to get {1} {2}........", Id, mealCountPair.Value,
                    mealCountPair.Key);

                if (!cook.TryGetMeals(mealCountPair.Value, out takenCount))
                    _restOrder.Add(mealCountPair.Key, mealCountPair.Value - takenCount);
            }
        }

        #endregion

        #region ICashier implementation

        public double Takings
        {
            get
            {
                lock (_lockObj)
                {
                    return _takings;
                }
            }
            set { _takings = value; }
        }

        public int LineCount
        {
            get
            {
                lock (_lockObj)
                {
                    return _line.Count;
                }
            }
        }

        /// <summary>
        ///     Adds client to queue.
        /// </summary>
        /// <param name="client">Client which stand in line</param>
        public void StandOnLine(Client client)
        {
            lock (_lockObj)
            {
                if (!IsEndOfDay)
                {
                    _line.Enqueue(client);
                }
            }

            Console.WriteLine(@"Client{0} stand in line to {1}", client.ClientId, _employeeName);
            _waitHandle.Set();
        }

        #endregion

        #region Employee abstract methods implementation

        /// <summary>
        ///     Determines whether somebody exist in line or nor.
        /// </summary>
        /// <returns>true if somebody exist, false otherwise</returns>
        protected override bool HasSomethingToDo()
        {
            lock (_lockObj)
            {
                return LineCount > 0;
            }
        }

        /// <summary>
        ///     Works this instance.
        /// </summary>
        protected override void Work()
        {
            TryToGatherOrder();
            GrabMissingMeals();
            GetMoney();
        }

        /// <summary>
        ///     Performs next instuctions before current thread would be killed and employee(cashier) goes home.
        /// </summary>
        protected override void GoHome()
        {
            PutClientsOut();
            ServeLastClient();
            Console.WriteLine(@"{0} has finished work and is going home.", _employeeName);
        }

        #endregion
    }
}