using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tienda365.API.DTO;
using Tienda365.API.DTO.Requests;
using Tienda365.API.DTO.Responses;
using Tienda365.BL;
using Tienda365.BL.Interface;
using Tienda365.BL.Models;
using Tienda365.DL;
using Tienda365.DL.Entities;

namespace Tienda365.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _authenticationService;
        private readonly IConfiguration _configuration;


        public AuthenticationController(IAuthenticationService authenticationService, IConfiguration configuration)
        {
            this._authenticationService = authenticationService;
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("details")]
        [Authorize]
        public async Task<IActionResult> GetDetails()
        {
            try
            {
                var user = HttpContext.User;
                var claims = HttpContext.User.Claims.ToList();
                var emailClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                var email = emailClaim.Value;
                var details = await _authenticationService.GetDetails(email);
                if (details.Email != null)
                {
                    return Ok(details);
                }
                var res = new Response();
                res.Message.Add($"Try to login again!");
                return StatusCode(StatusCodes.Status401Unauthorized, res);

            }catch(Exception ex)
            {
                var res = new Response();
                res.Message.Add($"Some Error Occurred: {ex.ToString()}");
                return StatusCode(StatusCodes.Status500InternalServerError, res);
            }
            
        }

        /// <summary>
        /// New User Registration
        /// </summary>
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                var newUser = new UserBL
                {
                    Email = registerRequest.Email,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    Password = registerRequest.Password,
                    PhoneNumber = registerRequest.PhoneNumber,
                    Address = registerRequest.Address,
                };
                var result = await _authenticationService.RegisterUser(newUser);
                if (result.Item1 == true)
                {
                    var res = new Response();
                    res.Message.Add("User registered Successfully");
                    return Ok(res);
                }
                else
                {
                    var res = new Response();
                    res.Message = result.Item2;
                    return StatusCode(StatusCodes.Status417ExpectationFailed, res);
                }
            }
            catch (Exception ex)
            {
                var res = new Response();
                res.Message.Add("Some Error Occurred!");
                return StatusCode(StatusCodes.Status500InternalServerError, res);
            }

        }

        /// <summary>
        /// User Login
        /// </summary>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var result = await _authenticationService.LoginUser(loginRequest.Email, loginRequest.Password);
                if (result.Item1)
                {
                    var response = GenerateJwt(loginRequest.Email, result.Item2);
                    if (response.Token != null)
                    {
                        return StatusCode(StatusCodes.Status200OK, new Response<LoginResponse> { Data = response });
                    }
                }
                else
                {
                    var res = new Response();
                    res.Message.Add(result.Item2);
                    return StatusCode(StatusCodes.Status400BadRequest, res);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                var res = new Response();
                res.Message.Add("Some Error Occurred!");
                return StatusCode(StatusCodes.Status500InternalServerError, res);
            }

        }

        


        private LoginResponse GenerateJwt(string email, string id)
        {
            try
            {
                var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.NameIdentifier, id),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };
                var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var expiration = DateTime.Now.AddHours(2);
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: expiration,
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
                    );
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                var expiry = token.ValidTo;
                return new LoginResponse {
                    Token = jwtToken,
                    Expiration = expiry
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Token = null,
                };
            }
        }

        /// <summary>
        /// Forgot Password Request
        /// </summary>
        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            try
            {
                await _authenticationService.ForgotPassword(forgotPasswordRequest.Email);
                var res = new Response();
                res.Message.Add("An email must be sent to your email if you have already registered on our website");
                return StatusCode(StatusCodes.Status200OK, res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Reset Password Request
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Token = model.Token.Replace(' ', '+');
                    var resetPasswordBL = new ResetPasswordBL
                    {
                        UserId = model.UserId,
                        Token = model.Token,
                        NewPassword = model.NewPassword,
                        IsSuccess = model.IsSuccess
                    };
                    var result = await _authenticationService.ResetPasswordAsync(resetPasswordBL);
                    if (result.Succeeded)
                    {
                        ModelState.Clear();
                        model.IsSuccess = true;
                        var res = new Response();
                        res.Message.Add("Password Changed!");
                        return StatusCode(StatusCodes.Status200OK, res);
                        //return View(model);
                    }
                    else
                    {
                        var res = new Response();
                        foreach (var error in result.Errors)
                        {
                            res.Message.Add($"{error.Code}: {error.Description}");
                        }
                        return StatusCode(StatusCodes.Status400BadRequest, res);
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch(Exception ex)
            {
                var res = new Response();
                res.Message.Add(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, res);
            }

            
        }
    }
}
