using GoRestClient.Core;
using GoRestClient.Models;
using GoRestClient.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
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
        public async Task<SearchResultModel> Search(string nameFilter, uint page)
        {
            var parameters = new StringBuilder("?");
            parameters.Append($"page={page};");
            parameters.Append(!string.IsNullOrWhiteSpace(nameFilter) ? $"name={nameFilter}" : String.Empty);

            string url = $"{ApiResourceName}{parameters}";
            var response = await
                _restProvider.GetAsync<ResponseModel>(url);
            var searchResult = new SearchResultModel
            {
                Pagination = response.Meta.ToObject<SearchResultModel>().Pagination,
                Records = response.Data.ToObject<IEnumerable<UserModel>>()
            };

            return searchResult;
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
