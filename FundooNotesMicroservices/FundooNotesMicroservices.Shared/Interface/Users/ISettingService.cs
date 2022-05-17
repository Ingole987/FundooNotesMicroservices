using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesMicroservices.Shared.Interface
{
    public interface ISettingService 
    {
        string GetUri();
        string GetPrimaryKey();
        string GetDatabaseName();
        string GetContainerName();
        string GetCosmosConnectionString();
    }
}
