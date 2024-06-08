using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Task.ModelObjects
{
    public class ModelObject
    {
        public DateTime? AddTimestamp { get; set; }
        public DateTime? UpdateTimestamp { get; set; }

        /// <summary>
        /// Get the user facing name (singular)
        /// </summary>
        /// <returns></returns>
        public virtual string GetName()
        {
            string res = string.Empty;

            res = this.GetType().Name;

            return res;
        }

        /// <summary>
        /// Get the user facing name (plural)
        /// </summary>
        /// <returns></returns>
        public virtual string GetNamePlural()
        {
            string res = string.Empty;

            res = $"{GetName()}s";

            return res;
        }

        /// <summary>
        /// Get all properties from Model Object
        /// </summary>
        /// <param name="excludePkAndFk">Excludes all Guid types</param>
        /// <returns></returns>
        public List<PropertyInfo> GetProperties(bool excludePkAndFk = false)
        {
            List<PropertyInfo> res = new List<PropertyInfo>();

            res = this.GetType().GetProperties().ToList();

            if(excludePkAndFk)
            {
                res = res.Where(p => p.GetType() != typeof(Guid)).ToList();
            }

            return res;
        }

        /// <summary>
        /// Get all properties from Model Object Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excludePkAndFk">Excludes all Guid types</param>
        /// <returns></returns>
        public static List<PropertyInfo> GetProperties<T>(bool excludePkAndFk = false) where T : ModelObject
        {
            List<PropertyInfo> res = new List<PropertyInfo>();

            T? obj = Activator.CreateInstance(typeof(T)) as T;

            if(obj != null)
            {
                res = obj.GetProperties(excludePkAndFk);
            }

            return res;
        }

        /// <summary>
        /// Get the user facing name of a ModelObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetName<T>() where T : ModelObject
        {
            string res = string.Empty;

            T? obj = Activator.CreateInstance(typeof(T)) as T;

            if(obj != null)
            {
                res = obj.GetName();
            }
            else
            {
                res = typeof(T).Name;
            }

            return res;
        }

        /// <summary>
        /// Get the plural user facing name of a ModelObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetNamePlural<T>() where T : ModelObject
        {
            string res = string.Empty;

            T? obj = Activator.CreateInstance(typeof(T)) as T;

            if(obj != null)
            {
                res = obj.GetNamePlural();
            }
            else
            {
                res = $"{typeof(T).Name}s";
            }

            return res;
        }
    }
}
