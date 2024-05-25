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
}