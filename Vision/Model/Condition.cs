using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision.Model
{
    public class Condition
    {
        /// <summary>
        /// Operator
        //  "=", "<", ">", "<=", ">=", "<>", "!=", "<=>",
        /// </summary>
        public string Op { get; set; }

        public string Column { get; set; }

        public string ColumValue { get; set; }

        public ConditionType ConditionType { get; set; }
    }

    public enum ConditionType
    {
        AND,
        OR,
        EMPTY
    }
}
