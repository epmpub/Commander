using Serilog;
using Serilog.Events;
using System;
using System.Text;

namespace Commander
{
    internal class MyLogger
    {
        public MyLogger(Config c)
        {
            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Debug()
             .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
             .Enrich.FromLogContext()
             //.WriteTo.Console()
             .WriteTo.File(c.logger + c.logger_name, encoding: Encoding.UTF8)
             .CreateLogger();
            Console.WriteLine($"{c.logger} - {c.logger_name} - {c.logger_clean_time}");
        }

        internal void Setup(Config c)
        {
            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Debug()
             .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
             .Enrich.FromLogContext()
             //.WriteTo.Console()
             .WriteTo.File(c.logger + c.logger_name, encoding: Encoding.UTF8)
             .CreateLogger();

            Console.WriteLine($"{c.logger} - {c.logger_name} - {c.logger_clean_time}");
        }
    }
}