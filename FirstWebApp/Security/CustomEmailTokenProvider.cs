using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebApp.Security
{
    public class CustomEmailTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public CustomEmailTokenProvider(IDataProtectionProvider dataProtectionProvider, 
            IOptions<CustomEmailConfirmationTokenProviderOptions> options) : base(dataProtectionProvider, options)
        {

        }
    }
}
