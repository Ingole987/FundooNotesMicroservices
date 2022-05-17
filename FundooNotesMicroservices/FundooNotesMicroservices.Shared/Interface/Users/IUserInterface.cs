using FundooNotesMicroservices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesMicroservices.Shared.Interface
{
    public interface IUserInterface
    {
        
        Task<UserModel> RegisterUser(UserModel userModel);
        public UserLoginResponse Login(UserLoginModel userLoginModel);


    }
}
