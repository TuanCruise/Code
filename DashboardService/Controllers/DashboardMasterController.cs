using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace DashboardService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardMasterController : ControllerBase
    {
        // GET: api/DashboardMaster
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/DashboardMaster/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value" + id.ToString();
        }

        // POST: api/DashboardMaster
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/DashboardMaster/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
