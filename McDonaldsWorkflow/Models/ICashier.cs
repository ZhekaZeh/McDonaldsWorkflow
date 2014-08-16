using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDonaldsWorkflow.Models
{
    interface ICashier
    {
        int Takings { get; set; }

        int LineCount { get; set; }
    }
}
