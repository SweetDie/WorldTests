using System;
using System.Configuration;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WorldTests.Client.Pages;
using WorldTests.Client.Utilities;
using WorldTests.Primitive.Models;

namespace WorldTests.Client
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        public long Buffersize { get; set; } = 1024;
        public TcpClient Client { get; set; }
        public NetworkStream Stream { get; set; }
        public readonly Navigation navigation;
        public UserModel CurrentUser { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ConnectToServer();

            navigation = new Navigation(this);
            ChangeUserBar();
            navigation.GoToMainPage(this, new RoutedEventArgs());
        }

        public void ChangeUserBar()
        {
            if (CurrentUser != null)
            {
                var chip = UserBar.GetUserChip(CurrentUser);
                chip.DeleteClick += LogOutClick;
                stackPanelUserBar.Children.Clear();
                stackPanelUserBar.Children.Add(chip);
            }
            else
            {
                var button = new Button
                {
                    Content = "Sign In/Sign Up"
                };
                button.Click += navigation.SignInSignUpButton;

                stackPanelUserBar.Children.Clear();
                stackPanelUserBar.Children.Add(button);
            }
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            CurrentUser = null;
            ChangeUserBar();
            if (navigation.CurrentPage is CreateTestPage)
            {
                navigation.GoToMainPage(sender, e);
            }
        }

        private void ConnectToServer()
        {
            var address = ConfigurationManager.AppSettings["ServerIPAddress"];
            var port = ConfigurationManager.AppSettings["ServerPort"];
            if (int.TryParse(port, out var portInt) == false)
            {
                portInt = 3100;
            }

            try
            {
                Client = new TcpClient(address, portInt);
                Stream = Client.GetStream();
            }
            catch (SocketException)
            {
                MessageBox.Show("Could not connect to the server, please try again later", "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        public void StatusLabelTimer(string text)
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(3);
            _timer.Tick += timerStatusLabelChange;
            mainStatusTextBlock.Text = text;
            _timer.Start();
        }

        private void timerStatusLabelChange(object? sender, EventArgs e)
        {
            mainStatusTextBlock.Text = string.Empty;
            _timer.Stop();
        }
    }
}
