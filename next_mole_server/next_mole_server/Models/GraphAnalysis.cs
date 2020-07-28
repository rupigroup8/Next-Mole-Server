using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace next_mole_server.Models
{
    public class GraphAnalysis
    {

        public static string PatchParameter(string parameter, int serviceid)
        {
            var engine = Python.CreateEngine(); // Extract Python language engine from their grasp
            var scope = engine.CreateScope(); // Introduce Python namespace (scope)
            var d = new Dictionary<string, object>
            {
                { "serviceid", serviceid},
                { "parameter", parameter}
            }; // Add some sample parameters. Notice that there is no need in specifically setting the object type, interpreter will do that part for us in the script properly with high probability

            scope.SetVariable("params", d); // This will be the name of the dictionary in python script, initialized with previously created .NET Dictionary
            ScriptSource source = engine.CreateScriptSourceFromFile(@"C:\Users\User\Desktop\Project\next_mole_server\next_mole_server\script.py"); // Load the script
            object result = source.Execute(scope);
            parameter = scope.GetVariable<string>("parameter"); // To get the finally set variable 'parameter' from the python script
            return parameter;
        }
        public static void doPython()
        {
            ScriptEngine engine = Python.CreateEngine();
            engine.ExecuteFile(@"C:\Users\User\Desktop\Project\next_mole_server\next_mole_server\script.py");
        }

        //public string PatchParameter(string parameter, int serviceid)
        //{
        //    var engine = Python.CreateEngine(); // Extract Python language engine from their grasp
        //    var scope = engine.CreateScope(); // Introduce Python namespace (scope)
        //    var d = new Dictionary<string, object>
        //    {
        //        { "serviceid", serviceid},
        //        { "parameter", parameter}
        //    }; // Add some sample parameters. Notice that there is no need in specifically setting the object type, interpreter will do that part for us in the script properly with high probability

        //    scope.SetVariable("params", d); // This will be the name of the dictionary in python script, initialized with previously created .NET Dictionary
        //    ScriptSource source = engine.CreateScriptSourceFromFile("C:/Users/User/Desktop/Project/Untitled.ipynb"); // Load the script
        //    object result = source.Execute(scope);
        //    parameter = scope.GetVariable<string>("parameter"); // To get the finally set variable 'parameter' from the python script
        //    return parameter;
        //}


        //private void run_cmd(string cmd, string args)
        //    {
        //        ProcessStartInfo start = new ProcessStartInfo();
        //        start.FileName = "my/full/path/to/python.exe";
        //        start.Arguments = string.Format("{0} {1}", cmd, args);
        //        start.UseShellExecute = false;
        //        start.RedirectStandardOutput = true;
        //        using (Process process = Process.Start(start))
        //        {
        //            using (StreamReader reader = process.StandardOutput)
        //            {
        //                string result = reader.ReadToEnd();
        //                Console.Write(result);
        //            }
        //        }
        //    }
    }
}