using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public class Cashier : Employee, ICashier
    {
        #region Private fields

        private object _lock;
        private double _takings;
        private Queue<Client> _line;
        private Client _currentClient;
        private readonly List<ICook> _cooks;
        private Dictionary<MealTypes, int> _restOrder; 
     
        #endregion
 
        #region Properties

        public int Id { get; private set; }

        #endregion

        #region Constructor

        public Cashier(int id, List<ICook> cooks):base(String.Format("Cashier #{0}", id))
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
                        var cook = _cooks.Find(x => x.MealType == (MealTypes) i);
                        if (cook.TryGetMeals(_restOrder[(MealTypes) i], out taken)) //sometimes trow exeption here
                            _restOrder[(MealTypes) i] = 0;
                        else
                        {
                            _restOrder[(MealTypes) i] -= taken;
                        }
                    }
                }
            }
            Console.WriteLine(@"Client go away------------------------------------------------");
        }

        /// <summary>
        ///     Takes cash from client and add to takings
        /// </summary>
        private void GetMoney()
        {
            double cash = _currentClient.Order.Sum(orderItems => Constants.PriceList[orderItems.Key]);
            lock (_lockObj)
            {
                _takings += cash;
            }

            Console.WriteLine(@"      Cashier {2} Take money $$$: {0}........... takings = {1}", cash, _takings, this.Id);
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Tries to gather next client's order
        /// </summary>
        public void TryToGatherOrder()
        {
            _currentClient = _line.Dequeue();

            foreach (KeyValuePair<MealTypes, int> mealCountPair in _currentClient.Order)
            {
                int takenCount;
                var cook = _cooks.Find(x => x.MealType == mealCountPair.Key);

                Console.WriteLine(@"  Cashier {0} try to get {1} {2}........", Id, mealCountPair.Value, mealCountPair.Key);

                if (!cook.TryGetMeals(mealCountPair.Value, out takenCount)) _restOrder.Add(mealCountPair.Key, mealCountPair.Value);
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
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void StandOnLine(Client client)
        {
            lock (_lockObj)
            {
                _line.Enqueue(client);
            }

            Console.WriteLine(@"Client{0} stand in line.", client.ClientId);
            _waitHandle.Set();
        }

        #endregion

        #region Employee abstract methods implementation

        /// <summary>
        /// Determines whether the table is full or nor.
        /// </summary>
        /// <returns>true if the table is full, false otherwise</returns>
        protected override bool HasSomethingToDo()
        {
            //check if the table is full
            lock (_lock)
            {
                return LineCount > 0;
            }
        }

        /// <summary>
        /// Works this instance.
        /// </summary>
        protected override void Work()
        {
            TryToGatherOrder();
            GrabMissingMeals();
            GetMoney();
            _restOrder.Clear();
        }

        #endregion
    }
}