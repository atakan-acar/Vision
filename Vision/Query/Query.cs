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
                throw new ArgumentException("Values empty");

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

        public Query Join(IEnumerable<Join> joins)
        {
            if (this.State.ConditionStarted)
                throw new Exception("Join started before condition");

            string fullJoin = "";
            foreach (var join in joins)
            {
                fullJoin += string.Format("{0} JOIN {1} ON {2} ", join.JoinType, join.SecondTable, join.Column, join.SecondColumn);
            }
            QAdd(fullJoin);
            return this;
        }

        public Query Join(string table, string joinType, string column, string secondColumn)
        {
            if (this.State.ConditionStarted)
                throw new Exception("Join started before condition");

            string join = $" {joinType} JOIN  {table} ON {column} = {secondColumn}"; 
            QAdd(join);
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
            return string.Format(" {0} {1} {2} {3} '{4}'", startCondition ,ResolveCondition(condition.ConditionType), condition.Column, condition.Op, condition.ColumValue);
        }
        private string ResolveCondition(ConditionType t)
        {
            if (t == ConditionType.EMPTY)
                return string.Empty;

            return t.ToString();
        }
    }
}
