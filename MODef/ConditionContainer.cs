using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.MODef
{
    public class ConditionContainer : IConditionType
    {
        public Dictionary<int, IConditionType> Conditions { get; set; }
        public ConditionContainerType Type { get; set; }
        private int NextPosition 
        { 
            get
            {
                return Conditions.Count > 0 ? Conditions.Max(d => d.Key) + 1 : 0;
            }
        }

        public ConditionContainer()
            : this(ConditionContainerType.And)
        {

        }

        public ConditionContainer(ConditionContainerType type) 
        {
            Conditions = new Dictionary<int, IConditionType>();
            Type = type;
        }

        public void AddCondition(Condition condition)
        {
            AddCondition(condition, NextPosition);
        }

        public void AddCondition(Condition condition, int position)
        {
            Conditions.Add(position, condition);
        }

        public void AddConditionContainer(ConditionContainer conditionContainer)
        {
            AddConditionContainer(conditionContainer, NextPosition);
        }

        public void AddConditionContainer(ConditionContainer conditionContainer, int position)
        {
            Conditions.Add(position, conditionContainer);
        }

        /// <summary>
        /// Creates a ConditionContainer and returns a reference
        /// </summary>
        /// <returns></returns>
        public ConditionContainer CreateAndAddConditionContainer()
        {
            ConditionContainer res = new ConditionContainer();
            AddConditionContainer(res);
            return res;
        }

    }

    public enum ConditionContainerType
    {
        And,
        Or
    }
}
