using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda365.API.DTO.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is requrired")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is requrired")]
        public string Password { get; set; }
    }
}
