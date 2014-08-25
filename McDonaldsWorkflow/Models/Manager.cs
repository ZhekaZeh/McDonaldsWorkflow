using System;
using System.Collections.Generic;
using System.Linq;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public class Manager
    {
        #region Private fields

        private double _allTakings;
        private readonly object _lock;
        private readonly List<ICashier> _cashiers;

        #endregion

        #region Constructor

        public Manager(List<ICashier> cashiers)
        {
            _allTakings = 0.0;
            _lock = new object();
            _cashiers = cashiers;
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Gets takings from all cashiers.
        /// </summary>
        public void GetTakings()
        {
            while (true)
            {
                var count = 0;
                foreach (var cashier in _cashiers.Where(cashier => cashier.IsFinishedWork))
                {
                    lock (_lock)
                    {
                        _allTakings += cashier.Takings;
                        cashier.Takings = 0;
                    }
                    count++;
                }
                if (count == _cashiers.Count) break;
            }
        }

        /// <summary>
        ///     Shows McDonald's daily takings.
        /// </summary>
        public void ShowTakings()
        {
            Console.WriteLine(@"****************** McDonald's daily takings: {0} ******************", _allTakings);
        }

        #endregion
    }
}