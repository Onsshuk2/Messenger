using Data_access_layer.Entities;
using DataAccessLayer;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace ClientChat
{
    public partial class MainWindow : Window
    {
        // Server communication (UDP)
        IPEndPoint serverEndPoint;
        UdpClient client;

        // P2P communication (TCP)
        private TcpClient tcpClient;
        private NetworkStream tcpStream;

        private string nickname;
        ObservableCollection<MessageInfo> messages = new ObservableCollection<MessageInfo>();
        NicknameWindow nicknameWindow = new NicknameWindow();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = messages;

            client = new UdpClient();
            string address = ConfigurationManager.AppSettings["ServerAddress"];
            short port = short.Parse(ConfigurationManager.AppSettings["ServerPort"]);
            serverEndPoint = new IPEndPoint(IPAddress.Parse(address), port);

            LeaveCheck();
            nicknameWindow.ShowDialog();
            nickname = nicknameWindow.Nickname;
            NicknameTextBox.Text = nickname;
        }

        private void Leave_Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessageToServer("$<Leave>");
            CloseTcpConnection();
            LeaveCheck();
        }

        private async void Join_Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessageToServer("$<join>");
            await Task.Delay(500);  
            ListenToServer();
            JoinCheck();
            InitializeTcpConnection();
            // Next statements are for example, how to start working with database
            ClientChatContext context = new();
            Repository repository = new Repository(context);

            List<Client> friends = repository.GetAllFriends(3);
            foreach (var friend in friends)
            {
                MessageBox.Show(friend.NickName.ToString());
            }
            /////////////////////////////////////////////////////////////
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(msgTextBox.Text))
                return;

            string message = $"|{nickname}|: {msgTextBox.Text}";

         
            messages.Add(new MessageInfo(message, DateTime.Now));
            SendMessageToServer(message);
            SendMessageToPeer(message); 
            msgTextBox.Clear(); 
        }



        private async void SendMessageToServer(string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                await client.SendAsync(data, data.Length, serverEndPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending to server: {ex.Message}");
            }
        }

     
        private void SendMessageToPeer(string message)
        {
            try
            {
                if (tcpStream != null && tcpStream.CanWrite)
                {
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    tcpStream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending to peer: {ex.Message}");
            }
        }

       
        private async Task ListenToServer()
        {
            try
            {
                while (true)
                {
                    var result = await client.ReceiveAsync();
                    string message = Encoding.UTF8.GetString(result.Buffer);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        messages.Add(new MessageInfo(message, DateTime.Now));
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error receiving from server: {ex.Message}");
            }
        }

   
        private async void InitializeTcpConnection()
        {
            try
            {
                tcpClient = new TcpClient();
                await tcpClient.ConnectAsync("127.0.0.1", 4040); 
                tcpStream = tcpClient.GetStream();
                ReceiveMessagesFromPeer();
                MessageBox.Show("P2P connection established.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to peer: {ex.Message}");
            }
        }


        private async void ReceiveMessagesFromPeer()
        {
            var buffer = new byte[1024];
            try
            {
                while (tcpClient.Connected)
                {
                    int bytesRead = await tcpStream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; 
                    var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        messages.Add(new MessageInfo(message, DateTime.Now));
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error receiving from peer: {ex.Message}");
            }
        }

        private void CloseTcpConnection()
        {
            try
            {
                tcpStream?.Close();
                tcpClient?.Close();
                MessageBox.Show("Disconnected from peer.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error closing peer connection: {ex.Message}");
            }
        }

        private void JoinCheck()
        {
            Join_Button.IsEnabled = false;
            Leave_Button.IsEnabled = true;
            msgTextBox.IsEnabled = true;
            Send_Button.IsEnabled = true;
        }

        private void LeaveCheck()
        {
            Join_Button.IsEnabled = true;
            Leave_Button.IsEnabled = false;
            msgTextBox.IsEnabled = false;
            Send_Button.IsEnabled = false;
        }

        private void msgTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                Send_Button_Click(sender, e);
                e.Handled = true;
            }
        }

       
    }

}
