using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private bool _isEndOfDay;
        private Manager _manager;
        private Dictionary<MealTypes, double> _menu;

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
            _cooks = new List<ICook>();
            _menu = new Dictionary<MealTypes, double>();
            int menuSize = Math.Min(Constants.MenuSize, Enum.GetNames(typeof (MealTypes)).Length);

            for (int i = 0; i < menuSize; i++)
            {
                var mealType = (MealTypes) i;
                var cook = new Cook(mealType, Constants.CookingTimesMs[mealType]);
                _cooks.Add(cook);
                _menu.Add(mealType, Constants.PriceList[mealType]);
                ThreadPool.QueueUserWorkItem(obj => cook.DoWork());
            }
        }

        /// <summary>
        ///     Initializes list of cashiers.
        /// </summary>
        private void InitializeCashiers()
        {
            _cashiers = new List<ICashier>();
            for (int i = 1; i <= Constants.CashierCount; i++)
            {
                var cashier = new Cashier(i, _cooks);
                _cashiers.Add(cashier);
                ThreadPool.QueueUserWorkItem(obj => cashier.DoWork());
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
            Console.WriteLine(@"---Employees are preparing to work---");
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
            Console.WriteLine(@"McDonalds is closing...");

            //The end of day for McDonald's.
            lock (_lockObj)
            {
                _isEndOfDay = true;
            }

            //The end of day for cashiers.
            foreach (Employee cashier in _cashiers.Cast<Employee>())
            {
                cashier.IsEndOfDay = true;
            }

            //Waiting for last clients at the cash desks.
            while (true)
            {
                int count = _cashiers.Count(cashier => cashier.EmployeeState == EmployeeStates.WentHome);
                if (count == _cashiers.Count) break;
            }
            TakeDailyTakings();

            //The end of day for cooks.
            foreach (Employee cook in _cooks.Cast<Employee>())
            {
                cook.IsEndOfDay = true;
            }

            //Waits until all cooks will finish their work.
            while (true)
            {
                int count = _cooks.Count(cook => cook.EmployeeState == EmployeeStates.WentHome);
                if (count == _cooks.Count) break;
            }

            ShowDailyTakings();

            Console.WriteLine(@"McDonald's was closed!" + new String('-', 30));
        }

        #endregion
    }
}