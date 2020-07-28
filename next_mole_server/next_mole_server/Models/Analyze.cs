using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using IronPython.Hosting;


namespace next_mole_server.Models
{
    public class Analyze
    {
        static public void Main()
        {
            string response = File.ReadAllText(@"C:\Users\Gal Vakselblat\Desktop\Project\next_mole_server\next_mole_server\Scripts\test.json");
            //string x = JsonConvert.DeserializeObject<string>(response);
            dynamic json = JsonConvert.DeserializeObject(response);

            Console.WriteLine(json[10]);



        }
    }
}
