﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesMicroservices.Models
{
    public class UserLoginModel
    {
        [JsonProperty("email")]
        public string Email { get; set; } = "";

        [JsonProperty("password")]
        public string Password { get; set; } = "";

    }
}
