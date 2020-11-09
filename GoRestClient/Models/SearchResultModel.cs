using System.Collections.Generic;

namespace GoRestClient.Models
{
    public class SearchResultModel
    {
        public PaginationModel Pagination { get; set; }
        public IEnumerable<UserModel> Records { get; set; }
    }
}