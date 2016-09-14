using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrintMood.ResponseDTO
{
    public sealed class ServiceInfo
    {
        public string ApplicationName { get; set; }
        public string ApiVersion { get; set; }
        public object Links { get; set; }
    }
}
