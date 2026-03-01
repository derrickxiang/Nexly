using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Domain
{
    public class User : IdentityUser
    {
        public string? DisplayName { get; set; }
    }

}
