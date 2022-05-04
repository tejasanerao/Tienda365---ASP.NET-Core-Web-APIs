using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tienda365.BL.Models;

namespace Tienda365.BL.Interface
{
    public interface IAuthenticationService
    {
        public Task<Tuple<bool, List<string>>> RegisterUser(UserBL user);
        public Task<Tuple<bool, string>> LoginUser(string email, string password);
        public Task<bool> ForgotPassword(string email);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordBL resetPasswordBL);
    }
}
