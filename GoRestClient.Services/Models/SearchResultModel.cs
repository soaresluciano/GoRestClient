using System.Collections.Generic;
using GoRestClient.Models;

namespace GoRestClient.Services.Models
{
    public class SearchResultModel
    {
        public PaginationModel Pagination { get; set; }
        public IEnumerable<UserModel> Records { get; set; }
    }
}