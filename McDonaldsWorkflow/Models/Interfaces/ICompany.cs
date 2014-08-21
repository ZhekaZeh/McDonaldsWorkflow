namespace McDonaldsWorkflow.Models.Interfaces
{
    public interface ICompany
    {
        bool IsEndOfDay { get; }
        void EndTheDay();
        void StartWork();
    }
}
