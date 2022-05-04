using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda365.API.DTO
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Email is requrired")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is requrired")]
        [MinLength(6)]
        public string Password { get; set; }
        [Required(ErrorMessage = "First Name is requrired")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is requrired")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Phone Number is requrired")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Address is requrired")]
        public string Address { get; set; }
    }
}
