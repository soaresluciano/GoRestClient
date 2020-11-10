namespace GoRestClient.Services.Models
{
    public class PaginationModel
    {
        public uint Total { get; set; }
        public uint Pages { get; set; }
        public uint Page { get; set; }
        public uint Limit { get; set; }
    }
}
