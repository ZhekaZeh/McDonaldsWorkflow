using System;

namespace McDonaldsWorkflow.Models
{
    public static class Manager
    {
        #region Private fields

        private static double _allTakings;
        private static readonly object _lock;

        #endregion

        #region Static Constructor

        static Manager()
        {
            _allTakings = 0.0;
            _lock = new object();
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Gets takings from all cashiers.
        /// </summary>
        public static void GetTakings(double cashierTakings)
        {
            lock (_lock)
            {
                _allTakings += cashierTakings;
            }
        }

        /// <summary>
        ///     Shows McDonald's daily takings.
        /// </summary>
        public static void ShowTakings()
        {
            Console.WriteLine(@"****************** McDonald's daily takings: {0} ******************", _allTakings);
        }

        #endregion
    }
}