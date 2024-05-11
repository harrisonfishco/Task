using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.MODef
{
    public class Condition : IConditionType
    {
        public string Property { get; set; }
        public ConditionType Type { get; set; }
        public string Value { get; set; }

        public Condition(string property, ConditionType type, string value)
        {
            Property = property;
            Type = type;
            Value = value;
        }

        public static string ConditionTypeToString(ConditionType type)
        {
            string res = string.Empty;
            switch (type)
            {
                case ConditionType.Equal:
                    res = "=";
                    break;
                case ConditionType.NotEqual:
                    res = "<>";
                    break;
                case ConditionType.Greater:
                    res = ">";
                    break;
                case ConditionType.Less:
                    res = "<";
                    break;
                case ConditionType.GreaterOrEqual:
                    res = ">=";
                    break;
                case ConditionType.LessOrEqual:
                    res = "<=";
                    break;
                case ConditionType.In:
                    res = "IN";
                    break;
            }
            return res;
        }
    }

    public enum ConditionType
    {
        Equal,
        NotEqual,
        Greater,
        Less,
        GreaterOrEqual,
        LessOrEqual,
        In
    }
}
