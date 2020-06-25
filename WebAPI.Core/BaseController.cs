using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Core.API
{
    [Route("/")]
    public class BaseController :
        Controller
    {
        private readonly IRequestClient<Message> _requestClient;

        public BaseController(IRequestClient<Message> requestClient)
        {
            _requestClient = requestClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {

                var request = _requestClient.Create(new {Value = "Hello, World."}, cancellationToken);

                var response = await request.GetResponse<Message>();

                return Content($"{response.Message.Value}, MessageId: {response.MessageId:D}");
            }
            catch (RequestTimeoutException exception)
            {
                return StatusCode((int) HttpStatusCode.RequestTimeout);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CancellationToken cancellationToken)
        {
            try
            {
                //1.CONVERT TO JSON
                string jsonString;
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    jsonString = await reader.ReadToEndAsync();
                }

                //2. CALL HOST
                var request = _requestClient.Create(new { Value = jsonString }, cancellationToken);
                var response = await request.GetResponse<Message>();

                //return Content($"{response.Message.Value}, MessageId: {response.MessageId:D}");
                //var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(data);

                //return Ok(response);
                return Ok(response.Message.Value);

            }
            catch (RequestTimeoutException exception)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }

    }
}