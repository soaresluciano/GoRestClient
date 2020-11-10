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
        private readonly IStatusManager _statusManager;

        public UserService(
            IRestProvider restProvider,
            IStatusManager statusManager)
        {
            _restProvider = restProvider;
            _statusManager = statusManager;
        }

        public void Dispose()
        {
            _restProvider?.Dispose();
        }

        ///<inheritdoc/>
        public async Task<SearchResultModel> Search(string nameFilter, uint page)
        {
            try
            {
                var parameters = new StringBuilder("?");
                parameters.Append($"page={page};");
                parameters.Append(!string.IsNullOrWhiteSpace(nameFilter) ? $"name={nameFilter}" : String.Empty);

                string url = $"{ApiResourceName}{parameters}";
                var response = await
                    _restProvider.GetAsync<ResponseModel>(url);
                var searchResult = new SearchResultModel
                {
                    Pagination = response.Meta.ToObject<SearchResultModel>()?.Pagination,
                    Records = response.Data.ToObject<IEnumerable<UserModel>>()
                };

                return searchResult;
            }
            catch (Exception e)
            {
                _statusManager.ReportException("Failed to fetch the results on the service.", e); ;
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task<UserModel> Create(UserModel userToBeCreated)
        {
            try
            {
                var response = await
                    _restProvider.PostAsync<ResponseModel, UserModel>(ApiResourceName, userToBeCreated);
                return response.Data.ToObject<UserModel>();
            }
            catch (Exception e)
            {
                _statusManager.ReportException("Failed to create the user on the service.", e);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task<UserModel> Update(UserModel userToBeUpdated)
        {
            try
            {
                string url = $"{ApiResourceName}/{userToBeUpdated.Id}";
                var response = await
                    _restProvider.PatchAsync<ResponseModel, UserModel>(url, userToBeUpdated);
                return response.Data.ToObject<UserModel>();
            }
            catch (Exception e)
            {
                _statusManager.ReportException("Failed to update the user on the service.", e);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task Delete(uint id)
        {
            try
            {
                string url = $"{ApiResourceName}/{id}";
                await _restProvider.DeleteAsync<ResponseModel>(url);
            }
            catch (Exception e)
            {
                _statusManager.ReportException("Failed to delete the user on the service.", e);
                throw;
            }
        }
    }
}
