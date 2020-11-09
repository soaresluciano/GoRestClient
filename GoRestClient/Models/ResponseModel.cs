using Newtonsoft.Json.Linq;

namespace GoRestClient.Models
{
    public class ResponseModel
    {
        public uint Code { get; set; }
        public JToken Meta { get; set; }
        public JToken Data { get; set; }
    }
}
