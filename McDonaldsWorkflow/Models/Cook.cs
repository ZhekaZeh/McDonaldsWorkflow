using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDonaldsWorkflow.Models
{
    class Cook : ICook
    {
        #region Properties



        #endregion

        #region Private Methods

        /// <summary>
        /// Cook starts to prepare his specific meal
        /// </summary>
        private void PrepareMeal()
        {

        }

        /// <summary>
        /// Cook has a rest
        /// </summary>
        private void Rest()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Cook's work logic
        /// </summary>
        public void DoWork()
        {

        }


        #endregion

        #region ICook implementation
        public MealsEnum MealType { get; set; }

        public int MealCount { get; set; }

        /// <summary>
        /// Tries get needed number of meal
        /// </summary>
        public void TryGetMeals()
        {
            throw new NotImplementedException();
        }

 
        
        #endregion
    }
}
