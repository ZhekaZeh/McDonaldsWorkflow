using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public class McDonalds : ICompany
    {
        #region Private fields
        
        private static volatile ICompany _instance;
        private static readonly object _syncRoot = new Object();
        private readonly object _lockObj;
        private readonly List<ICashier> _cashiers;
        private readonly List<ICook> _cooks;
        private bool _isEndOfDay;

        #endregion

        #region Constructor

        private McDonalds()
        {
            _cashiers = new List<ICashier>();
            _cooks = new List<ICook>();
            _isEndOfDay = false;
            _lockObj = new object();
        }

        #endregion

        #region Properties

        public static ICompany Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new McDonalds();
                }
                return _instance;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Initializes lists with Cashiers and Cooks (Emloyees). For test it creates only one cashier and one cook
        /// </summary>
        private void InitializeEmployees()
        {

            //foreach (MealTypes mealType in Enum.GetValues(typeof(MealTypes)))
            //{
            //    _cooks.Add(new Cook(mealType, Constants.CookingTimeBurgerMs));
            //}


            _cooks.Add(new Cook(MealTypes.Hamburger, Constants.CookingTimeBurgerMs));

            for (var i = 1; i <= Constants.CashierCount; i++)
            {
                var cashier = new Cashier(i, _cooks);
                _cashiers.Add(cashier);
                ThreadPool.QueueUserWorkItem((object obj) => cashier.DoWork());
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Logic for generate random clients
        /// </summary>
        private void GenerateClients()
        {
        // Must be use LINQ. This method will be fixed
            Thread.Sleep(500);
            Console.WriteLine(@"Client went to McDonalds.");
            Thread.Sleep(2000);
            var rnd = new Random(); // for test only

            lock (_lockObj)
            {
                _cashiers[0].StandOnLine(new Client(5, rnd.Next(99)));
            }
            
        }

        /// <summary>
        ///     Starts McDonald's workflow
        /// </summary>
        public void StartWork()
        {
            InitializeEmployees();
            do
            {
                GenerateClients();

            } while (!IsEndOfDay);
        }
        #endregion

        #region ICompany implementation

        public bool IsEndOfDay
        {
            get
            {
                lock (_lockObj)
                {
                    return _isEndOfDay;
                }
            }
        }

        /// <summary>
        ///     Ends the day.
        /// </summary>
        public void EndTheDay()
        {
            _isEndOfDay = true;

            foreach (var cook in _cooks)
            {
                cook.WaitHandle.Set();
            }
        }

        #endregion
    }
}