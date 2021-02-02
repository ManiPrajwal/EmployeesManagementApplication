using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FirstWebApp.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim("CreateRole", "CreateRole"),
            new Claim("DeleteRole", "DeleteRole"),
            new Claim("EditRole", "EditRole")
        };
    }
}
