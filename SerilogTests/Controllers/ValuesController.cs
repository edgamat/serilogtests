using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SerilogTests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [LoggingActionFilter("expiringQuoteId")]
        public ActionResult<IEnumerable<string>> Get(string expiringQuoteId)
        {
            if (expiringQuoteId == "123")
            {
                return BadRequest("BOOGA");
            }

            if (expiringQuoteId == "1234")
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "MY ERROR MESSAGE");
            }

            if (expiringQuoteId == "12345")
            {
                return StatusCode(StatusCodes.Status204NoContent, "NO CONTENT");
            }

            return new string[] { "value1", "value2" };
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
}
