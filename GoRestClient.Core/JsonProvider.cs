using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace GoRestClient.Core
{
    /// <inheritdoc cref="IJsonProvider" />
    public class JsonProvider : IJsonProvider
    {
        /// <summary>
        /// General serialization settings for the application.
        /// </summary>
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter()
            }
        };

        ///<inheritdoc/>
        public TOutput Deserialize<TOutput>(string value)
        {
            return JsonConvert.DeserializeObject<TOutput>(value, _settings);
        }

        ///<inheritdoc/>
        public string Serialize<TInput>(TInput target)
        {
            return JsonConvert.SerializeObject(target, Formatting.None, _settings);
        }
    }
}