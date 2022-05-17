using FundooNotesMicroservices.Shared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesMicroservices.Shared.Service
{
    public class SettingService : ISettingService
    {
        private readonly IJWTService _jWTService;

        public SettingService(IJWTService jWTService)
        {
            _jWTService = jWTService;
        }


        // Cosmos DB
        private const string Uri = "Uri";
        private const string PrimaryKey = "PrimaryKey";
        private const string CosmosConnectionString = "CosmosConnectionString";
        private const string DatabaseName = "DatabaseName";
        private const string ContainerName = "ContainerName";


        //*** PRIVATE ***//
        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
        public string GetContainerName()
        {
            return GetEnvironmentVariable(ContainerName);
        }

        public string GetCosmosConnectionString()
        {
            return GetEnvironmentVariable(CosmosConnectionString);
        }

        public string GetDatabaseName()
        {
            return GetEnvironmentVariable(DatabaseName);
        }

        public string GetPrimaryKey()
        {
            return GetEnvironmentVariable(PrimaryKey);
        }

        public string GetUri()
        {
            return GetEnvironmentVariable(Uri);
        }

    }
}





