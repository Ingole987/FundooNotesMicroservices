using FundooNotesMicroservices.Models;
using FundooNotesMicroservices.Shared.Interface;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesMicroservices.Shared.Service
{
    public class UserServices : IUserInterface
    {
        private string _uri;
        private string _primaryKey;
        private string _databaseName;
        private string _containerName;

        private static CosmosClient? _cosmosClient;
        private readonly ISettingService _settingService;
        private readonly Container _cosmosContainer;
        private readonly IJWTService _jWTService;

        public UserServices(ISettingService setting, IJWTService jWTService)
        {
            _settingService = setting;

            _uri = _settingService.GetUri();
            _primaryKey = _settingService.GetPrimaryKey();
            _databaseName = _settingService.GetDatabaseName();
            _containerName = _settingService.GetContainerName();

            _jWTService = jWTService;

            _cosmosClient = new CosmosClient(_settingService.GetUri(), _settingService.GetPrimaryKey());
            _cosmosContainer = _cosmosClient.GetContainer(_databaseName, _containerName);
            //_cosmosContainer = new Lazy<Task<UserModel>>(async () =>
            //{
            //    var cosmos = new CosmosClient(setting.GetUri(), setting.GetPrimaryKey());
            //    var db = cosmos.GetDatabase(setting.GetDatabaseName());
            //    return await db.CreateContainerIfNotExistsAsync(setting.GetContainerName(), partitionKeyPath: "/email");

            //});
        }

        public async Task<UserModel> RegisterUser(UserModel userModel)
        {
            try
            {
                if (string.IsNullOrEmpty(_containerName))
                    throw new Exception("No Digital Main collection defined!");
                userModel.Id = Guid.NewGuid().ToString();
                using (var result = _cosmosContainer.CreateItemAsync<UserModel>(userModel, new PartitionKey(userModel.Email)))
                {
                    return result.Result.Resource;
                }
            }
            catch (Exception ex)
            {
                // Detect a `Resource Not Found` exception...do not treat it as error
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) && ex.InnerException.Message.IndexOf("Resource Not Found") != -1)
                {
                    return null;
                }
                else
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        public UserLoginResponse Login(UserLoginModel userLoginModel)
        {
            try
            {
                if (string.IsNullOrEmpty(_containerName))
                    throw new Exception("No Digital Main collection defined!");
                var LoginResult = _cosmosContainer.GetItemLinqQueryable<UserModel>(true)
                            .Where(t => t.Email == userLoginModel.Email && t.Password == userLoginModel.Password)
                            .AsEnumerable().FirstOrDefault();
                if (LoginResult != null)
                {


                    UserLoginResponse login = new UserLoginResponse();
                    
                    login.userModel = LoginResult;
                    //login.Email = LoginResult.Email;
                    login.token = _jWTService.GetJWT(login.userModel.Id, login.userModel.Email);
                    return login ;
                }
                return null;
            }
            catch (Exception ex)
            {
                // Detect a `Resource Not Found` exception...do not treat it as error
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) && ex.InnerException.Message.IndexOf("Resource Not Found") != -1)
                {
                    return null;
                }
                else
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        //private string JWTToken(string email, string Id)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JwtKey));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //    {new Claim(ClaimTypes.Email,email),
        //    new Claim("Id",Id.ToString())};
        //    var token = new JwtSecurityToken()
            
        //    claims,
        //    expires: DateTime.Now.AddMinutes(60),
        //    signingCredentials: credentials);
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}
