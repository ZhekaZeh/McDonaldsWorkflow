using System.Collections.Concurrent;
using System.Collections.Generic;

namespace McDonaldsWorkflow.Models
{
    public class Client
    {
        #region Properties
        public Dictionary<MealTypes, int> Order { get; set; }

        //for test
        public int MealCount { get; set; }
        #endregion

        public Client(int mealCount)
        {
            MealCount = mealCount;
        }

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
