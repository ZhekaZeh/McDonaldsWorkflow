using System.Collections.Generic;

namespace McDonaldsWorkflow.Models.Interfaces
{
    public interface ICompany
    {
        bool IsEndOfDay { get; }
        void EndTheDay();

        //List<ICook> Cooks { get; }

        void StartWork();
    }
}
