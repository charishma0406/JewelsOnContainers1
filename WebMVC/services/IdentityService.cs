using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.services
{
    //this is basically our about page
    public class IdentityService : IIdentityService<ApplicationUser>
    {

        public ApplicationUser Get(IPrincipal principal)
        {
            //if the user is logged in 
            if (principal is ClaimsPrincipal claims)
            {
                //creating the new application user
                var user = new ApplicationUser()
                {
                    //with the email and id
                    Email = claims.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value ?? "",
                    //sub means which contains our Id
                    Id = claims.Claims.FirstOrDefault(x => x.Type == "sub")?.Value ?? "",
                };

                //returning the application user
                return new ApplicationUser
                {
                    Email = claims.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value ?? "",
                    Id = claims.Claims.FirstOrDefault(x => x.Type == "sub")?.Value ?? "",
                };

            }

            throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}
