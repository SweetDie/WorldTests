using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorldTests.Client.Validations;
using WorldTests.Primitive;
using WorldTests.Primitive.Models;

namespace WorldTests.Client.Pages
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private readonly MainWindow _mainWindow;

        public RegistrationPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void ConfirmPasswordBoxChanged(object sender, RoutedEventArgs e)
        {
            if (ConfirmPasswordBox.Password != PasswordBox.Password)
            {
                RegistrationErrorLabel.Content = "Passwords do not match";
                RegistrationErrorLabel.Visibility = Visibility.Visible;
            }
            else
            {
                RegistrationErrorLabel.Visibility = Visibility.Hidden;
            }
        }

        private void SignUpButtonClick(object sender, RoutedEventArgs e)
        {
            var userModel = ValidateRegistration();
            if (userModel != null)
            {
                Task.Run(() => SendRequestToServer(userModel));
            }
        }

        private void LoginPageClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.navigation.GoToLoginPage(sender, e);
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.navigation.GoToMainPage(sender, e);
        }

        private void SendRequestToServer(UserModel userModel)
        {
            var communication = new СommunicationServerClient(Command.Register, userModel);
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
                    case Command.Register:
                        AnswerFromServerRegister(communication); break;
                    case Command.RegisterError:
                        AnswerFromServerRegisterError(communication); break;
                }
            }
            catch (Exception)
            {
            }
        }

        private void AnswerFromServerRegisterError(СommunicationServerClient communication)
        {
            var loginError = communication.Data;
            RegistrationErrorLabel.Content = loginError;
            RegistrationErrorLabel.Visibility = Visibility.Visible;
        }

        private void AnswerFromServerRegister(СommunicationServerClient communication)
        {
            UsernameBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;
            ConfirmPasswordBox.Password = string.Empty;
            EmailBox.Text = string.Empty;
            FirstNameBox.Text = string.Empty;
            LastNameBox.Text = string.Empty;
            RegistrationErrorLabel.Visibility = Visibility.Hidden;
            _mainWindow.navigation.GoToLoginPage(this, new RoutedEventArgs());
        }

        private UserModel ValidateRegistration()
        {
            if (PasswordBox.Password == ConfirmPasswordBox.Password)
            {
                var userModel = new UserModel
                {
                    Username = UsernameBox.Text,
                    Password = PasswordBox.Password,
                    Email = EmailBox.Text,
                    Firstname = FirstNameBox.Text,
                    Lastname = LastNameBox.Text,
                    Id = Guid.NewGuid()
                };
                var validation = new UserModelValidation();
                var validateResult = validation.Validate(userModel);

                if (validateResult.IsValid)
                {
                    RegistrationErrorLabel.Visibility = Visibility.Hidden;
                    SendRequestToServer(userModel);
                    return userModel;
                }
                else
                {
                    RegistrationErrorLabel.Content = validateResult.Errors.FirstOrDefault();
                    RegistrationErrorLabel.Visibility = Visibility.Visible;
                }
            }
            else if (ConfirmPasswordBox.Password == string.Empty)
            {
                RegistrationErrorLabel.Content = "Confirm password cannot be empty";
                RegistrationErrorLabel.Visibility = Visibility.Visible;
            }
            else
            {
                RegistrationErrorLabel.Content = "Passwords do not match";
                RegistrationErrorLabel.Visibility = Visibility.Visible;
            }
            return null;
        }
    }
}
