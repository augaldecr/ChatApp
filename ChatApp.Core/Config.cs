using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core
{
    public static class Config
    {
        public static string MainEndpoint = "http://localhost:7155";
        public static string NegotiateEndpoint = $"{MainEndpoint}/api/negotiate"; 
    }
}
