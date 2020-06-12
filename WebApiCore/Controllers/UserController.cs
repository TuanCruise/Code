using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApiCore.Common.Filters;
using WebCore.Entities;
using WebModelCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[IdentityBasicAuthentication]
    public class UserController : ControllerBase
    {
        //public class ModuleController : BaseController {
        private IConfiguration _configuration;
        public UserController(IConfiguration configuration) //: base(configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetUserLogin")]
        public async Task<ActionResult> GetUserLogin(string userName, string password)
        {
            var outPut = new RestOutput<User>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var users = m_Client.GetUserByUserNamePassword(userName, password);
                if (!users.Any())
                {
                    outPut.Data = null;
                    return Ok(outPut);
                }
                outPut.Data = users.First();
                return Ok(outPut);
            }
            catch (Exception ex)
            {
                outPut.ResultCode = -1;
                outPut.Message = ex.ToString();
                return Ok(outPut);
            }
        }


    }
}