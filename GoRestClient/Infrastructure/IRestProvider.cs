using System.Threading.Tasks;

namespace GoRestClient.Infrastructure
{
    /// <summary>
    /// Provider responsible for the REST action abstractions.
    /// </summary>
    public interface IRestProvider
    {
        /// <summary>
        /// Performs an async Get action on the REST service.
        /// </summary>
        /// <typeparam name="TOutput">Type of the response object.</typeparam>
        /// <param name="requestUrl">URL of the request to be performed.</param>
        /// <returns>Response object.</returns>
        Task<TOutput> GetAsync<TOutput>(string requestUrl);

        /// <summary>
        /// Performs an async Post action on the REST service.
        /// </summary>
        /// <typeparam name="TOutput">Type of the response object.</typeparam>
        /// <typeparam name="TInput">Type of the content object.</typeparam>
        /// <param name="requestUrl">URL of the request to be performed.</param>
        /// <param name="content">Content to be posted.</param>
        /// <returns>Response object.</returns>
        Task<TOutput> PostAsync<TOutput, TInput>(string requestUrl, TInput content);

        /// <summary>
        /// Performs an async Patch action on the REST service.
        /// </summary>
        /// <typeparam name="TOutput">Type of the response object.</typeparam>
        /// <typeparam name="TInput">Type of the content object.</typeparam>
        /// <param name="requestUrl">URL of the request to be performed.</param>
        /// <param name="content">Content to be patched.</param>
        /// <returns>Response object.</returns>
        Task<TOutput> PatchAsync<TOutput, TInput>(string requestUrl, TInput content);

        /// <summary>
        /// Performs an async Delete action on the REST service.
        /// </summary>
        /// <typeparam name="TOutput">Type of the response object.</typeparam>
        /// <param name="requestUrl">URL of the request to be performed.</param>
        /// <returns>Response object.</returns>
        Task<TOutput> DeleteAsync<TOutput>(string requestUrl);
    }
}