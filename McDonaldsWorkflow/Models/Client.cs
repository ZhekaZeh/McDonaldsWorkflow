using System.Collections.Generic;

namespace McDonaldsWorkflow.Models
{
    class Client
    {
        #region Properties

        public bool EndOfDay { get; set; }

        public Dictionary<MealTypes, int> Order { get; set; }

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
