using System.Collections.Generic;
using System.Threading;

namespace McDonaldsWorkflow.Models
{
    interface ICashier
    {
        int Takings { get; }
        int LineCount { get; }
        Queue<Client> Line { get; set; }
        AutoResetEvent WaitHandle { get; }
    }
}
