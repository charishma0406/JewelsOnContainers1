using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace WebMVC.services
{
    public interface IIdentityService<T>
    {
        //to get the current login user
        T Get(IPrincipal principal);
    }
}
