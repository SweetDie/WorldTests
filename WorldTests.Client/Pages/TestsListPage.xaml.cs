using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorldTests.Client.Resources.Controls;
using WorldTests.Primitive;
using WorldTests.Primitive.Models;

namespace WorldTests.Client.Pages
{
    /// <summary>
    /// Interaction logic for TestsListPage.xaml
    /// </summary>
    public partial class TestsListPage : Page
    {
        private readonly MainWindow _mainWindow;
        private List<TestModel> _tests;

        public TestsListPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            //Task.Run(LoadTests);
            LoadTests();
        }

        private void SelectedTestClick(object sender, RoutedEventArgs e)
        {
            var testsId = (sender as TestView).Id;
            var selectedTest = _tests.FirstOrDefault(t => t.Id == testsId);
            _mainWindow.navigation.SetPlayTestPage(selectedTest);
        }

        private void LoadTests()
        {
            if (!_mainWindow.Client.Connected)
            {
                return;
            }

            var communication = new СommunicationServerClient(Command.GetTests, "GetTests");
            var sendData = JsonSerializer.Serialize(communication, typeof(СommunicationServerClient));
            var sendBytes = Encoding.UTF8.GetBytes(sendData);
            _mainWindow.Stream.Write(sendBytes);

            var buffer = new byte[_mainWindow.Buffersize];
            var serverAnswer = "";
            var countOfBytes = 0;
            do
            {
                try
                {
                    countOfBytes = _mainWindow.Stream.Read(buffer);
                    serverAnswer += Encoding.UTF8.GetString(buffer, 0, countOfBytes);
                }
                catch (Exception) { }
            } while (countOfBytes == 1024);
            communication = JsonSerializer.Deserialize<СommunicationServerClient>(serverAnswer);
            _tests = JsonSerializer.Deserialize<List<TestModel>>(communication.Data.ToString());

            Dispatcher.Invoke(() =>
            {
                wrapPanel.Children.Clear();
                foreach (var test in _tests)
                {

                    var testView = new TestView
                    {
                        TestName = test.Name,
                        QuestionsCount = test.Questions.Count.ToString(),
                        Id = test.Id
                    };
                    testView.Click += SelectedTestClick;
                    wrapPanel.Children.Add(testView);
                }
            });
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.navigation.GoToMainPage(sender, e);
        }

        private void RefreshButtonClick(object sender, RoutedEventArgs e)
        {
            LoadTests();
        }
    }
}
