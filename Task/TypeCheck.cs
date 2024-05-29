using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Task
{
    public static class TypeCheck
    {
        /// <summary>
        /// Checks if object is not empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool NotEmpty([NotNull] object? obj)
        {
            bool res = false;
            if(obj is string str)
            {
                res = obj != null && str != string.Empty;
            }
            else if (obj is ICollection collection)
            {
                res = collection != null && collection.Count > 0;
            }
            else if(obj is IEnumerable enumerable)
            {
                res = enumerable != null && enumerable.GetEnumerator().MoveNext();
            }
            else if(obj is Guid guid)
            {
                res = guid != Guid.Empty;
            }
            else
            {
                res = obj != null;
            }
            return res;
        }

        /// <summary>
        /// Checks if object is empty
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Empty([MaybeNull] object? obj)
        {
            return !NotEmpty(obj);
        }
    }
}
