namespace McDonaldsWorkflow.Models.Interfaces
{
    public interface ICook : IEmployee
    {
        MealTypes MealType { get; }
        bool TryGetMeals(int requestedCount, out int takenCount);
    }
}