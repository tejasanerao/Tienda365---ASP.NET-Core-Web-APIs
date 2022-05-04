using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tienda365.BL.Models;

namespace Tienda365.BL.Interface
{
    public interface IEmailService
    {
        Task SendEmailForForgotPassword(UserEmailOptions options);
    }
} 
