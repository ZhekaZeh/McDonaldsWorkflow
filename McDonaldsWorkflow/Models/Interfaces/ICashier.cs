using System.Threading;

namespace McDonaldsWorkflow.Models.Interfaces
{
    interface ICashier
    {
        int Takings { get; }
        int LineCount { get; }
        void StandOnLine(Client client);
        AutoResetEvent WaitHandle { get; }
    }
}
