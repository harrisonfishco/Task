namespace Task
{
    public class TaskCache
    {
        private static Dictionary<string, string> cache = new Dictionary<string, string>();

        public static bool ContainsKey(string key)
        {
            return cache.ContainsKey(key);
        }

        public static string? GetKey(string key)
        {
            return cache.ContainsKey(key) ? cache[key] : null;
        }

        public static void SetKey(string key, string value)
        {
            cache[key] = value;
        }

        public static void Clear()
        {
            cache.Clear();
        }
    }
}
