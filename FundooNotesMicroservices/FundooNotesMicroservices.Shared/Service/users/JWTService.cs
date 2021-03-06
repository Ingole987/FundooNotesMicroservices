using FundooNotesMicroservices.Models;
using FundooNotesMicroservices.Shared.Interface;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace FundooNotesMicroservices.Shared.Services
{
    public class JWTService : IJWTService
    {
        private readonly IJwtAlgorithm _algorithm;
        private readonly IJsonSerializer _serializer;
        private readonly IBase64UrlEncoder _base64Encoder;
        private readonly IJwtEncoder _jwtEncoder;
        public JWTService()
        {
            // JWT specific initialization.
            _algorithm = new HMACSHA256Algorithm();
            _serializer = new JsonNetSerializer();
            _base64Encoder = new JwtBase64UrlEncoder();
            _jwtEncoder = new JwtEncoder(_algorithm, _serializer, _base64Encoder);
        }

        public string GetJWT(string Id, string email)
        {
            Dictionary<string, object> claims = new Dictionary<string, object>
            {
                // JSON representation of the user Reference with ID and Email
                { "Id", Id},
                { "Email", email }
                
                // TODO: Add other claims here as necessary; maybe from a user database
            };

            string token = _jwtEncoder.Encode(claims, "ldbudnsvjdnvbsvhsiksncbcvsgwhkaamcbvbdguhrjfkfnmdnbsvwhkmdbbdhjdm dbbdjdkmdnbbjjkdmsnsbdbh"); // Put this key in config
            return token;
        }

        public JWTValidation ValidateJWT(HttpRequest request)
        {

            JWTValidation validationResponse = new JWTValidation();

            // Check if we have a header.
            if (!request.Headers.ContainsKey("token"))
            {
                validationResponse.IsValid = false;

                return validationResponse;
            }

            string authorizationHeader = request.Headers["token"];

            // Check if the value is empty.
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                validationResponse.IsValid = false;

                return validationResponse;
            }

            // Check if we can decode the header.
            IDictionary<string, object>? claims = null;

            try
            {
                if (authorizationHeader.StartsWith("Bearer"))
                {
                    authorizationHeader = authorizationHeader.Substring(7);
                }

                // Validate the token and decode the claims.
                claims = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret("ldbudnsvjdnvbsvhsiksncbcvsgwhkaamcbvbdguhrjfkfnmdnbsvwhkmdbbdhjdm dbbdjdkmdnbbjjkdmsnsbdbh")
                    .MustVerifySignature()
                    .Decode<IDictionary<string, object>>(authorizationHeader);
            }
            catch (Exception)
            {
                validationResponse.IsValid = false;

                return validationResponse;
            }

            // Check if we have user claim.
            if (!claims.ContainsKey("Id"))
            {
                validationResponse.IsValid = false;

                return validationResponse;
            }

            validationResponse.IsValid = true;
            validationResponse.Id = Convert.ToString(claims["Id"]);
            validationResponse.Email = Convert.ToString(claims["Email"]);

            return validationResponse;
        }

    }
}
