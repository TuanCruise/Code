using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCoreBlazorServer.Data
{
    public partial class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirnPassword { get; set; }
    }
}
