using System.Threading;

namespace McDonaldsWorkflow.Models
{
    interface ICashier
    {
        int Takings { get; set; }

        int LineCount { get; set; }

        ManualResetEvent WaitHandle { get; }
    }
}
