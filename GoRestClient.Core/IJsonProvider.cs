namespace GoRestClient.Core
{
    /// <summary>
    /// Provider to centralize all Json related operations and configurations. 
    /// </summary>
    public interface IJsonProvider
    {
        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <typeparam name="TOutput">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        TOutput Deserialize<TOutput>(string value);

        /// <summary>
        /// Serializes the specified object to a JSON string
        /// using the default formatting and configuration specified for Decision Service.
        /// </summary>
        /// <typeparam name="TInput">Type of the object to be serialized.</typeparam>
        /// <param name="target">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        string Serialize<TInput>(TInput target);
    }
}