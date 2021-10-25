using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Commander
{
    internal class PullServ
    {
        private Config conf = null;

        public PullServ(string url)
        {
            string configString = null;
            try
            {
                configString = Helper.DownloadString(url);
            }
            catch
            {
            }

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
                .Build();
            try
            {
                conf = deserializer.Deserialize<Config>(configString/*File.ReadAllText(filePath+fileName)*/);
            }
            catch (Exception e)
            {

                Console.WriteLine($"Des error with: {e.Message} ");
            }
        }

        internal void StarJob()
        {
            var executor = new Executor(conf);
            executor.Run(conf);
        }

        internal int GetInterval()
        {
            return conf.Interval;
        }
    }
}