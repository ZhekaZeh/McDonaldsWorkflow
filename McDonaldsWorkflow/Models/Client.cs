using System;
using System.Collections.Generic;
using System.Linq;
using McDonaldsWorkflow.Models.Enums;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public class Client
    {
        #region Private fields

        private readonly Dictionary<MealTypes, double> _menu;
        private readonly Random _random;

        #endregion

        #region Properties

        public int ClientId { get; private set; }

        public Dictionary<MealTypes, int> Order { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="Client" /> class.
        /// </summary>
        /// <param name="menu">McDonald's menu.</param>
        public Client(Dictionary<MealTypes, double> menu)
        {
            _random = new Random();
            _menu = menu;
            Order = new Dictionary<MealTypes, int>();
            AssignId();
            GenerateOrder();
        }

        #endregion

        #region Private methods

        /// <summary>
        ///     Assigns Id for current client.
        /// </summary>
        private void AssignId()
        {
            ClientId = _random.Next(Constants.TheRangeOfIdValues);
            Console.WriteLine(@"Client {0} went to McDonalds.", ClientId);
        }

        /// <summary>
        ///     Generates random order appropriate to menu.
        /// </summary>
        private void GenerateOrder()
        {
            for (int i = 0; i < _menu.Count; i++)
            {
                var mealType = (MealTypes) i;
                Order.Add(mealType, _random.Next(Constants.MinMealCountClientOrder, Constants.MaxMealCountClientOrder));
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