using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.MODef;

namespace MODef
{
    internal class Query
    {
        private string sqlCommand;
        public Query(string tableName, ConditionContainer container, List<string> properties)
        {
            ConstructQuery(tableName, container, properties);
        }

        private void ConstructQuery(string tableName, ConditionContainer container, List<string> properties)
        {
            sqlCommand = "SELECT ";

            sqlCommand += properties.Count > 0 ? String.Join(", ", properties) : "*";

            sqlCommand += $" FROM {tableName}";

            if(container.Conditions.Count > 0)
            {
                sqlCommand += " WHERE ";

                List<string> stringifiedConditions = new List<string>();

                foreach (IConditionType iCondition in container.Conditions.OrderBy(d => d.Key).Select(d => d.Value))
                {
                    if(iCondition is ConditionContainer cntr)
                    {
                        stringifiedConditions.Add($"({ConstructCondition(cntr)})");
                    }
                    else if(iCondition is Condition cond)
                    {
                        stringifiedConditions.Add($"({cond.Property} {Condition.ConditionTypeToString(cond.Type)} {cond.Value})");
                    }
                }

                sqlCommand += String.Join($" {(container.Type == ConditionContainerType.And ? "AND" : "OR")} ", stringifiedConditions);
            }
        }

        private string ConstructCondition(ConditionContainer container)
        {
            string res = string.Empty;

            List<string> stringifiedConditions = new List<string>();

            foreach (IConditionType iCondition in container.Conditions.OrderBy(d => d.Key).Select(d => d.Value))
            { 
                if(iCondition is ConditionContainer cntr)
                {
                    stringifiedConditions.Add($"({ConstructCondition(cntr)})");
                }
                else if(iCondition is Condition cond)
                {
                    stringifiedConditions.Add($"({cond.Property} {Condition.ConditionTypeToString(cond.Type)} {cond.Value})");
                }
            }

            res = String.Join($" {(container.Type == ConditionContainerType.And ? "AND" : "OR")} ", stringifiedConditions);

            return res;
        }
    }
}
