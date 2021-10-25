using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Threading;

namespace Commander
{
    public static class MQExtensions
    {
        //public static void SendT<T>(this PublisherSocket socket, T src)
        //{
        //    var json = JsonConvert.SerializeObject(src);
        //    socket.SendFrame(json);
        //}

        //public static T ReceiveT<T>(this PublisherSocket socket)
        //{
        //    var json = socket.ReceiveFrameString();
        //    T obj = JsonConvert.DeserializeObject<T>(json);
        //    return obj;
        //}

        public static void SendT<T>(this SubscriberSocket socket, T src)
        {
            var json = JsonConvert.SerializeObject(src);
            socket.SendFrame(json);
        }

        public static T ReceiveT<T>(this SubscriberSocket socket)
        {
            var json = socket.ReceiveFrameString();
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

    }

    internal class PushServ
    {
        public PushServ() { }

        internal void DoTest()
        {

            using (var subSocket = new SubscriberSocket())
            using (var poller = new NetMQPoller { subSocket })
            {
                subSocket.Options.ReceiveHighWatermark = 1000;
                subSocket.Connect("tcp://sz.epm.pub:8081");
                subSocket.Subscribe("");
                Console.WriteLine("connect server");
                poller.RunAsync();

                subSocket.ReceiveReady += (_, a) =>
                {
                    string message = a.Socket.ReceiveFrameString();

                    var commandx = JsonConvert.DeserializeObject<CommandX>(message);

                    Console.WriteLine($"{commandx.Id},{commandx.Name},{commandx.Url}");

                    if (commandx.Id == 1)
                    {
                        Log.Information($"Recv Name:{commandx.Name},Url: {commandx.Url}");

                        FluentScheduler.JobManager.AddJob(
                                                        () =>
                                                        {
                                                            Console.WriteLine($"{commandx.Name} {commandx.Url}");
                                                            //TODO:Log Setup;
                                                            Log.Information($"Recv Name:{commandx.Name},Url: {commandx.Url}");
                                                            var parser = new PullServ(commandx.Url);
                                                            parser.StarJob();
                                                        }, (s) => s.ToRunNow());
                    }
                };
                while (true)
                {
                    Console.WriteLine("Ready for Execute Command.");
                    Thread.Sleep(2 * 1000);
                }

            }

        }
    }
}