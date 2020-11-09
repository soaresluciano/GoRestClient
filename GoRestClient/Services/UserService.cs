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
        public async Task<IEnumerable<UserModel>> Search()
        {
            var searchResult = await
                _restProvider.GetAsync<ResponseModel<IEnumerable<UserModel>>>(ApiResourceName);
            return searchResult.Data;
        }


        ///<inheritdoc/>
        public async Task<UserModel> Create(UserModel userToBeCreated)
        {
            var response = await 
                _restProvider.PostAsync<ResponseModel<UserModel>, UserModel>(ApiResourceName, userToBeCreated);
            return response.Data;
        }

        ///<inheritdoc/>
        public async Task<UserModel> Update(UserModel userToBeUpdated)
        {
            string url = $"{ApiResourceName}/{userToBeUpdated.Id}";
            var response = await
                _restProvider.PatchAsync<ResponseModel<UserModel>, UserModel>(url, userToBeUpdated);
            return response.Data;
        }

        ///<inheritdoc/>
        public async Task Delete(uint id)
        {
            string url = $"{ApiResourceName}/{id}";
            await _restProvider.DeleteAsync<UserModel>(url);
        }
    }
}
