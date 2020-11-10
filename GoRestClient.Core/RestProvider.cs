using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GoRestClient.Core
{
    /// <inheritdoc cref="IRestProvider" />
    public class RestProvider : IRestProvider
    {
        private readonly HttpClient _client;
        private readonly IJsonProvider _jsonProvider;
        private readonly IStatusManager _statusManager;

        public RestProvider(
            IConfigurationProvider configurationProvider,
            IJsonProvider jsonProvider,
            IStatusManager statusManager,
            HttpClient client)
        {
            _statusManager = statusManager;
            _jsonProvider = jsonProvider;
            _client = client;
            _client.BaseAddress = new Uri(configurationProvider.ApiUrl);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {configurationProvider.ApiToken}");
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        ///<inheritdoc/>
        public async Task<TOutput> GetAsync<TOutput>(string requestUrl)
        {
            ValidateArguments(requestUrl);
            var response = await SendRequestAsync(HttpMethod.Get, requestUrl);
            return await ReadContent<TOutput>(response);
        }

        ///<inheritdoc/>
        public async Task<TOutput> PostAsync<TOutput, TInput>(string requestUrl, TInput content)
        {
            ValidateArguments(requestUrl, content);
            var response = await SendRequestAsync(HttpMethod.Post, requestUrl, SetContent(content));
            return await ReadContent<TOutput>(response);
        }

        ///<inheritdoc/>
        public async Task<TOutput> PatchAsync<TOutput, TInput>(string requestUrl, TInput content)
        {
            ValidateArguments(requestUrl, content);
            var response = await SendRequestAsync(new HttpMethod("PATCH"), requestUrl, SetContent(content));
            return await ReadContent<TOutput>(response);
        }

        ///<inheritdoc/>
        public async Task<TOutput> DeleteAsync<TOutput>(string requestUrl)
        {
            ValidateArguments(requestUrl);
            var response = await SendRequestAsync(HttpMethod.Delete, requestUrl);
            return await ReadContent<TOutput>(response);
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string requestUrl, Action<HttpRequestMessage> beforeSend = null)
        {
            using var request = new HttpRequestMessage(method, requestUrl);
            beforeSend?.Invoke(request);
            
            try
            {
                return await _client.SendAsync(request);
            }
            catch (Exception e)
            {
                _statusManager.ReportException("Fail to send the request.", e);
                throw;
            }
        }

        private Action<HttpRequestMessage> SetContent<TInput>(TInput content)
        {
            return request =>
            {
                try
                {
                    var serializedObject = _jsonProvider.Serialize(content);
                    request.Content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                }
                catch (Exception e)
                {
                    _statusManager.ReportException("An error occurred during the serialization of the request content.", e);
                    throw;
                }
            };
        }

        private async Task<TOutput> ReadContent<TOutput>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var responseContent = await ReadAsStringAsync(response);
                    return _jsonProvider.Deserialize<TOutput>(responseContent);
                }
                catch (Exception e)
                {
                    _statusManager.ReportException("An error occurred during the deserialization of the response content.", e);
                    throw;
                }
            }

            throw new InvalidOperationException($"{response.StatusCode} : {response.ReasonPhrase}");
        }

        private async Task<string> ReadAsStringAsync(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        private void ValidateArguments(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
        }

        private void ValidateArguments<T>(string url, T content)
        {
            ValidateArguments(url);
            if (content == null) throw new ArgumentNullException(nameof(content));
        }
    }
}
