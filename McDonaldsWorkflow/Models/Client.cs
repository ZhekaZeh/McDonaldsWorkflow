using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDonaldsWorkflow.Models
{
    class Client
    {
        #region Properties

        public bool EndOfDay { get; set; }

        public Dictionary<MealsEnum, int> Order { get; set; }

        #endregion

        #region Public methods

        public void MakeOrder()
        {

        }

        public void StandOnLine()
        {

        }

        public void GoAway()
        {

        }

        #endregion
    }
}
