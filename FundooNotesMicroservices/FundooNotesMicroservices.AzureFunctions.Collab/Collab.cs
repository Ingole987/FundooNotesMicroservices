using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FundooNotesMicroservices.Models.Collab;
using FundooNotesMicroservices.Shared.Interface;
using FundooNotesMicroservices.Shared.Interface.NewFolder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace FundooNotesMicroservices.AzureFunctions.Collab
{
    public class Collab
    {
        private readonly ICollabInterface _collabServices;
        private readonly IJWTService _JWTService;
        public Collab(ICollabInterface collabServices, IJWTService JWTService)
        {
            this._collabServices = collabServices;
            this._JWTService = JWTService;
        }

        [FunctionName("Collab")]
        [OpenApiOperation(operationId: "Collab", tags: new[] { "Collab" })]
        [OpenApiSecurity("JWT Bearer Token", SecuritySchemeType.ApiKey, Name = "token", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CollabModel), Required = true, Description = "New note details.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CollabModel), Description = "The OK response")]
        public async Task<IActionResult> CreateNotes([HttpTrigger(AuthorizationLevel.Anonymous, "post",
                Route = "FundooNotes/{Create}")] HttpRequest req,
           ILogger log)
        {
            try
            {
                var result = _JWTService.ValidateJWT(req);
                if (!result.IsValid)
                {
                    return new UnauthorizedResult();
                }
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject<CollabModel>(requestBody);
                return new OkObjectResult(await _collabServices.AddCollab(data, result.Id));
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

