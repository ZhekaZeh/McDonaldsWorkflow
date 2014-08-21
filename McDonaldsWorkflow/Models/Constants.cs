using System.Collections.Generic;

namespace McDonaldsWorkflow.Models
{
    public class Constants
    {
        public const int MaxMealCount = 10;
        public const int InitialMealCount = 10;
        public const int MealGrabTimeMs = 1000;
        public const int InitialTakings = 0;
        public const int CashierCount = 3;
        public const int MenuSize = 2;
        public const int ClientGenerationTimeoutMs = 500;

        #region Dictionaries

        public static readonly Dictionary<MealTypes, int> CookingTimesMs = new Dictionary<MealTypes, int>()
        {
            {MealTypes.Hamburger, 300},
            {MealTypes.Cheeseburger, 1500},
            {MealTypes.MacChicken, 1000},
            {MealTypes.ChippedPotato, 2000},
            {MealTypes.IceCream, 1500}
        };

        public static readonly Dictionary<MealTypes, double> PriceList = new Dictionary<MealTypes, double>
        {
            {MealTypes.Hamburger, 7.5},
            {MealTypes.Cheeseburger, 8.5},
            {MealTypes.MacChicken, 15},
            {MealTypes.ChippedPotato, 9},
            {MealTypes.IceCream, 2}
        };

        #endregion
    }
}
