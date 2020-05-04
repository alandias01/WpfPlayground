using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfPlayground.Networking.ChatClient
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : Window, INotifyPropertyChanged
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private string logWindow;
        private string chatWindow;

        public string ServerIp { get; set; }
        public string ServerPort { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string ChatMessage { get; set; }

        public string LogWindow
        {
            get => logWindow;
            set
            {
                logWindow = value;
                this.OnPropertyChanged();
                this.txtbxLogWindow.ScrollToEnd();
            }
        }

        public string ChatWindow
        {
            get => chatWindow;
            set
            {
                chatWindow = value;
                this.OnPropertyChanged();
                this.txtbxChatWindow.ScrollToEnd();
            }
        }

        public ChatView()
        {
            try
            {
                this.InitializeComponent();
                this.DataContext = this;
                this.ServerIp = "127.0.0.1";
                this.ServerPort = "8888";
                this.FromUser = "a";
                this.ToUser = "b";
                this.ChatMessage = "c";
                this.Logger("Initialized");
            }
            catch (Exception e)
            {
                this.Logger("Error initializing...");
                this.Logger(e);
            }
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this._client = new TcpClient();

                int port = int.Parse(this.ServerPort);
                this.Logger("Connecting to Server: " + this.ServerIp + ", Port: " + port.ToString());
                await _client.ConnectAsync(this.ServerIp, port);
                this.Logger("Connected to Server: " + this.ServerIp + ", Port: " + port.ToString());

                await Task.Delay(1000);
                this._stream = _client.GetStream();

                //Lets send an initial message to let them know who we are
                var connectMessage = this.FromUser + "<connect>";
                this.SendMessage(connectMessage, null, null);

                var readBuffer = new byte[_client.ReceiveBufferSize];
                int bytesRead;

                while ((bytesRead = await this._stream.ReadAsync(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    var decodedMessage = Encoding.UTF8.GetString(readBuffer, 0, bytesRead);

                    var messageComponents = decodedMessage.Split('|', 2);
                    
                    //Put in check to see if array has 2 elements

                    var fromUser = messageComponents[0];                    
                    var messageBody = messageComponents[1];

                    var formattedMessage = $"{fromUser}: {messageBody}{Environment.NewLine}";                    
                    this.ChatWindow += formattedMessage;
                }
            }
            catch (Exception ex)
            {
                this.Logger("Error connecting...");
                this.Logger(ex.ToString());
            }
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this._stream != null)
                {
                    this._stream.Close();
                }
                
                this._client.Close();
                this.Logger("Disconnected");
            }
            catch (Exception ex)
            {
                this.Logger("Error disconnecting...");
                this.Logger(ex);
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.SendMessage(this.FromUser, this.ToUser, this.ChatMessage);
            }
            catch (Exception ex)
            {
                this.Logger("Error sending...");
                this.Logger(ex);
            }
        }

        private async void SendMessage(string from, string to, string msg)
        {
            if (_stream.CanWrite)
            {
                to ??= "";
                msg ??= "";
                string finalMsg = $"{from}|{to}|{msg}";
                var encodedMessage = Encoding.UTF8.GetBytes(finalMsg);
                await this._stream.WriteAsync(encodedMessage, 0, encodedMessage.Length);
                this.Logger("Sent: " + finalMsg);
                this.Logger(string.Join(" ", encodedMessage));
            }
        }

        private void Logger(string msg) => this.LogWindow += (DateTime.Now.ToString() + ": " + msg + Environment.NewLine);
        private void Logger(Exception e) => this.LogWindow += (DateTime.Now.ToString() + ": " + e.ToString() + Environment.NewLine);

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
