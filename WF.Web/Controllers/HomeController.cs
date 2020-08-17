using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WFToolsTestAPI.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using MassTransit;
using System.Collections;
using WB.MESSAGE;
using WB.SYSTEM;
using Microsoft.JSInterop;

namespace WFToolsTestAPI.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

       
        HttpClient _client;

        public async Task<IActionResult> Index()
        {
            try
            {
                //1.Init
                _client = new HttpClient { Timeout = TimeSpan.FromMinutes(1) };

                var tasks = new List<Task>();

                //2. GEN MESSAGE                
                Message msg = new Message();
                msg.MsgType = Constants.MSG_MISC_TYPE;//MESSAGE TYPE: INQ, MAINTAIN, TXN, REPORT
                msg.ObjectName = Constants.OBJ_SEARCH;                
                msg.MsgAction = Constants.MSG_SEARCH;
                msg.ModId = "03510";

                msg.Body.Add("SearchObject");
                msg.Body.Add("CUSTOMER"); ///Entity or Procedure name
                msg.Body.Add("Condition");
                msg.Body.Add(" WHERE 1=1"); //Condition
                msg.Body.Add("Page");
                msg.Body.Add(0); //Current page

                //3. CALL HOST API
                tasks.Add(Execute(msg));

                //4.WAIT ALL TASK TO COMPLETE
                await Task.WhenAll(tasks.ToArray());

                //5. Result                
                Reservation reservation = new Reservation();
                foreach (Task<string> task in tasks.Cast<Task<string>>())
                {
                    reservation.value = task.Result;
                    //Console.WriteLine(task.Result);  
                    ViewBag.Title = task.Result;
                }
                
                
                return View(reservation);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public readonly Random _random = new Random();

        public async Task<string> Execute(object msg)
        {
            try
            {                
                var json = JsonConvert.SerializeObject(msg);                
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                
                var responseMessage = await _client.PostAsync($"http://localhost:12345", data);

                responseMessage.EnsureSuccessStatusCode();

                var result = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {

                    string apiResponse = await responseMessage.Content.ReadAsStringAsync();
                    try
                    {
                        //receivedReservation = JsonConvert.DeserializeObject<Reservation>(apiResponse);
                        //json = JsonConvert.DeserializeObject<string>(apiResponse);                        
                        //JSRuntime.InvokeAsync<string>("bb_alert", json, DotNetObjectReference.Create(this), "AlertCallBack");
                        json = result;
                    }
                    catch (Exception ex)
                    {
                        
                    }                   
                }

                return json;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void ShowMessage(string message)
        {
            try
            {
                
               

            }
            catch (Exception exp)
            {
                ;
            }
        }

        public ViewResult AddReservation() => View();

        [HttpPost]
        public async Task<IActionResult> AddReservation(Reservation reservation)
        {
            Reservation receivedReservation = new Reservation();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Key", "Secret@123");
                StringContent content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("http://localhost:8888/api/Reservation", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    try
                    {
                        receivedReservation = JsonConvert.DeserializeObject<Reservation>(apiResponse);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Result = apiResponse;
                        return View();
                    }
                }
            }
            return View(receivedReservation);
        }

      
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class OrderModel
    {
        public Guid Id { get; set; }
        public string CustomerNumber { get; set; }
        public string PaymentCardNumber { get; set; }
        public string Notes { get; set; }
    }


    public class OrderStatusModel
    {
        public Guid OrderId { get; set; }
        public string State { get; set; }
    }
}
