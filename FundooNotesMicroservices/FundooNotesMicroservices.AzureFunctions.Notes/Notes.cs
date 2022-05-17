using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FundooNotesMicroservices.Models;
using FundooNotesMicroservices.Shared.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace FundooNotesMicroservices.AzureFunctions.Notes
{
    public class Notes
    {
        private readonly INoteInterface _noteServices;
        private readonly IJWTService _JWTService;
        public Notes(INoteInterface noteServices , IJWTService JWTService)
        {
            this._noteServices = noteServices;
            this._JWTService = JWTService;
        }

        [FunctionName("CreateNotes")]
        [OpenApiOperation(operationId: "CreateNotes", tags: new[] { "Notes" })]
        [OpenApiSecurity("JWT Bearer Token", SecuritySchemeType.ApiKey, Name = "token", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(NotesModel), Required = true, Description = "New note details.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NotesModel), Description = "The OK response")]
        public  async Task<IActionResult> CreateNotes([HttpTrigger(AuthorizationLevel.Anonymous, "post",
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
                dynamic data = JsonConvert.DeserializeObject<NotesModel>(requestBody);
                return new OkObjectResult(await _noteServices.CreateNotes(data ,result.Id));
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

        [FunctionName("GetAllNotes")]
        [OpenApiOperation(operationId: "GetAllNotes", tags: new[] { "Notes" })]
        [OpenApiSecurity("JWT Bearer Token", SecuritySchemeType.ApiKey, Name = "token", In = OpenApiSecurityLocationType.Header)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetAllNotes(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "FundooNotes/{GetAll}")] HttpRequest req,
          ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var result = _JWTService.ValidateJWT(req);
            if (!result.IsValid)
            {
                return new UnauthorizedResult();
            }

            var response = _noteServices.GetAllNotes(result.Email);

            return new OkObjectResult(response);
        }

        [FunctionName("GetNoteById")]
        [OpenApiOperation(operationId: "GetNoteById", tags: new[] { "Notes" })]
        [OpenApiSecurity("JWT Bearer Token", SecuritySchemeType.ApiKey, Name = "token", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NotesModel), Description = "The OK response")]
        public IActionResult GetNoteById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "FundooNotes/GetAll/{Id}")] HttpRequest req, string id)
        {
            var result = _JWTService.ValidateJWT(req);
            if (!result.IsValid)
            {
                return new UnauthorizedResult();
            }

            var response = _noteServices.GetNoteById(result.Email, id);

            return new OkObjectResult(response);
        }
    }
}

