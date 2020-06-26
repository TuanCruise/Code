using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Host.BusinessFacade;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WB.SYSTEM;

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

        //[HttpPost]
        //public async Task<IActionResult> Post(CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        //1.CONVERT TO JSON
        //        string jsonString;
        //        using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
        //        {
        //            jsonString = await reader.ReadToEndAsync();
        //        }

        //        //2. CALL HOST
        //        var request = _requestClient.Create(new { Value = jsonString }, cancellationToken);
        //        var response = await request.GetResponse<Message>();

        //        //return Content($"{response.Message.Value}, MessageId: {response.MessageId:D}");
        //        //var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(data);

        //        //return Ok(response);
        //        return Ok(response.Message.Value);

        //    }
        //    catch (RequestTimeoutException exception)
        //    {
        //        return StatusCode((int)HttpStatusCode.RequestTimeout);
        //    }
        //}

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

                //2. CALL HOST: consumer
                //var request = _requestClient.Create(new { Value = jsonString }, cancellationToken);
                //var response = await request.GetResponse<Message>();
                //string val = response.Message.Value;
                //return Ok(val);

                //Fix to test
                var json = await Consume(jsonString);

                return Ok(json);

            }
            catch (RequestTimeoutException exception)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }

        public async Task<string> Consume(string json)
        {
            try
            {
                dynamic jsonObject = JsonConvert.DeserializeObject(json);
                //1. Get Json             
                JObject obj = JObject.Parse(json);
                JArray arrBody = (JArray)obj["Body"];

                var msg = new WB.MESSAGE.Message();
                msg.ObjectName = jsonObject.ObjectName;
                msg.MsgAction = jsonObject.MsgAction;

                //msg.Body = (ArrayList)jsonObject.Body;            
                msg.Body = SysUtils.String2Arrray(arrBody.ToString().Replace("[", "").Replace("]", "").Replace("\r\n", "").Replace("\"", ""), ",");

                //2.Process business
                if (jsonObject.MsgType == Constants.MSG_MNT_TYPE)
                {
                    MaintenanceFacade maintenanceFacade = new MaintenanceFacade();
                    maintenanceFacade.Process(ref msg);
                }
                else if (jsonObject.MsgType == Constants.MSG_MISC_TYPE)
                {
                    MiscellaneousFacade miscellaneousFacade = new MiscellaneousFacade();
                    miscellaneousFacade.Process(ref msg);
                }

                //3.Return
                var order = new {
                    Value = msg.Body
                };

                //json = JsonConvert.SerializeObject(order);
                json = JsonConvert.SerializeObject(msg.Body);

                return json;
            }
            catch (RequestTimeoutException ex)
            {
                throw ex;
            }
        }
    }
}