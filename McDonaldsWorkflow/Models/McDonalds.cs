using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using McDonaldsWorkflow.Models.Enums;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public class McDonalds : ICompany
    {
        #region Private fields

        private static volatile ICompany _instance;
        private static readonly object _syncRoot = new Object();
        private readonly object _lockObj;
        private List<ICashier> _cashiers;
        private List<ICook> _cooks;
        private Task[] _cooksTasksArray;
        private Task[] _cashiersTasksArray;
        private bool _isEndOfDay;
        private Manager _manager;
        private Dictionary<MealTypes, double> _menu;
        public static readonly ILog log = LogManager.GetLogger(typeof(McDonalds)); // Log4net Manager for current class

        #endregion

        #region Private Constructor

        /// <summary>
        ///     Has private constructor as part of Singelton Pattern. Accsess through 'Instanse' property.
        /// </summary>
        private McDonalds()
        {
            _isEndOfDay = false;
            _lockObj = new object();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Implementation of Singelton Pattern with tread-safe construction.
        /// </summary>
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
        ///     Initializes McDonald's employees.
        /// </summary>
        private void InitializeEmployees()
        {
            InitializeMenuAndCooks();
            InitializeCashiers();
            _manager = new Manager(_cashiers);
        }

        /// <summary>
        ///     Initializes list of cooks and menu items.
        /// </summary>
        private void InitializeMenuAndCooks()
        {
            int menuSize = Math.Min(Constants.MenuSize, Enum.GetNames(typeof (MealTypes)).Length);
            _cooks = new List<ICook>();
            _cooksTasksArray = new Task[menuSize];
            _menu = new Dictionary<MealTypes, double>();
            

            for (int i = 0; i < menuSize; i++)
            {
                var mealType = (MealTypes) i;
                var cook = new Cook(mealType, Constants.CookingTimesMs[mealType]);
                _cooks.Add(cook);
                _menu.Add(mealType, Constants.PriceList[mealType]);

                _cooksTasksArray[i] = Task.Factory.StartNew(cook.DoWork);
                //ThreadPool.QueueUserWorkItem(obj => cook.DoWork());
            }
        }

        /// <summary>
        ///     Initializes list of cashiers.
        /// </summary>
        private void InitializeCashiers()
        {
            _cashiers = new List<ICashier>();
            _cashiersTasksArray = new Task[Constants.CashierCount];

            for (int i = 0; i < Constants.CashierCount; i++)
            {
                var cashier = new Cashier(i + 1, _cooks);
                _cashiers.Add(cashier);
                _cashiersTasksArray[i] = Task.Factory.StartNew(cashier.DoWork);
            }
        }

        /// <summary>
        ///     Generates random clients.
        /// </summary>
        private void GenerateClients()
        {
            var currentClient = new Client(_menu);
            Thread.Sleep(Constants.ClientGenerationTimeoutMs);
            currentClient.StandOnLine(_cashiers);
        }

        /// <summary>
        /// Takes McDonald's daily takings.
        /// </summary>
        private void TakeDailyTakings()
        {
            _manager.GetTakings();
        }

        /// <summary>
        ///     Shows McDonald's daily takings to Console.
        /// </summary>
        private void ShowDailyTakings()
        {
            _manager.ShowTakings();
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Starts McDonald's workflow.
        /// </summary>
        public void StartWork()
        {
            #region log4net[info]

            log.Info("McDonalds start to work.");

            #endregion

            InitializeEmployees();
            Thread.Sleep(Constants.RestTimeBeforeWorkDayMs);

            while (!IsEndOfDay)
            {
                GenerateClients();
            }
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

            #region log4net[Debug]

            log.Debug("McDonald's has finished work.");

            #endregion

            //The end of day for McDonald's.
            lock (_lockObj)
            {
                _isEndOfDay = true;
            }

            //End of the day for cashiers.
            foreach (Employee cashier in _cashiers.Cast<Employee>())
            {
                cashier.IsEndOfDay = true;
            }

            //Waiting for last clients at the cash desks.
            Task.WaitAll(_cashiersTasksArray);

            //End of the day for cooks.
            foreach (var cook in _cooks.Cast<Employee>())
            {
                cook.IsEndOfDay = true;
            }

            TakeDailyTakings();
            
            //Waits until all cooks will finish their work.
            Task.WaitAll(_cooksTasksArray);

            ShowDailyTakings();

            #region log4net[info]

            log.Info("McDonald's was closed.");

            #endregion

        }

        #endregion
    }
}