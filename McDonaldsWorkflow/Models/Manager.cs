using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
