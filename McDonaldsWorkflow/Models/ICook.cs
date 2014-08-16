using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDonaldsWorkflow.Models
{
    interface ICook
    {
        MealsEnum MealType { get; set; }

        void TryGetMeals();

        int MealCount { get; set; }
    }

    
}
