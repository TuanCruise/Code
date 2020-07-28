using System.Collections;
using System.Threading.Tasks;
using Host.BusinessFacade;
using MassTransit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WB.MESSAGE;
using WB.SYSTEM;
using WebCore.Entities;

namespace Core.API
{
    public class BaseConsumer :IConsumer<Message>
    {
        private readonly IService _service;

        public BaseConsumer(IService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<Message> context)
        {
            //1. Get Json
            await _service.ServiceTheThing(context.Message.Value);
            dynamic jsonObject = JsonConvert.DeserializeObject(context.Message.Value);
            //var json2  = JsonConvert.DeserializeObject<string>(context.Message.Value);            

            JObject o = JObject.Parse(context.Message.Value);
            JArray arrBody = (JArray)o["Body"];

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
            var order = new
            {
                Value = msg.Body
            };
            var json = JsonConvert.SerializeObject(order);

            await context.RespondAsync<Message>(new
            {
                Value = $"Tra ve tu host"               
                //Value = json
            });

        }



    }


}