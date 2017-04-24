using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace DarkBlue
{
    public partial class MainWindow : Window
    {
        MessageBoxButton buttons = MessageBoxButton.OKCancel;
        bool alive = false;
        UdpClient client;
        int LOCALPORT;
        int REMOTEPORT;
        const int TTL = 20;
        string HOST;
        IPAddress groupAddress;
        string userName;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                HOST = ParanoikChat.Settings.Default.Ip;
                REMOTEPORT = Convert.ToInt32(ParanoikChat.Settings.Default.Port);
                LOCALPORT = Convert.ToInt32(ParanoikChat.Settings.Default.Port);
                groupAddress = IPAddress.Parse(HOST);
                checkBox.IsChecked = ParanoikChat.Settings.Default.SaveFile;
                autorunCheckBox.IsChecked = ParanoikChat.Settings.Default.Autorun;
            }catch(Exception ex)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(ex.Message, " ", buttons);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            userNameTextBox.Text = ParanoikChat.Settings.Default.Login;
            IpTextBox.Text = ParanoikChat.Settings.Default.Ip;
            PortTextBox.Text = ParanoikChat.Settings.Default.Port;
            RsaCheckBox.IsChecked = ParanoikChat.Settings.Default.Rsa;

        }
        
        private void SendMsgbuttom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string message = String.Format("{0}: {1}", userName, messageTextBox.Text);
                byte[] data = Encoding.Unicode.GetBytes(message);

                client.Send(data, data.Length, HOST, REMOTEPORT);
                messageTextBox.Clear();
            }
            catch (Exception ex)
            {
                chatListBox.Items.Add(ex.Message);
            }
        }

        private void EnterBottum_Click(object sender, RoutedEventArgs e)
        {
            userName = userNameTextBox.Text;
            userNameTextBox.IsReadOnly = true;

            try
            {
                client = new UdpClient(LOCALPORT);
                client.JoinMulticastGroup(groupAddress, TTL);

                Task receiveTask = new Task(ReceiveMessages);
                receiveTask.Start();

                string message = userName + " Вошел в чат \n" + "Порт: " + LOCALPORT + " IP Адресс: " + HOST;
                byte[] data = Encoding.Unicode.GetBytes(message);
                client.Send(data, data.Length, HOST, REMOTEPORT);

                loginButton.IsEnabled = false;
                logoutButton.IsEnabled = true;
                sendButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                chatListBox.Items.Add(ex.Message);
            }
        }

        private void ReceiveMessages()
        {
            alive = true;
            try
            {
                while (alive)
                {
                    IPEndPoint remoteIp = null;
                    byte[] data = client.Receive(ref remoteIp);
                    string message = Encoding.Unicode.GetString(data);

                    Dispatcher.Invoke(new MethodInvoker(() =>
                    {
                        string time = DateTime.Now.ToShortTimeString();
                        chatListBox.Items.Add(time + " " + message + "\r\n");
                    }));
                }
            }
            catch (ObjectDisposedException)
            {
                if (!alive)
                    return;
                throw;
            }
            catch (Exception ex)
            {
                chatListBox.Items.Add(ex.Message);
            }
        }

        private void ExitBottum_Click_1(object sender, RoutedEventArgs e)
        {
            if (alive == true)
            {
                ExitChat();
            }
        }

        // выход из чата
        private void ExitChat()
        {
            try
            {
                string message = userName + " покидает чат";
                byte[] data = Encoding.Unicode.GetBytes(message);
                client.Send(data, data.Length, HOST, REMOTEPORT);
                client.DropMulticastGroup(groupAddress);

                alive = false;
                client.Close();
                userNameTextBox.IsReadOnly = false;
                loginButton.IsEnabled = true;
                logoutButton.IsEnabled = false;
                sendButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(ex.Message, " ", buttons);

            }

        }

        autorun auto = new autorun();
        private void autorunCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                auto.SetAutoRun();
            }
            catch (Exception ex)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(ex.Message, " ", buttons);
            }

        }

        private void autorunCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                auto.DeleteAutorun();
            }
            catch (Exception ex)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(ex.Message, " ", buttons);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParanoikChat.Settings.Default.SaveFile = (Boolean)checkBox.IsChecked;
                ParanoikChat.Settings.Default.Login = userNameTextBox.Text;
                ParanoikChat.Settings.Default.Ip = IpTextBox.Text;
                ParanoikChat.Settings.Default.Port = PortTextBox.Text;
                ParanoikChat.Settings.Default.Rsa = (bool)(RsaCheckBox.IsChecked);
                ParanoikChat.Settings.Default.Autorun = (Boolean)autorunCheckBox.IsChecked;

                ParanoikChat.Settings.Default.Save();
                System.Windows.Forms.Application.Restart();
                if (alive == true)
                {
                    ExitChat();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(ex.Message, " ", buttons);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string[] spp = new string[chatListBox.Items.Count];
            if (checkBox.IsChecked == true)
            {
                SaveDialog(spp);
            }
            if (alive == true)
            {
                ExitChat();
            }
        }
        
        private void SaveDialog(string[] spp)
        {
            try
            {
                DirectoryInfo data = new DirectoryInfo("Client_info");
                data.Create();
                string Path = @"Client_info/data_info.txt";
                for (int i = 0; i < spp.Length; i++)
                {
                    spp[i] = chatListBox.Items[i].ToString();
                    File.AppendAllText(Path, spp[i], Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}


