using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision.Model.QueryState
{
    public partial class StateObject
    {
        public bool ConditionStarted { get; set; } 
        public bool QueryStarted { get; set; } 
        public bool QueryCompleted { get; set; }
        
    }

    public partial class StateException : Exception
    {
        public StateException()
        {

        }
        public StateException(string message) : base(message)
        {

        }
    }
}
