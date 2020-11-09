using GoRestClient.Models.Enums;
using System;
using Newtonsoft.Json;

namespace GoRestClient.Models
{
    public class UserModel
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public Status Status { get; set; }
        [JsonProperty("created_at")]
        public DateTimeOffset Created { get; set; }
        [JsonProperty("updated_at")]
        public DateTimeOffset Updated { get; set; }
    }
}
