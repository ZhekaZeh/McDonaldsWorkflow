using McDonaldsWorkflow.Models.Enums;

namespace McDonaldsWorkflow.Models.Interfaces
{
    public interface ICook
    {
        MealTypes MealType { get; }
        EmployeeStates EmployeeState { get; }
        bool TryGetMeals(int requestedCount, out int takenCount);
    }
}