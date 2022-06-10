using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tienda365.BL.Interface;
using Tienda365.BL.Models;
using Tienda365.DL;
using Tienda365.DL.Entities;
using Tienda365.DL.Repositories;

namespace Tienda365.BL.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;



        public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<UserBL> GetDetails(string email)
        {
            try
            {
                var userInfo = await _userManager.FindByEmailAsync(email);
                if(userInfo != null)
                {
                    return new UserBL
                    {
                        Email = email,
                        Address = userInfo.Address,
                        FirstName = userInfo.FirstName,
                        LastName = userInfo.LastName,
                        PhoneNumber = userInfo.PhoneNumber
                    };
                }
                return new UserBL { Email = null};
            }
            catch
            {
                return new UserBL { Email = null };
            }
        }

        public async Task<Tuple<bool, string>> LoginUser(string email, string password)
        {
            try
            {
                var userInfo = await _userManager.FindByEmailAsync(email);
                if (userInfo != null && await _userManager.CheckPasswordAsync(userInfo, password))
                {
                    return Tuple.Create(true, userInfo.Id);   
                }
                return Tuple.Create(false, "Username or Password is incorrect!");
            }
            catch
            {
                return Tuple.Create(false, "Some error occurred!");
            }
        }

        public async Task<Tuple<bool, List<string>>> RegisterUser(UserBL user)
        {
            try
            {
                var userInfo = await _userManager.FindByEmailAsync(user.Email);
                if (userInfo != null)
                {
                    return Tuple.Create(false, new List<string>{ "User already Exists. Please try to login using registered Email Address." });
                }
                var newUser = new ApplicationUser()
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    UserName = user.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                var result = await _userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    return Tuple.Create(true, new List<string>());
                }
                else
                {
                    var errors = new List<string>();
                    foreach(var error in result.Errors)
                    {
                        errors.Add($"{error.Code} : {error.Description}");
                    }
                    return Tuple.Create(false, errors);
                }
            }
            catch
            {
                return Tuple.Create(false, new List<string>());
            }
        }

        public async Task<bool> ForgotPassword(string email)
        {
            var userInfo = await _userManager.FindByEmailAsync(email);
            if(userInfo != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(userInfo);
                if (!string.IsNullOrEmpty(token))
                {
                    await SendForgotPasswordEmail(userInfo, token);
                }
            }
            

            return false;
        }

        private async Task SendForgotPasswordEmail(ApplicationUser user, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:ForgotPassword").Value;

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };

            await _emailService.SendEmailForForgotPassword(options);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordBL resetPasswordBL)
        {
            return await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(resetPasswordBL.UserId), resetPasswordBL.Token, resetPasswordBL.NewPassword);
        }
    }
}
