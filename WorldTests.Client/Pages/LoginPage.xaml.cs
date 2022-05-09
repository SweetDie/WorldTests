using System;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using WorldTests.Primitive;
using WorldTests.Primitive.Models;

namespace WorldTests.Client.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly MainWindow _mainWindow;

        public LoginPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void SignInButtonClick(object sender, RoutedEventArgs e)
        {
            SendRequestToServer();
        }

        private void RegistrationPageClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.navigation.GoToRegistrationPage(sender, e);
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.navigation.GoToMainPage(sender, e);
        }

        private void SendRequestToServer()
        {
            var credentials = new CredentialModel
            {
                Username = UsernameBox.Text.ToLower(),
                Password = PasswordBox.Password
            };

            var communication = new СommunicationServerClient(Command.Login, credentials);
            var sendData = JsonSerializer.Serialize(communication, typeof(СommunicationServerClient));
            var sendBytes = Encoding.UTF8.GetBytes(sendData);
            _mainWindow.Stream.Write(sendBytes);

            ReadAnswerFromServer();
        }

        private void ReadAnswerFromServer()
        {
            try
            {
                var buffer = new byte[_mainWindow.Buffersize];
                var countOfBytes = _mainWindow.Stream.Read(buffer);
                var serverAnswer = Encoding.UTF8.GetString(buffer, 0, countOfBytes);
                var communication = JsonSerializer.Deserialize<СommunicationServerClient>(serverAnswer);
                switch (communication.Command)
                {
                    case Command.Login:
                        AnswerFromServerLogin(communication); break;
                    case Command.LoginError:
                        AnswerFromServerLoginError(communication); break;
                }
            }
            catch (Exception)
            {
            }
        }

        private void AnswerFromServerLoginError(СommunicationServerClient communication)
        {
            var loginError = communication.Data;
            LoginErrorLabel.Content = loginError;
            LoginErrorLabel.Visibility = Visibility.Visible;
        }

        private void AnswerFromServerLogin(СommunicationServerClient communication)
        {
            var user = JsonSerializer.Deserialize<UserModel>(communication.Data.ToString());
            _mainWindow.CurrentUser = user;
            UsernameBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;
            _mainWindow.ChangeUserBar();
            _mainWindow.navigation.GoToMainPage(this, new RoutedEventArgs());
        }

        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            LoginErrorLabel.Visibility = Visibility.Hidden;
        }

        private void PasswordBoxTextChanged(object sender, RoutedEventArgs e)
        {
            LoginErrorLabel.Visibility = Visibility.Hidden;
        }
    }
}
