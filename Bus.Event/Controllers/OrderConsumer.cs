using System;
using System.Net.Http;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using WebCore.Entities.Entities;

namespace EBus.Event.Controllers
{
    public class OrderConsumer : IConsumer<Orders>
    {
        static HttpClient _client;
        ILogger<OrderConsumer> _logger;
        public OrderConsumer(ILogger<OrderConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<Orders> context)
        {
            var data = context.Message;
            await Execute();
            //var tasks = new List<Task>();
            //tasks.Add(Execute(order));

            //for (var i = 0; i < limit; i++)
            //{
            //    var order = new OrderModel
            //    {
            //        Id = NewId.NextGuid(),
            //        CustomerNumber = $"CUSTOMER{i}",
            //        PaymentCardNumber = i % 4 == 0 ? "5999" : "4000-1234",
            //        Notes = new string('*', 1000 * (i + 1))
            //    };

            //    tasks.Add(Execute(order));
            //}

            //await Task.WhenAll(tasks.ToArray());
        }
        /// <summary>
        /// Call API here
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        static async Task<string> Execute()
        {

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://dog.ceo/api/");
                    //HTTP GET
                    HttpResponseMessage response = await client.GetAsync("breeds/image/random");
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {

                    }
                }

                //var json = JsonConvert.SerializeObject(order);
                //var data = new StringContent(json, Encoding.UTF8, "application/json");

                //var responseMessage = await _client.PostAsync($"https://localhost:5001/Order", data);

                //responseMessage.EnsureSuccessStatusCode();

                //var result = await responseMessage.Content.ReadAsStringAsync();

                //if (responseMessage.StatusCode == HttpStatusCode.Accepted)
                //{
                //    await Task.Delay(2000);
                //    await Task.Delay(_random.Next(6000));

                //    var orderAddress = $"https://localhost:5001/Order?id={order.Id:D}";

                //    var patchResponse = await _client.PatchAsync(orderAddress, data);

                //    patchResponse.EnsureSuccessStatusCode();

                //    var patchResult = await patchResponse.Content.ReadAsStringAsync();

                //    do
                //    {
                //        await Task.Delay(5000);

                //        var getResponse = await _client.GetAsync(orderAddress);

                //        getResponse.EnsureSuccessStatusCode();

                //        var getResult = await getResponse.Content.ReadAsAsync<OrderStatusModel>();

                //        if (getResult.State == "Completed" || getResult.State == "Faulted")
                //            return $"ORDER: {order.Id:D} STATUS: {getResult.State}";

                //        Console.Write(".");
                //    }
                //    while (true);
                //}
                return null;

            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
    }
    public class Dog
    {
        public string message { get; set; }
        public string status { get; set; }

    }
}
