using System.Collections.Concurrent;
using System.Collections.Generic;

namespace McDonaldsWorkflow.Models
{
    public class Client
    {
        #region Properties
        public Dictionary<MealTypes, int> Order { get; set; }

        //For test only MealCount 
        public int MealCount { get; set; }

        public int clientID { get; set; }
        #endregion

        public Client(int mealCount, int id)
        {
            MealCount = mealCount;
            clientID = id;
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
