using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAppCoreBlazorServer.Service
{
    public class DITest : IDITest
    {
        // The constructor receives an HttpClient via dependency
        // injection. HttpClient is a default service.
        public DITest(HttpClient client)
        {
            
        }

        public async Task<string> Test()
        {
            return "abcd";

        }
    }
}
