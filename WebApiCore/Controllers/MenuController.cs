using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess;
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
    public class MenuController : ControllerBase
    {
        //public class ModuleController : BaseController {
        private IConfiguration _configuration;
        public MenuController(IConfiguration configuration) //: base(configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllMenu")]
        public async Task<ActionResult> GetAllMenu(int userId)
        {
            var outPut = new RestOutput<List<MenuItemInfo>>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var menu = m_Client.GetAllMenu(userId);
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