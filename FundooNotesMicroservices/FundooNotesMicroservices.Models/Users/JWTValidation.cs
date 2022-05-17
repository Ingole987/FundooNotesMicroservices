using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooNotesMicroservices.Models
{
    public class JWTValidation
    {
        public bool IsValid { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; } = "";

        [JsonProperty("email")]
        public string Email { get; set; } = "";
    }
}
