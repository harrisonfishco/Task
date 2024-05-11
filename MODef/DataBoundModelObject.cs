using MODef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.MODef
{
    public abstract class DataBoundModelObject : ModelObject
    {
        public ConditionContainer RootConditionContainer { get; }
        public string IdentityProperty { get; }

        protected string table;

        protected List<string> queryProperties;

        private Dictionary<string, string> properties;

        public bool DataRead { get; protected set; }


        public DataBoundModelObject(string indentityProperty, string table)
        {
            IdentityProperty = indentityProperty;

            RootConditionContainer = new ConditionContainer();

            this.table = table;
            queryProperties = new List<string>();
            properties = new Dictionary<string, string>();
            DataRead = false;

            AddProperty(IdentityProperty);
        }

        public void AddProperty(string propertyName)
        {
            queryProperties.Add(propertyName);
        }

        public void AddProperties(string[] propertyList)
        {
            queryProperties.AddRange(propertyList);
        }

        public string GetProperty(string propertyName)
        {
            string res = string.Empty;

            if (properties.ContainsKey(propertyName))
            {
                res = properties[propertyName];
            }
            return res;
        }

        public void SetProperty(string propertyName, string value) 
        {
            properties[propertyName] = value;
        }

        public void AddCondition(Condition condition)
        {
            RootConditionContainer.AddCondition(condition);
        }

        public void AddCondition(Condition condition, int position)
        {
            RootConditionContainer.AddCondition(condition, position);
        }

        public void AddConditionContainer(ConditionContainer container)
        {
            RootConditionContainer.AddConditionContainer(container);
        }

        public void AddConditionContainer(ConditionContainer container, int position)
        {
            RootConditionContainer.AddConditionContainer(container, position);
        }

        public virtual bool FindMany()
        {
            Query query = new Query(table, RootConditionContainer, queryProperties);
            return false;
        }

        public virtual bool Find()
        {
            return false;
        }

        public virtual void GetNextRow()
        {

        }
    }
}
