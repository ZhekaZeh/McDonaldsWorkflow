using System;

namespace McDonaldsWorkflow.Models
{
    class Manager
    {
        #region Properties

        private int DailyTakings { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets takings from all cashiers  
        /// </summary>
        public void GetTakings()
        {

        }


        #endregion

        #region Events

        /// <summary>
        /// Notifies that the current workday is over
        /// </summary>
        public event EventHandler TheDayIsOver;

        #endregion
    }
}
