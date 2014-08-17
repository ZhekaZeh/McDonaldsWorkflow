using System.Collections.Generic;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public class McDonalds : ICompany
    {
        #region Private fields

        private static McDonalds _instance;
        private List<ICashier> _cashiers;
        private List<ICook> _cooks;
        private readonly object _lockObj;
        private bool _isEndOfDay;

        #endregion

        #region Constructor

        private McDonalds()
        {
            _isEndOfDay = false;
            _lockObj = new object();
        }

        #endregion

        #region Properties
        
        public static ICompany Instance 
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                return _instance = new McDonalds();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Logic for generate random clients
        /// </summary>
        public void GenerateClients()
        {

        }

        /// <summary>
        /// Ends the day.
        /// </summary>
        public void EndTheDay()
        {
            _isEndOfDay = true;
            //foreach (var cook in _cooks)
            //{
            //    cook.WaitHandle.Set();
            //}
        }

        #endregion

        #region ICompany implementation

        public bool IsEndOfDay {
            get
            {
                lock (_lockObj)
                {
                    return _isEndOfDay;
                }
            }
        }

        #endregion
        
    }
}
