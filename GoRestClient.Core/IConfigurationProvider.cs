namespace GoRestClient.Core
{
    /// <summary>
    /// Provide access to the configurations of the application.
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Authentication Token.
        /// </summary>
        string ApiToken { get; }

        /// <summary>
        /// Url of the API.
        /// </summary>
        string ApiUrl { get; }
    }
}