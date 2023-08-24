using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Web.Models
{
    public enum ResponseCode
    {
        None = 0,
        Ok = 1,
        GeneralError = 2,
        NotFound = 3,
        MethodNotSupported = 4,
        Unauthorized = 5,
        Expired = 6,
        AlreadyExists = 7
}
}
