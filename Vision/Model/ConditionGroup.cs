using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision.Model
{
    public class ConditionGroup
    {
        public List<Condition> Conditions { get; set; }

        public ConditionGroup()
        {
            Conditions = new List<Condition>();
        }
    }
}
