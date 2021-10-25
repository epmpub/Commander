using System;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace Commander
{
    [RunInstaller(true)]
    public partial class Service : ServiceBase
    {
        //int ScheduleTime = Convert.ToInt32(ConfigurationSettings.AppSettings["ThreadTime"]);

        public Thread Worker = null;

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                ThreadStart start = new ThreadStart(Working);
                Worker = new Thread(start);
                Worker.Start();
            }
            catch 
            {

            }


        }

        public void Working()
        {
            while (true)
            {

                //Ver:0.1;
                //var util = new Util();
                //util.StartJob();
                
                string url = "https://it2u.oss-cn-shenzhen.aliyuncs.com/yaml/conf.yaml";
                var pullServ = new PullServ(url);

                Task.Run(()=> { pullServ.StarJob();});
                int interval = 0;
                try
                {
                    interval = pullServ.GetInterval();
                }
                catch
                {

                }
                EventLog.Source = "Commander";
                EventLog.WriteEntry(interval.ToString(), EventLogEntryType.Information);
                Thread.Sleep(interval * 60*1000);
                
            }
        }

        protected override void OnStop()
        {
            try
            {
                if ((Worker != null) && Worker.IsAlive)
                {
                    Worker.Abort();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
