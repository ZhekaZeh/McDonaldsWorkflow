using System;
using System.Collections.Generic;
using System.Linq;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public class McDonalds : ICompany
    {
        #region Private fields

        //System.Lazy type guarantees thread-safe lazy-construction
        private static readonly Lazy<McDonalds> _instance = new Lazy<McDonalds>(() => new McDonalds());
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
            get { return _instance.Value; }
        }

        #endregion

        #region Private Methods

        private void InitializeEmployees()
        {
            foreach (MealTypes mealType in Enum.GetValues(typeof(MealTypes)))
            {
                _cooks.Add(new Cook(mealType, Constants.CookingTimeBurgerMs));
            }

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
           // use linq 
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