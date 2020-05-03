using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkServer
{
    public class BasicMessagingListener
    {
        public BasicMessagingListener()
        {
            try
            {
                this.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void Start()
        {
            //Begin listening for incoming connection requests. Start will queue incoming connections 
            //until you either call the Stop method or it has queued MaxConnections.
            var listener = new TcpListener(IPAddress.Any, 8888);
            listener.Start();

            while (true)
            {
                //AcceptTcpClient will pull a connection from the incoming connection request queue.
                //These are pending connection requests
                //Returns a tcpClient to handle the request.  
                //You use the tcpCLient to send and receive data
                var incomingClient = listener.AcceptTcpClient();
                this.Logger("Incoming Connection");

                //Now that we have the client, we communicate by sending and receiving data 
                //packets using streams.
                //The GetStream method returns a NetworkStream that you can use to send and receive data.
                var networkStream = incomingClient.GetStream();

                //When they send you data over a stream, the data blocks have to be stored somewhere temporarily
                //Here you set the size of the buffer. 
                //Setting it to ReceiveBufferSize ensures you can read in 1 run.
                //ReceiveBufferSize is the network buffer size
                //If your buffer size is 1kb and they send you 2kb, then you have to call read twice and incur 
                //the overhead of calling the read method twice
                byte[] readBuffer = new byte[incomingClient.ReceiveBufferSize];

                //If you were to set an arbitrary size for buffer, you have to make sure you get all the data
                //This also will keep listening to the stream until they close it
                //byte[] readBuffer256 = new byte[256];
                //int i;
                //while ((i = networkStream.Read(readBuffer256, 0, readBuffer256.Length)) != 0)
                //{
                //    var tempDecodeMessage = Encoding.UTF8.GetString(readBuffer256, 0, i);
                //    Console.WriteLine("Received: {0}", tempDecodeMessage);
                //}

                //You will get the data in 1 read call
                //0 is the location in buffer to begin storing the data to.
                //3rd arg is the number of bytes to read from the NetworkStream.
                //Read() return how many bytes were read
                var bytesRead = networkStream.Read(readBuffer, 0, incomingClient.ReceiveBufferSize);

                //Decode the bytes into a string using UTF8
                //The read buffer has space allocated to it and decoding the whole thing will give you a weird result
                //We only want to decode the bytes that were sent to us so we put 0 and bytesRead
                var decodedMessage = Encoding.UTF8.GetString(readBuffer, 0, bytesRead);
                this.Logger(decodedMessage);

                //To send data, encode your string to bytes using UTF8
                byte[] encodedMessage = Encoding.UTF8.GetBytes("Message from Server");
                networkStream.Write(encodedMessage, 0, encodedMessage.Length);
            }
        }

        private void StartSocket()
        {
            //Establish the local endpoint for the socket.
            //Dns.GetHostName returns the name of the host running the application.
            //When you do a look up of a server, todays modern hosts may have more than 1 IP
            //and name aliases.  Dns methods return an IPHostEntry as a container for those
            //results
            //IPEndpoint encapsulates an IP and port
            var hostname = Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8888);

            var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    Socket client = listener.Accept();
                    this.Logger("Incoming Connection");

                    byte[] readBuffer = new byte[client.ReceiveBufferSize];
                    var bytesRead = client.Receive(readBuffer);
                    var decodedMessage = Encoding.UTF8.GetString(readBuffer, 0, bytesRead);
                    this.Logger(decodedMessage);

                    byte[] encodedMessage = Encoding.UTF8.GetBytes("Message from Server");
                    client.Send(encodedMessage);
                }
            }
            catch (Exception ex)
            {
                this.Logger(ex);
            }
        }

        private void MessageReading()
        {
            Socket listener = new Socket(IPAddress.Loopback.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, 8888));
            listener.Listen(10);
            while (true)
            {
                var client = listener.Accept();
                var readBuffer = new byte[3];

                //var bytesRead = client.Receive(readBuffer);  //Receive reads data available from the stream up to the buffer size
                //var bytesRead = client.Receive(readBuffer, 0, 3, SocketFlags.None);  //Will read only 3 bytes from the stream and place at position 0 in readBuffer

                int bytesRead;
                int totalBytesRead = 0;

                //Here we will read from a stream in smaller increments
                //Lets say you have a chat with 1 million users.  If you have a read buffer thats large, imagine
                //that multiplied by 1 million

                //readBuffer has 3 spaces, insert into buffer at position 0, read 3 bytes from stream, 
                //repeat but place data into buffer starting at new position 3, then 6, etc...
                //Benefit is low mem usage since you're only reading 3 bytes, then processing 3 bytes, then you're done
                //Downside is overhead of calling .Receive()
                while ((bytesRead = client.Receive(readBuffer, totalBytesRead, readBuffer.Length, SocketFlags.None)) > 0)
                {
                    totalBytesRead += bytesRead;

                    //You can do something with readBuffer
                    var res = Encoding.UTF8.GetString(readBuffer, 0, bytesRead);
                    this.Logger(res);
                }
            }
        }

        private void ReadBlockOfMemory()
        {
            //Write 122 bytes to memoryStream then move position to 0
            Stream s = new MemoryStream();
            for (int i = 0; i < 122; i++)
            {
                s.WriteByte((byte)i);
            }
            s.Position = 0;

            // Create a buffer 10 bytes bigger than the memory stream for padding, 132
            byte[] bytes = new byte[s.Length + 10];
            int numBytesToRead = (int)s.Length;

            //We have numBytesToRead = 132 bytes to read and we are doing it 10 bytes at a time.
            int numBytesRead = 0;
            do
            {
                //read 10 bytes and place it into the buffer at the specified position "numBytesRead"
                int n = s.Read(bytes, numBytesRead, 10);
                numBytesRead += n;  //starts with 0, then to 10 so now place data in buffer s.Read(bytes, numBytesRead=10, 10);
                numBytesToRead -= n;    //132 -10 = 122
            } while (numBytesToRead > 0);
            s.Close();

            Logger(string.Format("number of bytes read: {0:d}", numBytesRead));
        }

        private void Logger(string msg) => Console.WriteLine(DateTime.Now.ToString() + ": Server: " + msg);
        private void Logger(Exception msg) => Console.WriteLine(DateTime.Now.ToString() + ": Server: " + msg.ToString());
    }
}
