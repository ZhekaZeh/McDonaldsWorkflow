using System.Threading;

namespace McDonaldsWorkflow.Models
{
    interface ICashier
    {
        int Takings { get; set; }

        int LineCount { get; set; }
        AutoResetEvent WaitHandle { get; }
    }
}
