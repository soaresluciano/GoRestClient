using GoRestClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoRestClient.Services
{
    /// <summary>
    /// Service responsible for all the CRUD operations with the User entity.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get the list of the users that match the given criteria.  
        /// </summary>
        /// <param name="nameFilter">Search criteria for the field name.</param>
        /// <returns>List of users that match the search criteria.</returns>
        Task<SearchResultModel> Search(string nameFilter);
        
        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="userToBeCreated">Details of the user to be created.</param>
        /// <returns>Register as created on the service.</returns>
        Task<UserModel> Create(UserModel userToBeCreated);

        /// <summary>
        /// Update the details of an existing user.
        /// </summary>
        /// <param name="userToBeUpdated">Details of the user to be updated.</param>
        /// <returns>Register after the information update.</returns>
        Task<UserModel> Update(UserModel userToBeUpdated);

        /// <summary>
        /// Delete an user with the given Id.
        /// </summary>
        /// <param name="id">Id of the user to be deleted.</param>
        Task Delete(uint id);
    }
}
