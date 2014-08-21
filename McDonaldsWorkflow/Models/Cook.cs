using System;
using System.Threading;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public class Cook : Employee, ICook
    {
        #region Private fields

        private int _mealCount;
        private readonly int _maxMealCount;
        private readonly int _cookingTime;
        private readonly int _grabMealTime;

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="Cook" /> class.
        /// </summary>
        /// <param name="mealType">Type of the meal.</param>
        /// <param name="cookingTime">The cooking time.</param>
        public Cook(MealTypes mealType, int cookingTime):base(String.Format("{0} cook", mealType))
        {
            _mealCount = Constants.InitialMealCount;
            _maxMealCount = Constants.MaxMealCount;
            _grabMealTime = Constants.MealGrabTimeMs;
            MealType = mealType;
            _cookingTime = cookingTime;
        }

        #endregion

        #region ICook implementation

        /// <summary>
        ///     Gets type of the meal.
        /// </summary>
        /// <value>
        /// The type of the meal.
        /// </value>
        public MealTypes MealType { get; private set; }

        /// <summary>
        ///     Tries get meals.
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

        #region Employee abstract methods implementation

        /// <summary>
        /// Determines whether the table is full or nor.
        /// </summary>
        /// <returns>true if the table is full, false otherwise</returns>
        protected override bool HasSomethingToDo()
        {
            //check if the table is full
            lock (_lockObj)
            {
                return _mealCount < _maxMealCount;
            }
        }

        /// <summary>
        /// Works this instance.
        /// </summary>
        protected override void Work()
        {
            Console.WriteLine(@"Starting to cook {0}...", MealType);
            Thread.Sleep(_cookingTime);
            lock (_lockObj)
            {
                _mealCount++;
                Console.WriteLine(@"Finished cooking {0}. Meal count {1}", MealType, _mealCount);
            }
        }

        #endregion
    }
}
