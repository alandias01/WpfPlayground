using System;

namespace NetworkServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            new ChatServer.ChatListener().StartServer().Wait();
            //new BasicMessagingListener();
        }
    }
}
