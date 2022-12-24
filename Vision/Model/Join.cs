using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision.Model
{
    public class Join
    {
        public string Table { get; set; }
        public string SecondTable { get; set; }

        public JoinType JoinType { get; set; }

        public string Column { get; set; }

        public string SecondColumn { get; set; }    
    }

    public enum JoinType {  LEFT, RIGHT, INNER }
}
