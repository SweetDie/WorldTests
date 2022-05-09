using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.VisualBasic.CompilerServices;
using WorldTests.Client.Pages;
using WorldTests.Primitive.Models;

namespace WorldTests.Client.Utilities
{
    public class Navigation
    {
        private readonly MainWindow _mainWindow;
        private MainPage _mainPage;
        private RegistrationPage _registrationPage;
        private LoginPage _loginPage;
        private PlayTestPage _playTestPage;
        private TestsListPage _testsListPage;
        private CreateTestPage _createTestPage;
        private TestResultPage _testResultPage;

        public Page CurrentPage { get; set; }

        public Navigation(MainWindow mainWindow)
        {
            mainWindow.mainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            _mainWindow = mainWindow;
        }

        public void GoToLoginPage(object sender, RoutedEventArgs e)
        {
            SetMainTitle("World tests");
            _loginPage ??= new LoginPage(_mainWindow);
            SetPage(_loginPage);
        }

        public void GoToRegistrationPage(object sender, RoutedEventArgs e)
        {
            SetMainTitle("World tests");
            _registrationPage ??= new RegistrationPage(_mainWindow);
            SetPage(_registrationPage);
        }

        public void GoToMainPage(object sender, RoutedEventArgs e)
        {
            SetMainTitle("Main menu");
            _mainPage ??= new MainPage(_mainWindow);
            SetPage(_mainPage);
        }

        public void SignInSignUpButton(object sender, RoutedEventArgs e)
        {
            if (_mainWindow.mainFrame.Content is LoginPage)
            {
                GoToRegistrationPage(sender, e);
            }
            else
            {
                GoToLoginPage(sender, e);
            }
        }

        public void GoToPlayTestPage(object sender, RoutedEventArgs e)
        {
            _playTestPage = new PlayTestPage(_mainWindow);
            SetPage(_playTestPage);
        }

        public void GoToTestsListPage(object sender, RoutedEventArgs e)
        {
            SetMainTitle("Tests list");
            _testsListPage ??= new TestsListPage(_mainWindow);
            SetPage(_testsListPage);
        }

        public void GoToCreateTestPage(object sender, RoutedEventArgs e)
        {
            if (_mainWindow.CurrentUser == null)
            {
                GoToLoginPage(sender, e);
            }
            else
            {
                SetMainTitle("Create test");
                _createTestPage ??= new CreateTestPage(_mainWindow);
                SetPage(_createTestPage);
            }
        }

        public void GoToTestResultPage(object sender, RoutedEventArgs e)
        {
            SetMainTitle("Test result");
            _testResultPage ??= new TestResultPage(_mainWindow);
            SetPage(_testResultPage);
        }

        public void SetPlayTestPage(TestModel testModel)
        {
            SetMainTitle(testModel.Name);
            GoToPlayTestPage(this, new RoutedEventArgs());
            _playTestPage.TestModel = testModel;
        }

        public void SetResultPage(string testName, int numberQuestions, int correctAnswers)
        {
            _testResultPage ??= new TestResultPage(_mainWindow);
            _testResultPage.testNameTextBlock.Text = $"Test: {testName}";
            _testResultPage.numberQuestionTextBlock.Text = $"Number of questions: {numberQuestions}";
            _testResultPage.correctAnswersTextBlock.Text = $"Correct answers: {correctAnswers}";
            GoToTestResultPage(this, new RoutedEventArgs());
        }

        private void SetPage(Page page)
        {
            _mainWindow.mainFrame.Content = page;
            CurrentPage = page;
        }

        private void SetMainTitle(string text)
        {
            _mainWindow.mainTitleTextBlock.Text = text;
        }
    }
}
