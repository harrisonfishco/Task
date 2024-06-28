using System.Reflection;
using System.Text.RegularExpressions;

namespace Task.ModelObjects
{
    public class ModelObject
    {
        #region DO NOT CHANGE
        public static readonly string PROPERTY_ADDTIMESTAMP = "AddTimestamp";
        public static readonly string PROPERTY_UPDATETIMESTAMP = "UpdateTimestamp";
        #endregion

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
        /// Get value of property from property name
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object? GetProperty(string propertyName)
        {
            return GetType()!.GetProperty(propertyName)!.GetValue(this);
        }

        /// <summary>
        /// Set value of property from property name
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void SetProperty(string propertyName, object? value)
        {
            GetType()!.GetProperty(propertyName)!.SetValue(this, value);
        }

        /// <summary>
        /// Get all properties from Model Object
        /// </summary>
        /// <param name="excludePkAndFk">Excludes all Guid types</param>
        /// <returns></returns>
        public List<PropertyInfo> GetProperties(PropertiesType type = PropertiesType.MemberSpecific)
        {
            List<PropertyInfo> res = new List<PropertyInfo>();

            res = this.GetType().GetProperties().ToList();

            //Remove all navigation properties
            res.RemoveAll(p => !p.PropertyType.IsPrimitive && p.PropertyType != typeof(Guid) && p.PropertyType != typeof(string));

            switch (type)
            {
                default:
                case PropertiesType.All:
                    break;
                case PropertiesType.MemberSpecific:
                    res.RemoveAll(p => typeof(ModelObject)
                        .GetProperties()
                        .Select(pr => pr.Name)
                        .Contains(p.Name));
                    break;
                case PropertiesType.PrimaryAndForiegnKeys:
                    res = res.Where(p => p.PropertyType == typeof(Guid)).ToList();
                    break;
                case PropertiesType.Default:
                    res = typeof(ModelObject)
                        .GetProperties()
                        .ToList();
                    break;
            }

            return res;
        }

        /// <summary>
        /// Get all properties from Model Object Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excludePkAndFk">Excludes all Guid types</param>
        /// <returns></returns>
        public static List<PropertyInfo> GetProperties<T>(PropertiesType type = PropertiesType.MemberSpecific) where T : ModelObject
        {
            List<PropertyInfo> res = new List<PropertyInfo>();

            T? obj = Activator.CreateInstance(typeof(T)) as T;

            if(obj != null)
            {
                res = obj.GetProperties(type);
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
        /// Formats any concatted camel case strings (eg. "FullName" -> "Full Name")
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetFormattedPropertyNameFromName(string propertyName)
        {
            if(string.IsNullOrEmpty(propertyName))
            {
                return propertyName;
            }

            return Regex.Replace(propertyName, "(?<!^)([A-Z])", " $1");
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

    public enum PropertiesType
    {
        All,
        Default, // Timestamps
        PrimaryAndForiegnKeys, // Only Guid Types
        MemberSpecific // Only defined in class (excludes Default)
    }
}
