using System.Threading;

namespace McDonaldsWorkflow.Models.Interfaces
{
    public interface IEmployee
    {
        bool IsEndOfDay { get; set; }
        bool IsFinishedWork { get; set; }
    }
}
