using System;
using System.Collections.Generic;
using System.Linq;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public class Client
    {
        #region Private fields

        private readonly Dictionary<MealTypes, double> _menu;

        #endregion

        #region Properties

        public int ClientId { get; private set; }

        public Dictionary<MealTypes, int> Order { get; private set; }

        #endregion

        #region Constructor

        public Client(Dictionary<MealTypes, double> menu, int id)
        {
            ClientId = id;
            _menu = menu;
            Order = new Dictionary<MealTypes, int>();
            GenerateOrder();
        }

        #endregion

        #region Public methods

        /// <summary>
        ///     Generates random order appropriate to menu
        /// </summary>
        public void GenerateOrder()
        {
            var random = new Random();
            for (int i = 0; i < _menu.Count; i++)
            {
                var mealType = (MealTypes) i;
                Order.Add(mealType, random.Next(Constants.MinMealCountClientOrder, Constants.MaxMealCountClientOrder));
            }
        }

        /// <summary>
        ///     Chooses shortest line and stands on it.
        ///     Also it calls cashier's method StandOnLine which adds him to queue.
        /// </summary>
        /// <param name="cashiers"></param>
        public void StandOnLine(List<ICashier> cashiers)
        {
            ICashier chosenCachier = cashiers.OrderBy(cashier => cashier.LineCount).First();

            chosenCachier.StandOnLine(this);
        }

        #endregion
    }
}