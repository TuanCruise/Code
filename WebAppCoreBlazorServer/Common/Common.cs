using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebAppCoreBlazorServer.Common {
    public class Common {
        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++) {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        public static void WriteElasticSearch()
        {
            // IConfiguration _Configuration=new Configuration();
            //var urlLog=Congg
            var loggerConfig = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
            {
                FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                EmitEventFailure = EmitEventFailureHandling.RaiseCallback,
                ModifyConnectionSettings = x => x.BasicAuthentication(username: "elastic", password: "changeme")
            });

            var logger = loggerConfig.CreateLogger();


            while (true)
            {
                Console.WriteLine("Logging..");
                logger.Information("This is Serilog by AnhTT47...");
                //Thread.Sleep(1000);
            }
        }


        public enum Role {
            Admin = 1,
            News = 2
        }
        public static DataTable GetDataTableFromJsonString(string json)
        {
            var jsonLinq = JObject.Parse(json);

            // Find the first array using Linq
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>()) {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties()) {
                    // Only include JValue types
                    if (column.Value is JValue) {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }
                trgArray.Add(cleanRow);
            }

            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }
        public static DataTable JsonStringToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string jSA in jsonStringArray) {
                string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData) {
                    try {
                        if (!string.IsNullOrEmpty(ColumnsNameData)) {
                            int idx = ColumnsNameData.IndexOf(":");
                            string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                            if (!ColumnsName.Contains(ColumnsNameString)) {
                                ColumnsName.Add(ColumnsNameString);
                            }
                        }
                    }
                    catch (Exception ex) {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName) {
                dt.Columns.Add(AddColumnName);
            }
            foreach (string jSA in jsonStringArray) {
                string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in RowData) {
                    try {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                        nr[RowColumns] = RowDataString;
                    }
                    catch (Exception ex) {
                        continue;
                    }
                }
                dt.Rows.Add(nr);
            }
            return dt;
        }

    }
    public class AppConfiguration {
        private readonly string _hostAddress = string.Empty;
        private readonly string _ipFirstLine = string.Empty;
        private readonly string _ipPrintHomo = string.Empty;
        private readonly string _numberCopyHomo = string.Empty;
        private readonly string _fromToPageHomo = string.Empty;
        private readonly string _printNameQc = string.Empty;
        public AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            _hostAddress = root.GetSection("ConfigApp").GetSection("WebAppUrl").Value;
            _ipFirstLine = root.GetSection("ConfigApp").GetSection("IpFirstLine").Value;
            _ipPrintHomo = root.GetSection("ConfigApp").GetSection("PrinterNameHomo").Value;
            _numberCopyHomo = root.GetSection("ConfigApp").GetSection("NumberCopyHomo").Value;
            _fromToPageHomo = root.GetSection("ConfigApp").GetSection("FromToPageHomo").Value;
            _printNameQc = root.GetSection("ConfigApp").GetSection("PrinterNameQC").Value;
            var appSetting = root.GetSection("ApplicationSettings");
        }
        public string HostAddress {
            get => _hostAddress;
        }
        public string IpFirstLine {
            get => _ipFirstLine;
        }
        public string IpPrintHomo {
            get => _ipPrintHomo;
        }
        public string NumberCopyHomo {
            get => _numberCopyHomo;
        }
        public string FromToPageHomo {
            get => _fromToPageHomo;
        }
        public string PrintNameQc {
            get => _printNameQc;
        }

    }
}
