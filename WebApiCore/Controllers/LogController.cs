using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    public class LogController : ControllerBase
    {
        //public class ModuleController : BaseController {
        private IConfiguration _configuration;
        public LogController(IConfiguration configuration) //: base(configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("InsertLog")]
        public async Task<ActionResult> InsertLog([FromBody] LOG log)
        {
            var outPut = new RestOutput<int>();
            var m_Client = new CoreController(_configuration);
            SqlConnection conn = new SqlConnection(m_Client.ConnectionString);
            try
            {

                var cm = new WebApiCore.CommonFunction();
                var lst = new List<string>();
                var store = "sp_LOG_INS";
                var param = m_Client.DiscoveryParameters(store);
                var fieldInfos = new List<SqlParameter>();
                fieldInfos.Add(new SqlParameter("@ModId", log.ModId));
                fieldInfos.Add(new SqlParameter("@Type", log.Type));
                fieldInfos.Add(new SqlParameter("@ActionError", log.ActionError));
                fieldInfos.Add(new SqlParameter("@Note", log.Note));
                var userName = GetUserName();
                fieldInfos.Add(new SqlParameter("@UserName", userName));
                fieldInfos.Add(new SqlParameter("@Ip", log.Ip));
                conn.Open();
                var comm = new SqlCommand(store, conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(fieldInfos.ToArray());
                comm.ExecuteNonQuery();
                return Ok(outPut);
            }
            catch (Exception ex)
            {
                outPut.ResultCode = -1;
                outPut.Message = ex.ToString();
                return Ok(outPut);
            }
            finally
            {
                conn.Close();
            }
        }
        private string GetUserName()
        {
            var basic = HttpContext.Request.Headers["Authorization"];
            if (basic.Count > 0)
            {
                byte[] b = Convert.FromBase64String(basic.First().Replace("Basic", "").Trim());
                var strOriginal = System.Text.Encoding.UTF8.GetString(b);
                var split = strOriginal.Split(":");
                string userId = "";
                if (split.Length > 2)
                {
                    return split[2];
                }
            }
            return "";
        }

    }
}