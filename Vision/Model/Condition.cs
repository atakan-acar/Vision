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

        public ConditionType ConditionType { get; private set; }

        public Condition(string column, string op, string columValue, ConditionType conditionType)
        {
            Op = op;
            Column = column;
            ColumValue = columValue;
            ConditionType = conditionType;
        }
    }

    public enum ConditionType
    {
        AND,
        OR,
        EMPTY
    }
}
