using Chat19Aug.MVVM.Core;
using Chat19Aug.MVVM.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Chat19Aug.MVVM.ViewModel
{
    internal class MainViewModel
    {
        public RelayCommand ConnectToServerCommand { get; set; }
        public string Username { set; get; }
        private Server server;
        public MainViewModel() 
        { 
            server = new Server();
            ConnectToServerCommand = new RelayCommand(x => server.ConnectToServer(Username), x => !string.IsNullOrEmpty(Username));
        }
    }
}
