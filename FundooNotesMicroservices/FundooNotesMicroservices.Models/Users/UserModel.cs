using Newtonsoft.Json;

namespace FundooNotesMicroservices.Models
{
    public class UserModel
    {
        [JsonProperty("id")]
        public string Id { get; set; } = "";

        [JsonProperty("firstName")]
        public string FirstName { get; set; } = "";

        [JsonProperty("lastName")]
        public string LastName { get; set; } = "";

        [JsonProperty("mobileNumber")]
        public string MobileNumber { get; set; } = "";

        [JsonProperty("email")]
        public string Email { get; set; } = "";

        [JsonProperty("password")]
        public string Password { get; set; } = "";

    }
}