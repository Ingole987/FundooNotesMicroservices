using FundooNotesMicroservices.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooNotesMicroservices.Shared.Interface
{
    public interface IJWTService
    {
        string GetJWT(string Id, string email);

        public JWTValidation ValidateJWT(HttpRequest request);

    }
}
