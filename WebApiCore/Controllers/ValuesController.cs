using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiCore.Common.Filters;
using WebApiCoreNew.Common.Filters;

namespace WebApiCore.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    //[ClaimRequirement("abcd", "CanReadResource")]
    //[IdentityBasicAuthentication]
    public class ValuesController : ControllerBase {
        // GET api/values
        [HttpGet]
        [CustomAuthentication]

        public string Get()
        {
            return "value hieupx";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class ClaimRequirementAttribute : TypeFilterAttribute {
        public ClaimRequirementAttribute(string claimType, string claimValue) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }

    public class ClaimRequirementFilter : Microsoft.AspNetCore.Mvc.Filters.IAuthorizationFilter {
        readonly Claim _claim;

        public ClaimRequirementFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == _claim.Type && c.Value == _claim.Value);
            if (!hasClaim) {
                context.Result = new ForbidResult();
            }
        }
    }

}
