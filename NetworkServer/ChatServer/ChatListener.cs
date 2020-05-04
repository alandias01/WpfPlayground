using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkServer.ChatServer
{
    public class ChatListener
    {
        List<IChatUser> chatUsers = new List<IChatUser>();

        public async Task StartServer()
        {
            TcpListener listener = null;

            try
            {
                listener = new TcpListener(IPAddress.Any, 8888);
                listener.Start();
                Logger.Log("TcpListener started");

                while (true)
                {
                    var client = await listener.AcceptTcpClientAsync();                    
                    Logger.Log("Incoming Client");
                    var chatUser = new ChatUser(client);
                    chatUser.MessageReceived += ChatUser_MessageReceived;
                    this.chatUsers.Add(chatUser);
                }
            }
            catch (Exception e)
            {
                Logger.Log(e);
            }
            finally
            {
                listener.Stop();
            }
        }

        private async void ChatUser_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var currentClient = sender as IChatUser;            
            if (currentClient == null)
            {
                Logger.Log("Sender was not of type IChatUser");
            }

            var decodedMessage = Encoding.UTF8.GetString(e.ReadBuffer, 0, e.BytesRead);
            var messageComponents = decodedMessage.Split('|');

            var fromUser = messageComponents[0];
            var toUserRaw = messageComponents[1];            
            var messageBody = messageComponents[2];

            if (string.IsNullOrWhiteSpace(fromUser))
            {
                Logger.Log("Connection didn't have user information");
                return;
            }

            fromUser = fromUser.Trim().ToLower();

            //This is a connect message from the user
            //We will assign the nickname to the user and return
            int pos = fromUser.IndexOf("<connect>");
            if (pos != -1)
            {
                currentClient.NickName = fromUser.Remove(pos, "<connect>".Length);
                return;
            }

            if (string.IsNullOrWhiteSpace(toUserRaw))
            {
                Logger.Log("Connection didn't have user information");
                return;
            }

            Logger.Log(fromUser + ":" + toUserRaw);
            Logger.Log(messageBody);

            var newMessageBody = $"{fromUser}|{messageBody}";
            var encodedMessage = Encoding.UTF8.GetBytes(newMessageBody);

            //Who do we send this to
            var toUserArray = toUserRaw.Split(",", StringSplitOptions.RemoveEmptyEntries);

            var tasks = new List<Task>();

            foreach (var chatUser in this.chatUsers)
            {
                foreach( var toUser in toUserArray)
                {
                    if(chatUser.NickName?.ToLower() == toUser?.ToLower())
                    {
                        var stream = chatUser.Client.GetStream();

                        var t = stream.WriteAsync(encodedMessage);
                        tasks.Add(t.AsTask());                        
                    }
                }
            }

            await Task.WhenAll(tasks);
        }
    }

    public interface IChatUser
    {
        string NickName { get; set; }
        TcpClient Client { get; set; }
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
    }

    public class ChatUser : IChatUser, IEquatable<ChatUser>
    {
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public string NickName { get; set; }
        public TcpClient Client { get; set; }

        public ChatUser(TcpClient client)
        {
            try
            {
                this.Client = client;
                this.HandleClient(client);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            try
            {
                var stream = client.GetStream();

                var readBuffer = new byte[client.ReceiveBufferSize];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    var messageReceivedEventArgs = new MessageReceivedEventArgs { BytesRead = bytesRead, ReadBuffer = readBuffer };
                    this.OnMessageReceived(messageReceivedEventArgs);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        private void OnMessageReceived(MessageReceivedEventArgs messageReceivedEventArgs)
        {
            this.MessageReceived?.Invoke(this, messageReceivedEventArgs);
        }

        public bool Equals([AllowNull] ChatUser other) => this.NickName == other.NickName;
    }


    public class MessageReceivedEventArgs
    {
        public byte[] ReadBuffer { get; set; }
        public int BytesRead { get; set; }
    }

    public static class Logger
    {
        public static void Log(string msg) => Console.WriteLine(DateTime.Now.ToString() + ": " + msg);
        public static void Log(Exception e) => Console.WriteLine(DateTime.Now.ToString() + ": " + e.ToString());
    }
}
