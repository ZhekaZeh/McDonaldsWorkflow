using System.Threading;

namespace McDonaldsWorkflow.Models
{
    interface ICashier
    {
        int Takings { get; }
        int LineCount { get; }
        AutoResetEvent WaitHandle { get; }
    }
}
