using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BigGrayBison.Common.Core
{
    public class DataSerializer
    {
        public T Deserialize<T>(string json)
            => !string.IsNullOrEmpty(json)
            ? JsonConvert.DeserializeObject<T>(json, GetSerializerSettings())
            : default(T);

        public string Serialize(object target)
            => target != null
            ? JsonConvert.SerializeObject(target, GetSerializerSettings())
            : string.Empty;

        private static JsonSerializerSettings GetSerializerSettings()
            => new JsonSerializerSettings { Formatting = Formatting.None, ContractResolver = new DefaultContractResolver() };
    }
}
