using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WpfPlayground.Networking
{
    /*  Tcp abstracts the details for creating a Socket for requesting and receiving data.
     *  The connection is represented as a stream and so the data can be read and written with 
     *  .NET Framework stream-handling techniques.
     *  The TCP protocol needs IP and port to establish a connection with an endpoint and then 
     *  uses that connection to send and receive data packets.
     *  TCP ensures that the data packets are sent to the end point and assembled in the correct order
     *  when they arrive
     *  
     *  Streams have Backing stores (ex. NetworkStream), decorators (ex. BufferedStream), and adapters (ex. FileStream)
     *  They all inherit from the base stream class which exposes standard set of methods for reading, writing, and positioning.
     *  An array will deal with data in memory all at once where a stream can deal with data 1 byte or blocks of bytes at a time so 
     *  this means streams can use little memory
     *  
     *  Backing stores and decorators deal in bytes but applications work at a higher level like text or xml so we have adapters
     *  Backing stores deal with raw data
     *  Decortors provide binary transformations like encryption
     *  Adaptors deal with higher level types like text
     *  The stream class provides the fundamental operations: reading, writing, seeking, closing, flushing, and configuration timeouts
     *  
     *  stringwriter, streamwriter, textwriter, filestream
     *  Write to a file using a stream
     */

    public class BasicMessaging
    {
        public BasicMessaging()
        {
            try
            {
                this.BasicClient();                
            }
            catch (Exception e)
            {
                this.Logger(e.ToString());
            }
        }

        private void BasicClient()
        {
            var client = new TcpClient();
            client.Connect("localhost", 8888);

            //Get a client stream for reading and writing.
            NetworkStream stream = client.GetStream();

            Byte[] encodedMessage = Encoding.UTF8.GetBytes("Im from a client");

            //Send the message to the connected TcpServer. 
            stream.Write(encodedMessage, 0, encodedMessage.Length);

            // Buffer to store the response bytes.
            byte[] readBuffer = new byte[client.ReceiveBufferSize];

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(readBuffer, 0, readBuffer.Length);
            string responseData = Encoding.UTF8.GetString(readBuffer);
            this.Logger("Received: " + responseData);

            // Close everything.
            stream.Close();
            client.Close();
        }


        private void BasicClientSocket()
        {
            var hostname = Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8888);

            var client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(localEndPoint);
            byte[] encodedMessage = Encoding.UTF8.GetBytes("From ClientSocket");
            var bytesSent = client.Send(encodedMessage);
        }

        private void Logger(string msg) => Console.WriteLine(DateTime.Now.ToString() + ": Client: " + msg);
    }
}
