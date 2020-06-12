using System;
using System.Collections.Generic;
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
    public class LanguageController : ControllerBase
    {
        //public class ModuleController : BaseController {
        private IConfiguration _configuration;
        public LanguageController(IConfiguration configuration) //: base(configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllIcon")]
        public async Task<ActionResult> GetAllIcon()
        {
            var outPut = new RestOutput<List<LanguageInfo>>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var menu = m_Client.GetAllLanguageIcon();
                if (!menu.Any())
                {
                    outPut.Data = null;
                    return Ok(outPut);
                }
                outPut.Data = menu;
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