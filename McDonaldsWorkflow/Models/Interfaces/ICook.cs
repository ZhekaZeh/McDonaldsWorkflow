using System.Threading;

namespace McDonaldsWorkflow.Models
{
    interface ICook
    {
        MealTypes MealType { get; }
        bool TryGetMeals(int requestedCount, out int takenCount);
        AutoResetEvent WaitHandle { get; }
    }
}
