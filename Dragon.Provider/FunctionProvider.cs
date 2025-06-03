using System.ComponentModel;
using System.Globalization;
using System.Net.Mail;
using System.Reflection;

namespace Dragon.Provider
{
    public static class FunctionProvider
    {
        public static string GetOrigin(string data) { try { if (!string.IsNullOrWhiteSpace(data) && !data.StartsWith("http")) { data = $"http://{data}"; } Uri uri = new(data); return uri.Host; } catch { return string.Empty; } }
        public static string GetDescription<T>(this T data) where T : IConvertible { return typeof(T).IsEnum ? data.GetType().GetMember(data.ToString()).FirstOrDefault().GetCustomAttribute<DescriptionAttribute>().Description : string.Empty; }

        public static bool IsEmail(string data) { try { _ = new MailAddress(data); return true; } catch { return false; } }
        public static bool IsEqualString(string source, string compare) => source.Equals(compare, StringComparison.OrdinalIgnoreCase);

        public static object GetPropertyValue<T>(this T data, string propertyName) => data.GetType().GetProperty(propertyName).GetValue(data, null);
        public static object SetPropertyValue<T>(this T data, string propertyName, object value) { data.GetType().GetProperty(propertyName).SetValue(data, value, null); return data; }
    }

    public static class StringProvider
    {
        public static string ToLowerCase(this string data) => CultureInfo.CurrentCulture.TextInfo.ToLower(data);
        public static string ToTitleCase(this string data) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data);
        public static string ToCamelCase(this string data) => string.IsNullOrWhiteSpace(data) || data.Length < 2 ? data.ToLowerInvariant() : char.ToLowerInvariant(data[0]) + data[1..];
        public static string AddSpaceBeforeCapital(this string data) => string.Concat(data.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
    }
    public static class RandomProvider
    {
        private static readonly Random _random = new();
        public static string RandomString() { return Guid.NewGuid().ToString().Replace("-", ""); }
        public static string RandomString(int length) { return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", length).Select(s => s[_random.Next(s.Length)]).ToArray()); }
        public static string RandomNumber(int length) { return new string(Enumerable.Repeat("0123456789", length).Select(s => s[_random.Next(s.Length)]).ToArray()); }
        public static string RandomAlphabets(int length) { return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", length).Select(s => s[_random.Next(s.Length)]).ToArray()); }
        public static string RandomNumberOnDate() { return DateTime.Now.ToString("yyyyMMddHHmmssfffffff"); }
    }
}
