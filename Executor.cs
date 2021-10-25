using Serilog;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Text;

namespace Commander
{
    internal class Executor
    {
        public Executor(Config c)
        {
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Debug()
                 .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                 .Enrich.FromLogContext()
                 //.WriteTo.Console()
                 .WriteTo.File(c.logger + c.logger_name, encoding: Encoding.UTF8)
                 .CreateLogger();
        }

        public void RunPWSHCommand(string command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/c powershell  -NoLogo -NoProfile -executionpolicy unrestricted -Command " + command;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = false;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            Log.Information(process.StandardOutput.ReadToEnd());
            //Log.Error(process.StandardError.ReadToEnd());

        }
        public void RunBATCommand(string command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/c " + command;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = false;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            Log.Information(process.StandardOutput.ReadToEnd());

        }

        public void RunVBSCommand(string command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/c cscript " + command;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = false;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            Log.Information(process.StandardOutput.ReadToEnd());

        }


        internal void Run(Config p)
        {
            foreach (var item in p.Tasks)
            {
                Console.WriteLine($"{item["Command"].Id},{item["Command"].Name},{item["Command"].Type},{item["Command"].Cmd}");

                switch (item["Command"].Type)
                {
                    case "PWSH":
                        Console.WriteLine(item["Command"].reserve);
                        RunPWSHCommand(item["Command"].Cmd);
                        break;
                    case "BAT":
                        //Console.WriteLine("BAT");
                        RunBATCommand(item["Command"].Cmd);
                        break;
                    case "VBS":
                        //Console.WriteLine("VBS");
                        RunVBSCommand(item["Command"].Cmd);
                        break;
                    default:
                        //Console.WriteLine("Unkown");
                        break;
                }
            }


            foreach (var item in p.Scripts)
            {
                Console.WriteLine($"{item["Script"].Id}");

                switch (item["Script"].Type)
                {
                    case "PWSH":
                        Helper.DownloadFile(item["Script"].Url,p.Resource,"demo.ps1");
                        RunPWSHCommand(p.Resource+ "demo.ps1");
                        break;
                    case "VBS":
                        //Console.WriteLine("BAT");
                        Helper.DownloadFile(item["Script"].Url, p.Resource, "test.vbs");
                        RunVBSCommand(p.Resource + "test.vbs");
                        break;
                    case "BAT":
                        //Console.WriteLine("VBS");
                        Helper.DownloadFile(item["Script"].Url, p.Resource, "test.bat");
                        RunBATCommand(p.Resource + "test.bat");
                        break;
                    default:
                        //Console.WriteLine("Unkown");
                        break;
                }
            }
        }
    }
}