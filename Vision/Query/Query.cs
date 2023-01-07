using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision.Bases;
using Vision.Model;
using Vision.Model.QueryState;

namespace Vision.Query
{
    public partial class Query
    {
        private string Table = "";
        private string query;
        private StateObject State;
        public Query(string tableName)
        {
            this.Table = tableName;
            this.State = new StateObject();
        }

        public Query Select(IEnumerable<string> columns)
        {
            this.State.QueryStarted = true;
            QAdd(BaseSelect(this.Table, columns));
            return this;
        }

        public Query StartCondition()
        {
            this.State.ConditionStarted = true;
            QAdd(SqlBaseConstants.Where);
            return this;
        }

        public Query MultipleCondition(List<Condition> conditions)
        {
            string cQ = string.Empty;
            this.State.ConditionStarted = true;
            foreach (var condition in conditions)
            {
                cQ += ConditionToString(condition);
            }

            QAdd(cQ);
            return this;
        }

        public Query WhereIn(string column, string[] values, string op = "")
        {
            if (values.Length == 0)
                throw new StateException("Values empty");

            string query =
                this.State.ConditionStarted ? " " + column + " IN" + " {0} " : SqlBaseConstants.Where + " " + column + " IN {0} ";

            string inValue = "(";

            for (int i = 0; i < values.Length; i++)
            {
                string checkComma = values[i] == values[values.Length - 1] ? ")" : " , ";
                inValue += $"'{values[i]}'{checkComma}";
            }
            inValue = inValue + " " + op;
            query = string.Format(query, inValue);

            this.State.ConditionStarted = true;

            QAdd(query);

            return this;
        }

        /// <summary>
        /// {joinType} JOIN {table} ON {column} = {secondColumn}
        /// For Example: LEFT JOIN Category  ON [P].categoryId = [C].Id
        /// </summary>
        /// <param name="joins">tables to merge</param>  
        public Query Join(IEnumerable<Join> joins)
        {
            if (this.State.ConditionStarted)
                throw new StateException("Join started before condition");

            string fullJoin = "";
            foreach (var join in joins)
            {
                fullJoin += string.Format("{0} JOIN {1} ON {2} ", join.JoinType, join.SecondTable, join.Column, join.SecondColumn);
            }
            QAdd(fullJoin);
            return this;
        }


        /// <summary>
        /// {joinType} JOIN {table} ON {column} = {secondColumn}
        /// For Example: LEFT JOIN Category  ON [P].categoryId = [C].Id
        /// </summary>
        /// <param name="table">table to merge</param> 
        /// <param name="joinType">Join type "LEFT", "RIGHT", "INNER"</param> 
        /// <param name="column">Main table column</param> 
        /// <param name="secondColumn">table column to join</param> 
        /// <returns>this object</returns>
        public Query Join(string table, string joinType, string column, string secondColumn)
        {
            if (this.State.ConditionStarted)
                throw new StateException("Join started before condition");

            string join = $" {joinType} JOIN  {table} ON {column} = {secondColumn}";
            QAdd(join);
            return this;
        }


        /// <summary>
        /// ORDER BY <order.column> <order.orderType>
        /// For Example: ... ORDER BY Name DESC
        /// </summary>
        /// <param name="order">Object to sort</param> 
        /// <returns></returns>
        public Query Order(Order order)
        {
            if (!this.State.QueryStarted)
                throw new StateException("Query is not started");

            StringBuilder sb = new StringBuilder();

            sb.Append(SqlBaseConstants.ORDERBY);
            sb.Append(order.Column);
            sb.Append(" " + order.OrderType.ToString());

            QAdd(sb.ToString());

            return this;
        }

        /// <summary>
        /// ORDER BY <column> <orderType>
        /// For Example: ... ORDER BY Name ASC
        /// </summary>
        /// <param name="column">Order of column</param>
        /// <param name="orderType">Order Type ASC or DESC</param>
        /// <returns></returns>
        public Query Order(string column, OrderType orderType)
        {
            Order order = new Order(column, orderType);
            Order(order); 
            return this;
        }


        /// <summary>
        /// WHERE (<CONDITIONGROUP>)
        /// For Example: ... WHERE (State = 1 OR STATE = 2) AND STATE = 4
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public Query ConditionGroup(List<ConditionGroup> groups)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" (");
            foreach (var group in groups)
            {
                foreach (var condition in group.Conditions)
                {
                    builder.Append(ConditionToString(condition));
                    this.State.ConditionStarted = true;
                }
            }
            builder.Append(")");
            QAdd(builder.ToString());
            return this;
        }

        /// <summary>
        /// WHERE (<CONDITIONGROUP>)
        /// For Example: WHERE (State = 1 OR STATE = 2) AND STATE = 4
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public Query ConditionGroup(ConditionGroup groups)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" ("); 
            foreach (var condition in groups.Conditions)
            {
                builder.Append(ConditionToString(condition));
                this.State.ConditionStarted = true;
            }

            builder.Append(")");
            QAdd(builder.ToString());
            return this;
        }

        public Query SingleCondition(Condition condition)
        {
            this.State.ConditionStarted = true;
            QAdd(ConditionToString(condition));
            return this;
        }

        public string Q()
        {
            this.State.QueryCompleted = true;
            return this.query;
        }

        public StateObject QState()
        {
            return this.State;
        }

        private string BaseSelect(string table, IEnumerable<string> columnsArr)
        {
            string columns = string.Join(",", columnsArr);
            return string.Format(SqlBaseConstants.BaseSelect, columns, this.Table); ;
        }

        private string QAdd(string query)
        {
            return this.query += query;
        }
        private string ConditionToString(Condition condition)
        {
            string startCondition = this.State.ConditionStarted ? "" : "WHERE";
            return string.Format(" {0} {1} {2} {3} '{4}'", startCondition, ResolveCondition(condition.ConditionType), condition.Column, condition.Op, condition.ColumValue);
        }
        private string ResolveCondition(ConditionType t)
        {
            if (t == ConditionType.EMPTY)
                return string.Empty;

            return t.ToString();
        }

    }
}
