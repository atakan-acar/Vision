using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision.Bases
{
    public static class SqlBaseConstants
    {
        /// <summary>
        /// 0 = Columns,
        /// 1 = Table
        /// </summary>
        public const string BaseSelect = "SELECT {0} FROM {1}";
        public const string Where = " WHERE";
        public const string AND = "AND";
        public const string OR = "OR";
        public const string LEFTJOIN = "LEFT JOIN";
        public const string RIGHTJOIN = "RIGHT JOIN";
        public const string INNERJOIN = "INNER JOIN";
    }
}
