using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public class Client
    {
        #region Private fields


        #endregion


        #region Properties
        public Dictionary<MealTypes, int> Order { get; set; }

        //For test only MealCount. Must be Menu instead MealCount
        public int MealCount { get; set; }

        public int ClientId { get; set; }
        #endregion

        public Client(int mealCount, int id)
        {
            MealCount = mealCount;
            ClientId = id;
        }

        #region Public methods

        public void MakeOrder()
        {

        }

        /// <summary>
        ///     Chooses shortest line and stands on it. 
        ///     Also it calls cashier's method StandOnLine which adds him to queue.
        /// </summary>
        /// <param name="cashiers"></param>
        public void StandOnLine(List<ICashier> cashiers)
        {
            var chosenCachier = cashiers.OrderBy(cashier => cashier.LineCount).First();

            chosenCachier.StandOnLine(this);
        }

        #endregion
    }
}
