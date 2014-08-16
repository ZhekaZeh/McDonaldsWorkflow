using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDonaldsWorkflow.Models
{
    internal class Cashier : ICashier
    {
        #region Properties

        private Queue<Client> Line { get; set; }
        public bool EndOfDay { get; set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Cashier has a rest
        /// </summary>
        private void Rest()
        {

        }

        #endregion

        #region Public Methods

        public void GetMoney()
        {

        }

        public void TryToGatherOrder()
        {

        }

        #endregion

        #region ICashier implementation
        public int Takings
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int LineCount
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }

}
