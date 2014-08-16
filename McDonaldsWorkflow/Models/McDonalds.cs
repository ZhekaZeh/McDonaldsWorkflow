using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDonaldsWorkflow.Models
{
    class McDonalds
    {
        #region Private fields

        private static McDonalds _instance;

        #endregion

        #region Constructor

        private McDonalds() { }

        #endregion

        #region Properties

        public List<ICashier> Cashiers { get; set; }

        public List<ICook> Cooks { get; set; }

        public McDonalds Instance 
        {
            get
            {
                if (_instance != null) return _instance;
                else return _instance = new McDonalds();
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

        #endregion
    }
}
