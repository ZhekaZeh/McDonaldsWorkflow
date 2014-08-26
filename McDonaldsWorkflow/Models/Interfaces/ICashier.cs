using McDonaldsWorkflow.Models.Enums;

namespace McDonaldsWorkflow.Models.Interfaces
{
    public interface ICashier
    {
        int LineCount { get; }
        double Takings { get; }
        void StandOnLine(Client client);
        EmployeeStates EmployeeState { get; }
    }
}
