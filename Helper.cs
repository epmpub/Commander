using System;
using System.Net;

namespace Commander
{
    internal class Helper
    {
        internal static void DownloadFile(string url, string Location, string filename)
        {
            //TODO::folder should as parameter by pass ,YAMF config specify dest path

            if (!System.IO.Directory.Exists(Location))
            {
                System.IO.Directory.CreateDirectory(Location);
            }
            var webClient = new WebClient();
            try
            {
                webClient.DownloadFile(url, Location + filename);
            }
            catch
            {


            }



        }

        internal static string DownloadString(string url)
        {
            var webClient = new WebClient();
            string result = null;
            try
            {
                result =  webClient.DownloadString(url);
            }
            catch
            {
            }
            return result;
        }
    }
}