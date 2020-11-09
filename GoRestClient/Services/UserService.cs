using GoRestClient.Infrastructure;
using GoRestClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoRestClient.Services
{
    /// <inheritdoc cref="IUserService" />
    public class UserService : IUserService
    {
        private const string ApiResourceName = "users";
        private readonly IRestProvider _restProvider;
        private readonly string _apiResourceUrl;

        public UserService(
            IRestProvider restProvider,
            IConfigurationProvider configurationProvider)
        {
            _restProvider = restProvider;
            _apiResourceUrl = $"{configurationProvider.ApiUrl}{ApiResourceName}";
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<UserModel>> Search()
        {
            var searchResult = await
                _restProvider.GetAsync<ResponseModel<IEnumerable<UserModel>>>(_apiResourceUrl);
            return searchResult.Data;
        }

        ///<inheritdoc/>
        public async Task<UserModel> Get(uint id)
        {
            string url = $"{_apiResourceUrl}/{id}";
            var response = await
                _restProvider.GetAsync<ResponseModel<UserModel>>(_apiResourceUrl);
            return response.Data;
        }

        ///<inheritdoc/>
        public async Task<UserModel> Create(UserModel userToBeCreated)
        {
            var response = await 
                _restProvider.PostAsync<ResponseModel<UserModel>, UserModel>(_apiResourceUrl, userToBeCreated);
            return response.Data;
        }

        ///<inheritdoc/>
        public async Task<UserModel> Update(UserModel userToBeUpdated)
        {
            string url = $"{_apiResourceUrl}/{userToBeUpdated.Id}";
            var response = await
                _restProvider.PatchAsync<ResponseModel<UserModel>, UserModel>(url, userToBeUpdated);
            return response.Data;
        }

        ///<inheritdoc/>
        public async Task Delete(uint id)
        {
            string url = $"{_apiResourceUrl}/{id}";
            await _restProvider.DeleteAsync<UserModel>(url);
        }
    }
}
