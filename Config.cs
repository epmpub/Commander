using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commander
{
    public class Config
    {
        public string Name { get; set; }
        public int Interval { get; set; }
        public string Resource { get; set; }
        public string logger { get; set; }
        public string logger_name { get; set; }

        public int logger_clean_time { get; set; }

        public List<Dictionary<string, Command>> Tasks { get; set; }
        public List<Dictionary<string, Script>> Scripts { get; set; }
    }

    public class Command
    {
        public string reserve { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Cmd { get; set; }
    }



    public class Script
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }



}
