﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private Dictionary<MealTypes, double> _menu;

        #endregion

        #region Constructor

        private McDonalds()
        {
            _isEndOfDay = false;
            _lockObj = new object();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Implemetation of Singelton pattern with tread-safe construction.
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
        ///     Initializes lists with Cashiers and Cooks (Emloyees). For test it creates only one cashier and one cook
        /// </summary>
        private void InitializeEmployees()
        {
            InitializeMenuAndCooks();
            InitializeCashiers();
        }

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
        ///     Logic for generate random clients
        /// </summary>
        private void GenerateClients()
        {
            var rnd = new Random();
            int tempId = rnd.Next(100);
            var currentClient = new Client(_menu, tempId);

            Console.WriteLine(@"Client {0} went to McDonalds.", tempId);
            Thread.Sleep(Constants.ClientGenerationTimeoutMs);
            currentClient.StandOnLine(_cashiers);
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Starts McDonald's workflow
        /// </summary>
        public void StartWork()
        {
            Console.WriteLine(@"---Employees are preparing to work---");
            InitializeEmployees();
            Thread.Sleep(Constants.RestTimeBeforeWorkDayMs);
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
            Console.WriteLine(@"McDonalds is closing...");
            lock (_lockObj)
            {
                _isEndOfDay = true;

                //Get the list of all employees from cooks and cashiers
                IEnumerable<IEmployee> emplyees = _cooks.Concat(_cashiers.Cast<IEmployee>());

                foreach (IEmployee employee in emplyees)
                {
                    employee.IsEndOfDay = _isEndOfDay;
                }
            }
        }

        #endregion
    }
}