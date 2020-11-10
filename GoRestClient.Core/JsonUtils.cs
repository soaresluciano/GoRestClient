using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace GoRestClient.Core
{
    /// <summary>
    /// Provider to handle Json operations. 
    /// </summary>
    public class JsonUtils
    {
        /// <summary>
        /// General serialization settings for the API.
        /// </summary>
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter()
            }
        };

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <typeparam name="TOutput">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static TOutput Deserialize<TOutput>(string value)
        {
            return JsonConvert.DeserializeObject<TOutput>(value, Settings);
        }

        /// <summary>
        /// Serializes the specified object to a JSON string
        /// using the default formatting and configuration specified for Decision Service.
        /// </summary>
        /// <typeparam name="TInput">Type of the object to be serialized.</typeparam>
        /// <param name="target">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string Serialize<TInput>(TInput target)
        {
            return JsonConvert.SerializeObject(target, Formatting.None, Settings);
        }
    }
}