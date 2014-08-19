using System;
using System.Collections.Generic;

namespace McDonaldsWorkflow.Models
{
    public class Cashier : Employee, ICashier
    {
        #region Properties

        private Queue<Client> Line { get; set; }
        public bool EndOfDay { get; set; }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Cashier has a rest
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
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int LineCount
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion
    }
}