using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApiCoreNew.Common.Filters;
using WebCore.Entities;
using WebModelCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [CustomAuthentication]
    //[IdentityBasicAuthentication]
    public class SysVarController : ControllerBase
    {
        //public class ModuleController : BaseController {
        private IConfiguration _configuration;
        public SysVarController(IConfiguration configuration) //: base(configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllSysVar")]
        public async Task<ActionResult> GetAllSysVar()
        {
            var outPut = new RestOutput<List<SysVar>>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var sysVar = m_Client.GetAllSysVar();
                if (!sysVar.Any())
                {
                    outPut.Data = null;
                    return Ok(outPut);
                }
                outPut.Data = sysVar;
                return Ok(outPut);
            }
            catch (Exception ex)
            {
                outPut.ResultCode = -1;
                outPut.Message = ex.ToString();
                return Ok(outPut);
            }
        }

        [HttpGet]
        [Route("DeleteCacheStore")]

        public async Task<ActionResult> DeleteCacheStore()
        {
            try
            {
                PostgresqlHelper.m_CachedNpgParameters.Clear();
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }


    }
}