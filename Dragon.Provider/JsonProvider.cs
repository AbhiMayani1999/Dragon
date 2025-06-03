using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Dragon.Provider
{
    public static class JsonProvider
    {
        public static bool IsJson(this string data)
        {
            try { JToken.Parse(data); return true; } catch { return false; }
        }
        public static string ToJson(this object data)
        {
            return JsonConvert.SerializeObject(data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
        public static T ReadJson<T>(string location)
        {
            return File.Exists(location) ? JsonConvert.DeserializeObject<T>(EncryptionProvider.Decrypt(new StreamReader(location).ReadToEnd())) : Activator.CreateInstance<T>();
        }
        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static string ToFormattedJson(this object data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
        public static void ToJsonFile(this object data, string location)
        {
            PathProvider.DeleteFile(location);
            StreamWriter writer = new(location, append: true);
            writer.Write(data.ToJson()); writer.Dispose();
        }
        public static T FromJsonFile<T>(this object data, string location)
        {
            throw new NotImplementedException();
        }

        public static Dictionary<string, object> ToJsonKeyValue(this object data)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }
    }
}
