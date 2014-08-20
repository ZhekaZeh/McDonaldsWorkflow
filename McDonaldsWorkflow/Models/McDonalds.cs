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

        //System.Lazy type guarantees thread-safe lazy-construction
        
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
            InitializeEmployees();
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

        public List<ICook> Cooks
        {
            get { return _cooks; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes lists with Cashiers and Cooks (Emloyees). For test it creates only one cashier and one cook
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
                _cashiers.Add(new Cashier(i));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Logic for generate random clients
        /// </summary>
        public void GenerateClients()
        {
           // must be use linq .....................this method will be fixed
            Thread.Sleep(500);
            Console.WriteLine(@"Client went to McDonalds...");
            Thread.Sleep(500);

            lock (_lockObj)
            {
                _cashiers[0].Line.Enqueue(new Client(5));
                Console.WriteLine(@"Client is making order...5 meals");
            }
            
        }

        /// <summary>
        /// Starts McDonald's workflow
        /// </summary>
        public void StartWork()
        {
            do
            {
                ThreadPool.QueueUserWorkItem((object obj) => GenerateClients()); 

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