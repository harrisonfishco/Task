using Microsoft.EntityFrameworkCore;
using Task.ModelObjects;

public delegate void SaveHandler<T>(object sender, Task.Context context, T entity) where T : ModelObject;

namespace Task.Components.Layout
{
    public partial class ModelLayout<T> where T : ModelObject
    {
        public event SaveHandler<T> BeforeSave;
        public event SaveHandler<T> AfterSave;

        protected Mode CurrentMode { get; set; } = Mode.Find;

        public void SetIdentity(string? identity)
        {
            if(TypeCheck.NotEmpty(identity))
            {
                if (Guid.TryParse(identity, out Guid id))
                {
                    this.Identity = id;
                }
                CurrentMode = Mode.Update;
            }
        }

        public void SetIdentity(Guid? identity)
        {
            if(TypeCheck.NotEmpty(identity))
            {
                this.Identity = identity;
                CurrentMode = Mode.Update;
            }
        }

        public Guid? GetIdentity()
        {
            return this.Identity;
        }

        /// <summary>
        /// Sets PropertyValues to the default value of an empty T
        /// </summary>
        private void SetDefaultProperties()
        {
            object? obj;
            T entity;
            try
            {
                obj = Activator.CreateInstance(typeof(T));
                if(TypeCheck.NotEmpty(obj))
                {
                    entity = (T)obj;

                    foreach(string property in PropertyValues.Keys)
                    {
                        PropertyValues[property] = typeof(T).GetProperty(property)!.GetValue(entity)!.ToString()!;
                    }
                }
                else
                {
                    foreach (string property in PropertyValues.Keys)
                    {
                        PropertyValues[property] = string.Empty;
                    }
                }
            } catch(Exception ex) { TaskError.HandleError(ex); }
        }

        /// <summary>
        /// Grab the title of the page depending on the current Mode
        /// </summary>
        /// <returns></returns>
        protected string GetTitle()
        {
            string res = string.Empty;
            switch(CurrentMode)
            {
                default:
                case Mode.Find:
                    res = $"Find {ModelObject.GetNamePlural<T>()}";
                    break;
                case Mode.Insert:
                    res = $"Add {ModelObject.GetName<T>()}";
                    break;
                case Mode.Update:
                    res = $"Update {ModelObject.GetName<T>()}";
                    break;
            }
            return res;
        }
    }

    public enum Mode
    {
        Find,
        Update,
        Insert
    }
}
