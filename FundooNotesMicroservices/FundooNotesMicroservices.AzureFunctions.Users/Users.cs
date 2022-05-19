using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FundooNotesMicroservices.Models;
using FundooNotesMicroservices.Shared.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FundooNotesMicroservices.AzureFunctions.Users
{
    public class Users
    {
        private readonly IUserInterface userServices;
        public Users(IUserInterface userServices)
        {
            this.userServices = userServices;
        }

        [FunctionName("UserRegistration")]
        [OpenApiOperation(operationId: "UserRegistration", tags: new[] { "Users" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserModel), Required = true, Description = "New user details.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public  async Task<IActionResult> UserRegistration([HttpTrigger(AuthorizationLevel.Anonymous, "post",
                Route = "FundooUsers")] HttpRequest req,
           ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject<UserModel>(requestBody);
                return new OkObjectResult(await userServices.RegisterUser(data));
            }
            catch (Exception e)
            {
                var error = $"GetDrivers failed: {e.Message}";
                
                if (error!=null)
                    return new StatusCodeResult(401);
                else
                    return new BadRequestObjectResult(error);
            }
        }

        [FunctionName("UserLogin")]
        [OpenApiOperation(operationId: "UserLogin", tags: new[] { "Users" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserLoginModel), Required = true, Description = "User Logged.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserLoginResponse), Description = "The OK response")]
        public async Task<IActionResult> UserLogin([HttpTrigger(AuthorizationLevel.Anonymous, "post",
                Route = "FundooUsers/{Login}")] HttpRequest req,
           ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject<UserLoginModel>(requestBody);
                dynamic result = userServices.Login(data);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                var error = $"GetDrivers failed: {e.Message}";

                if (error != null)
                    return new StatusCodeResult(401);
                else
                    return new BadRequestObjectResult(error);
            }
        }
    }
}

