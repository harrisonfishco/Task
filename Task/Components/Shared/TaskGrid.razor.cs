using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Task.Extensions;
using Task.ModelObjects;

namespace Task.Components.Shared
{
    public partial class TaskGrid
    {
        /// <summary>
        /// Creates tool tips on grid headers for developers. 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        internal static string GetTitle(Type type, PropertyInfo prop)
        {
            string res = string.Empty;
#if DEBUG
            res = $"{type.FullName}.{prop.Name}";
#endif 
            return res;
        }
    }

    internal class TaskGridRow<T> where T : ModelObject
    { 
        public T Row { get; private set; } 

        public List<TaskGridProperty<T>> Properties { get; } = new List<TaskGridProperty<T>>();

        public bool Changed
        { 
            get
            {
                return Properties.Select(p => p.Changed).Any(c => c);
            } 
        }

        public TaskGridRow(T row, List<PropertyInfo> properties)
        {
            Row = row;
            if(TypeCheck.NotEmpty(properties))
            {
                properties.ForEach(p =>
                {
                    Properties.Add(new TaskGridProperty<T>(p, row));
                });
            }
        }

        public void SetValue(PropertyInfo property, string value)
        {
            if(Properties.Select(p => p.Property).Contains(property))
            {
                Properties.Where(p => p.Property == property).First().Value = value;
            }
        }

        public void Undo()
        {
            foreach(TaskGridProperty<T> prop in Properties)
            {
                prop.Undo();
            }
        }
    }

    internal class TaskGridProperty<T> where T : ModelObject
    { 
        public PropertyInfo Property { get; set; }

        private T Obj { get; }

        public bool Changed
        { 
            get
            {
                return OldValue != Value;
            } 
        }

        public string Value
        {
            get
            {
                return Property.GetValue(Obj)!.ToString()!;
            }
            set
            {
                Property.SetValue(Obj, value);
            }
        }
        public string OldValue { get; private set; }

        public TaskGridProperty(PropertyInfo property, T obj) 
        {
            Property = property;
            Obj = obj;

            object? value = property.GetValue(obj);

            if(TypeCheck.NotEmpty(value))
            {
                OldValue = Value = value.ToString()!;
            }
            else
            {
                OldValue = Value = string.Empty;
            }
        }

        public void Undo()
        {
            Value = OldValue;
        }
    }
}