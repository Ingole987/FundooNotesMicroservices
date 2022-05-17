using FundooNotesMicroservices.Shared.Interface;
using FundooNotesMicroservices.Shared.Service;
using FundooNotesMicroservices.Shared.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(FundooNotesMicroservices.AzureFunctions.Notes.Startup))]
namespace FundooNotesMicroservices.AzureFunctions.Notes
{
    public class Startup : FunctionsStartup
    {
        private static readonly IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<INoteInterface, NoteService>();
            builder.Services.AddSingleton<ISettingService, SettingService>();
            builder.Services.AddSingleton<IJWTService, JWTService>();
        }
    }
}

