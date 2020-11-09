using System;
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

        public UserService(IRestProvider restProvider)
        {
            _restProvider = restProvider;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<UserModel>> Search(string nameFilter)
        {
            string parameters = !string.IsNullOrWhiteSpace(nameFilter) ? $"?name={nameFilter}" : String.Empty;
            string url = $"{ApiResourceName}{parameters}";
            var searchResult = await
                _restProvider.GetAsync<ResponseModel>(url);
             return searchResult.Data.ToObject<IEnumerable<UserModel>>();
        }

        ///<inheritdoc/>
        public async Task<UserModel> Create(UserModel userToBeCreated)
        {
            var response = await 
                _restProvider.PostAsync<ResponseModel, UserModel>(ApiResourceName, userToBeCreated);
            return response.Data.ToObject<UserModel>();
        }

        ///<inheritdoc/>
        public async Task<UserModel> Update(UserModel userToBeUpdated)
        {
            string url = $"{ApiResourceName}/{userToBeUpdated.Id}";
            var response = await
                _restProvider.PatchAsync<ResponseModel, UserModel>(url, userToBeUpdated);
            return response.Data.ToObject<UserModel>();
        }

        ///<inheritdoc/>
        public async Task Delete(uint id)
        {
            string url = $"{ApiResourceName}/{id}";
            await _restProvider.DeleteAsync<ResponseModel>(url);
        }
    }
}
