﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FundooNotesMicroservices.Models
{
    public class UserLoginResponse
    {
        public UserModel? userModel { get; set; }
        //public string Email { get; set; } = "";
        public string token { get; set; } = "";
    }
}