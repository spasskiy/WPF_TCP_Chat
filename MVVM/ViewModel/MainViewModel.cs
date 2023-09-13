using Chat19Aug.MVVM.Core;
using Chat19Aug.MVVM.Net;
using Chat19Aug.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;

namespace Chat19Aug.MVVM.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }

        bool IsConnected { get; set; } = false;

        public string Username { set; get; }
        public string IP { set; get; } = "127.0.0.1";
        public int Port { set; get; } = 31338;

        private string _message;
        public string Message 
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged(nameof(Message));
                }
            }
        }

        private Server server;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MainViewModel() 
        {
            
            Users = new ObservableCollection<UserModel>();
            Messages = new ObservableCollection<string>();
            server = new Server();
            server.connectedEvent += UserConnected;
            server.msgReceivedEvent += MessageReceived;
            server.userDisconectEvent += RemoveUser;
            ConnectToServerCommand = new RelayCommand(x => { server.ConnectToServer(Username, IP, Port); IsConnected = true; }, x => !string.IsNullOrEmpty(Username));
            SendMessageCommand = new RelayCommand(x => { server.SendMessageToServer(Message); Message = string.Empty; }, x => !string.IsNullOrEmpty(Message) && IsConnected);
        }

        private void MessageReceived()
        {
            var msg = server.packetReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => { Messages.Add(msg); });
        }
        private void UserConnected()
        {
            var user = new UserModel 
            { 
                Username = server.packetReader.ReadMessage(),
                UID = server.packetReader.ReadMessage()
            };
            if (!Users.Any(x => x.UID == user.UID))
            {                
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }
        private void RemoveUser()
        {
            var uid = server.packetReader?.ReadMessage();
            var user = Users.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher?.Invoke(() => Users.Remove(user));
        }

    }
}
