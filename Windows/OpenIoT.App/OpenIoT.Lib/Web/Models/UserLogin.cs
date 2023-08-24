using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Web.Models
{
    public class UserLogin
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
