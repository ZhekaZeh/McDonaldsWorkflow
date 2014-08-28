using System;
using System.Collections.Generic;
using log4net;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public class Manager
    {
        #region Private fields

        private readonly List<ICashier> _cashiers;
        private readonly object _lock;
        private double _allTakings;
        public static readonly ILog log = LogManager.GetLogger(typeof(Manager)); // Log4net Manager for current class

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Manager" /> class.
        /// </summary>
        /// <param name="cashiers">List of McDonald's cashiers.</param>
        public Manager(List<ICashier> cashiers)
        {
            _allTakings = Constants.InitialTakings;
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
            foreach (var cashier in _cashiers)
            {
                lock (_lock)
                {
                    _allTakings += cashier.Takings;
                }
            }
        }

        /// <summary>
        ///     Shows McDonald's daily takings.
        /// </summary>
        public void ShowTakings()
        {
            Console.WriteLine(@"McDonald's daily takings: {0}", _allTakings);
        }

        #endregion
    }
}