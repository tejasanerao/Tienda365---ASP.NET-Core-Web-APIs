using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda365.API.Config
{
    public class AuthConfig
    {
        public int BlockAfterHowManyLoginAttempts { get; set; }
        public int AfterHowManySecondsToRetryLoginOTP { get; set; }
    }
}
