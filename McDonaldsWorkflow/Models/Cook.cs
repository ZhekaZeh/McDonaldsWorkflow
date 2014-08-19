using System;
using System.Threading;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public class Cook : Employee, ICook
    {
        #region Properties

        private int _mealCount;
        private readonly int _maxMealCount;
        private readonly object _lockObj;
        private readonly int _cookingTime;
        private readonly ICompany _company;
        private readonly int _grabMealTime;

        #endregion

        #region 

        /// <summary>
        /// Initializes a new instance of the <see cref="Cook" /> class.
        /// </summary>
        /// <param name="mealType">Type of the meal.</param>
        /// <param name="cookingTime">The cooking time.</param>
        public Cook(MealTypes mealType, int cookingTime)
        {
            _mealCount = Constants.InitialMealCount;
            _maxMealCount = Constants.MaxMealCount;
            _grabMealTime = Constants.MealGrabTimeMs;
            MealType = mealType;
            _cookingTime = cookingTime;
            _lockObj = new object();
            _company = McDonalds.Instance;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Cook starts to prepare his specific meal
        /// </summary>
        private void CookMeal()
        {
            Console.WriteLine(@"Starting to cook {0}...", MealType);
            Thread.Sleep(_cookingTime);
            lock (_lockObj)
            {
                _mealCount++;
                Console.WriteLine(@"Finished cooking {0}. Meal count {1}", MealType, _mealCount);
            }
        }

        /// <summary>
        /// Cook has a rest
        /// </summary>
        private void Rest()
        {
            Console.WriteLine(@"{0} cook is resting, table is full...", MealType);
            _waitHandle.WaitOne();
            Console.WriteLine(@"{0} cook finished resting.", MealType);
        }

        /// <summary>
        /// Determines whether [is table full].
        /// </summary>
        /// <returns></returns>
        private bool IsTableFull()
        {
            lock (_lockObj)
            {
                return _mealCount >= _maxMealCount;
            }
        }

        /// <summary>
        /// Goes the home.
        /// </summary>
        private void GoHome()
        {
            Console.WriteLine(@"{0} cook is going home. Bye bye", MealType);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Cook's work logic
        /// </summary>
        public void DoWork()
        {
            while (!_company.IsEndOfDay )
            {
                if (!IsTableFull())
                {
                    CookMeal();
                }
                else
                {
                    Rest();
                }
            }

            GoHome();
        }

        #endregion

        #region ICook implementation

        /// <summary>
        /// Gets the type of the meal.
        /// </summary>
        /// <value>
        /// The type of the meal.
        /// </value>
        public MealTypes MealType { get; private set; }

        /// <summary>
        /// Tries the get meals.
        /// </summary>
        /// <param name="requestedCount">The requested count.</param>
        /// <param name="takenCount">The taken count.</param>
        /// <returns></returns>
        public bool TryGetMeals(int requestedCount, out int takenCount)
        {
            Console.WriteLine(@"{0} Trying to take {1} {2} from the table...", DateTime.Now, requestedCount, MealType);
            
            lock (_lockObj)
            {
                Thread.Sleep(_grabMealTime);
                bool success;
                if (_mealCount >= requestedCount)
                {
                    _mealCount -= requestedCount;
                    takenCount = requestedCount;

                    if (_mealCount < 0)
                    {
                        throw new ArithmeticException("Negative meal count");
                    }

                    Console.WriteLine(@"{0} Success. Took all {1} meals of {2} type", DateTime.Now, takenCount, MealType);
                    success = true;
                }
                else
                {
                    takenCount = _mealCount;
                    _mealCount = 0;
                    Console.WriteLine(@"{0} Fail. Took just {1} meals of {2} type", DateTime.Now, takenCount, MealType);
                    success = false;
                }

                _waitHandle.Set();
                return success;
            }
        }
        
        #endregion
    }
}
