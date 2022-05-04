using System;
using System.Collections.Generic;
using System.Text;

namespace Tienda365.BL.Models
{
    public class ResetPasswordBL
    {
        public string UserId { get; set; }

        public string Token { get; set; }

        public string NewPassword { get; set; }

        public bool IsSuccess { get; set; }
    }
}
