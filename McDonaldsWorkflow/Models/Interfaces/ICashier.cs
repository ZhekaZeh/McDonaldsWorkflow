using System.Threading;

namespace McDonaldsWorkflow.Models.Interfaces
{
    public interface ICashier: IEmployee
    {
        int LineCount { get; }
        double Takings { get; set; }
        void StandOnLine(Client client);
    }
}
