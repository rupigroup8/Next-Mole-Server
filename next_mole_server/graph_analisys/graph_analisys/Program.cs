//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace graph_analisys
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//        }
//    }
//}
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Diagnostics;

//using IronPython.Hosting;

//namespace graph_analisys
//{
//    internal class Program
//    {
//        private static void Main()
//        {
//            Console.WriteLine("What would you like to print from python?");
//            var input = Console.ReadLine();

//            var py = Python.CreateEngine();
//            try
//            {
//                py.Execute("print('From Python: " + input + "')");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(
//                   "Oops! We couldn't print your message because of an exception: " + ex.Message);
//            }

//            Console.WriteLine("Press enter to exit...");
//            Console.ReadLine();
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Text;

using IronPython.Hosting;

namespace graph_analisys
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Press enter to execute the python script!");
            Console.ReadLine();

            var py = Python.CreateEngine();
            try
            {
                py.ExecuteFile("script.py");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                   "Oops! We couldn't execute the script because of an exception: " + ex.Message);
            }

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}