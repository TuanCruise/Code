using System.Collections.Generic;
using Core.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace CatalogsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogsController : ControllerBase
    {
        // GET: api/Catalogs
        [HttpGet]
        public IEnumerable<string> Get()
        {
            PostgresqlHelper postgresqlHelper = new PostgresqlHelper("User ID=core;Password=core;Host=10.36.36.12;Port=5432;Database=core;");
            //postgresqlHelper.ExecuteStore();
            return new string[] { "value1", "value2" };
        }

        // GET: api/Catalogs/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Catalogs
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Catalogs/5
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
