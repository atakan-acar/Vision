using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision.Bases;
using Vision.Model;

namespace Vision.Query
{
    public partial class Query
    {
        private string Table = "";
        private string query; 

        public Query(string tableName)
        {
            this.Table = tableName;
        }

        public Query Select(IEnumerable<string> columns)
        {
            QAdd(BaseSelect(this.Table, columns));
            return this;
        } 

        public Query StartCondition()
        {
            QAdd(SqlBaseConstants.Where);
            return this;
        } 

        public Query AddMultipleCondition(List<Condition> conditions)
        {
            string cQ = string.Empty;
            foreach (var condition in conditions)
            {
                cQ += ConditionToString(condition);
            }

            QAdd(cQ);
            return this;
        }

        public Query AddSingleCondition(Condition condition)
        {  
            QAdd(ConditionToString(condition));
            return this;
        } 

        public string Q()
        {
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
            return string.Format(" {0} {1} {2} '{3}'", ResolveCondition(condition.ConditionType), condition.Column, condition.Op, condition.ColumValue);
        }
        private string ResolveCondition(ConditionType t)
        {
            if (t == ConditionType.EMPTY)
                return string.Empty;

            return t.ToString();
        }
    }
}
