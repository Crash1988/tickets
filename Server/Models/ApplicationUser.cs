using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCoreServer.Models
{
    public class ApplicationUser : IdentityUser
    {
    string name;
    string lastName;
    }
}
