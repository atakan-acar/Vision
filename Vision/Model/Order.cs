using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision.Model
{
    public class Order
    {
        public string Column { get; private set; }
        public OrderType OrderType { get; private set; }

        public Order(string column, OrderType orderType)
        {
            Column = column;
            OrderType = orderType;
        }
    }
    public enum OrderType
    {
        ASC, DESC
    }
}
