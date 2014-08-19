using System;
using System.Collections.Generic;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public class McDonalds : ICompany
    {
        #region Private fields

        //System.Lazy type guarantees thread-safe lazy-construction
        private static readonly Lazy<McDonalds> _instance = new Lazy<McDonalds>(() => new McDonalds());
        private readonly object _lockObj;
        private List<ICashier> _cashiers;
        private List<ICook> _cooks;
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
            get { return _instance.Value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Ends the day.
        /// </summary>
        public void EndTheDay()
        {
            _isEndOfDay = true;
            //foreach (var cook in _cooks)
            //{
            //    cook.WaitHandle.Set();
            //}
        }

        /// <summary>
        ///     Logic for generate random clients
        /// </summary>
        public void GenerateClients()
        {
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

        #endregion
    }
}