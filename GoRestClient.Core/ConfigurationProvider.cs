using System.Configuration;

namespace GoRestClient.Core
{
    /// <inheritdoc cref="IConfigurationProvider" />
    public class ConfigurationProvider : IConfigurationProvider
    {
        private string _apiKey;
        private string _apiUrl;

        ///<inheritdoc/>
        public string ApiToken
        {
            get { return _apiKey ??= GetAppSettingValue(nameof(ApiToken)); }
        }

        ///<inheritdoc/>
        public string ApiUrl
        {
            get { return _apiUrl ??= GetAppSettingValue(nameof(ApiUrl)); }
        }

        private string GetAppSettingValue(string appSettingKey)
        {
            return ConfigurationManager.AppSettings[appSettingKey];
        }
    }
}
