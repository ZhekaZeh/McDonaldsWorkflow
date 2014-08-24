using System;
using System.Collections.Generic;
using System.Linq;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public class Cashier : Employee, ICashier
    {
        #region Private fields

        private readonly List<ICook> _cooks;
        private readonly Queue<Client> _line;
        private readonly object _lock;
        private readonly Dictionary<MealTypes, int> _restOrder;
        private Client _currentClient;
        private double _takings;

        #endregion

        #region Properties

        public int Id { get; private set; }

        #endregion

        #region Constructor

        public Cashier(int id, List<ICook> cooks) : base(String.Format("Cashier #{0}", id))
        {
            Id = id;
            _cooks = cooks;
            _lock = new object();
            _line = new Queue<Client>();
            _takings = Constants.InitialTakings;
            _restOrder = new Dictionary<MealTypes, int>();
        }

        #endregion

        #region Private methods

        private void GrabMissingMeals()
        {
            if (_restOrder.Count > 0)
            {
                while (_restOrder.Max(x => x.Value) > 0)
                {
                    for (int i = 0; i < _restOrder.Count - 1; i++)
                    {
                        int taken;
                        ICook cook = _cooks.Find(x => x.MealType == (MealTypes) i);
                        if (cook.TryGetMeals(_restOrder[(MealTypes) i], out taken))
                            //sometimes trow exeption here 'KeyNotFoundExeption'
                            _restOrder[(MealTypes) i] = 0;
                        else
                        {
                            _restOrder[(MealTypes) i] -= taken;
                        }
                    }
                }
            }
            Console.WriteLine(@"Client {0} go away-------------------------------------------------",
                _currentClient.ClientId);
        }

        /// <summary>
        ///     Takes cash from client and add to takings
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
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Tries to gather next client's order
        /// </summary>
        public void TryToGatherOrder()
        {
            _currentClient = _line.Dequeue();

            foreach (var mealCountPair in _currentClient.Order)
            {
                int takenCount;
                ICook cook = _cooks.Find(x => x.MealType == mealCountPair.Key);

                Console.WriteLine(@"  Cashier {0} try to get {1} {2}........", Id, mealCountPair.Value,
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
        ///     Adds client to queue
        /// </summary>
        /// <param name="client"></param>
        public void StandOnLine(Client client)
        {
            lock (_lockObj)
            {
                _line.Enqueue(client);
            }

            Console.WriteLine(@"Client{0} stand in line to {1}", client.ClientId, _employeeName);
            _waitHandle.Set();
        }

        #endregion

        #region Employee abstract and virtual methods implementation

        /// <summary>
        ///     Determines whether somebody exist in line or nor.
        /// </summary>
        /// <returns>true if somebody exist, false otherwise</returns>
        protected override bool HasSomethingToDo()
        {
            lock (_lock)
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
            _restOrder.Clear();
        }

        protected override void GoHome()
        {
            Manager.GetTakings(Takings);
            Console.WriteLine(@"{0} took his daily takings to manager {1}", _employeeName, Takings);

            Console.WriteLine(@"{0} is going home. Bye bye", _employeeName);
        }

        #endregion
    }
}