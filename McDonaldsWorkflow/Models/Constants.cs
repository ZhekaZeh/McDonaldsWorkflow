using System.Collections.Generic;
using McDonaldsWorkflow.Models.Enums;

namespace McDonaldsWorkflow.Models
{
    public class Constants
    {
        public const int MenuSize = 5;
        public const int CashierCount = 3;
        public const int MaxMealCount = 10;
        public const int InitialMealCount = 10;
        public const int TheRangeOfIdValues = 100;
        public const int MinMealCountClientOrder = 0;
        public const int MaxMealCountClientOrder = 4;

        public const int MealGrabTimeMs = 500;
        public const int RestTimeBeforeWorkDayMs = 6000;
        public const int ClientGenerationTimeoutMs = 1500;
        public const int CashierGrabMealRetryTimeoutMs = 2000;

        public const double InitialTakings = 0.0;


        #region Dictionaries

        public static readonly Dictionary<MealTypes, int> CookingTimesMs = new Dictionary<MealTypes, int>
        {
            {MealTypes.Hamburger, 500},
            {MealTypes.Cheeseburger, 800},
            {MealTypes.MacChicken, 1100},
            {MealTypes.ChippedPotato, 1400},
            {MealTypes.IceCream, 1700}
        };

        public static readonly Dictionary<MealTypes, double> PriceList = new Dictionary<MealTypes, double>
        {
            {MealTypes.Hamburger, 7.1},
            {MealTypes.Cheeseburger, 8.2},
            {MealTypes.MacChicken, 9.3},
            {MealTypes.ChippedPotato, 10.4},
            {MealTypes.IceCream, 11.5}
        };
        

        #endregion
    }
}