using System.Reflection;

namespace Dragon.Provider
{
    public struct Options { public object Key { get; set; } public string Value { get; set; } }
    public static class ConversationProvider
    {
        public static List<Options> ToOptions<K>()
        {
            if (!typeof(K).IsEnum) { throw new InvalidCastException(); }
            List<Options> options = [];
            Enum.GetValues(typeof(K)).Cast<K>().ToList().ForEach(x => { options.Add(new Options { Key = Convert.ToInt32(x), Value = Convert.ToString(x) }); });
            return options;
        }
        public static List<Options> ToStringOptions<K>()
        {
            if (!typeof(K).IsEnum) { throw new InvalidCastException(); }
            List<Options> options = [];
            Enum.GetValues(typeof(K)).Cast<K>().ToList().ForEach(x => { options.Add(new Options { Key = Convert.ToString(x), Value = Convert.ToString(x) }); });
            return options;
        }
        public static string ToCommaSeparateString(this List<int> data)
        {
            return string.Join(", ", data.Select(n => n.ToString()));
        }
        public static string ToCommaSeparateString(this List<string> data)
        {
            return data != null && data.Count > 0 ? string.Join(", ", data.Select(n => n.ToString())) : "";
        }

        public static List<int> ToInt(this List<string> stringList)
        {
            int x = 0;
            List<int> intList = stringList.Where(str => int.TryParse(str, out int x)).Select(str => x).ToList();
            return intList;
        }
        public static bool ToBool(this string data, bool defaultValue)
        {
            return !bool.TryParse(data, out bool result) ? data == "Yes" || defaultValue : result;
        }
        public static List<int> ToIntArray(this string data, string separator = ", ")
        {
            return !string.IsNullOrWhiteSpace(data) ? data.ToStringArray(separator).ToInt() : [];
        }
        public static List<string> ToStringArray(this string data, string separator = ", ")
        {
            return !string.IsNullOrWhiteSpace(data) ? [.. data.Split(separator)] : [];
        }

        public static string ToTrimString(this object data)
        {
            try { return (data.Equals(null) || data.Equals(DBNull.Value)) ? string.Empty : Convert.ToString(data).Trim(); }
            catch { return string.Empty; }
        }
        public static string ToLowerString(this object data)
        {
            try { return data == null ? string.Empty : Convert.ToString(data).ToLower(); }
            catch { return string.Empty; }
        }

        public static float UpToDecimal(this float data, int places = 2)
        {
            return (float)Math.Round(Convert.ToDecimal(data), places);
        }
        public static float UpToDecimal(this float? data, int places = 2)
        {
            return (float)Math.Round(Convert.ToDecimal(data), places);
        }

        public static List<T> FillValueFromDictionary<T>(this List<Dictionary<string, string>> keyValuePairs)
        {
            List<T> listData = [];
            keyValuePairs.ForEach(pair =>
            {
                object dataObj = Activator.CreateInstance(typeof(T));
                foreach (PropertyInfo property in dataObj.GetType().GetProperties())
                {
                    if (!property.GetAccessors(true).Any(x => x.IsStatic))
                    {
                        pair.TryGetValue(property.Name.AddSpaceBeforeCapital(), out string value);
                        if (string.IsNullOrWhiteSpace(value)) { pair.TryGetValue(property.Name, out value); }
                        property.SetValue(dataObj, value);
                    }
                }
                listData.Add((T)dataObj);
            });
            return listData;
        }
    }
}
