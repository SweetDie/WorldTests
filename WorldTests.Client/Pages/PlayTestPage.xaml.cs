using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WorldTests.Client.Utilities;
using WorldTests.Primitive.Models;

namespace WorldTests.Client.Pages
{
    /// <summary>
    /// Interaction logic for PlayTestPage.xaml
    /// </summary>
    public partial class PlayTestPage : Page
    {
        private readonly MainWindow _mainWindow;
        private PlayTestNavigation _playTestNavigation;
        private int currentQuestionId = 0;
        private List<QuestionModel> _questions;
        public TestModel TestModel { get; set; }

        public PlayTestPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _playTestNavigation = new PlayTestNavigation(this);
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.navigation.GoToTestsListPage(sender, e);
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentQuestionId == _questions.Count)
            {
                if (_playTestNavigation.NextQuestion(null, true))
                {
                    ShowResult();
                }
                else
                {
                    _mainWindow.StatusLabelTimer("Сhoose the answer");
                }
            }
            else
            {
                if (currentQuestionId + 1 < _questions.Count)
                {
                    if (_playTestNavigation.NextQuestion(_questions[currentQuestionId], false))
                    {
                        currentQuestionId++;
                    }
                    else
                    {
                        _mainWindow.StatusLabelTimer("Сhoose the answer");
                    }
                }
                else
                {
                    if (_playTestNavigation.NextQuestion(_questions[currentQuestionId], true))
                    {
                        currentQuestionId++;
                    }
                    else
                    {
                        _mainWindow.StatusLabelTimer("Сhoose the answer");
                    }
                }
            }
        }

        private void ShowResult()
        {
            _mainWindow.navigation.SetResultPage(TestModel.Name, TestModel.Questions.Count, _playTestNavigation.CorrectAnswers);
        }

        private void LoadedPage(object sender, RoutedEventArgs e)
        {
            _questions = TestModel.Questions.ToList();

            if (currentQuestionId + 1 < _questions.Count)
            {
                _playTestNavigation.SetQuestion(_questions[currentQuestionId], false);
                currentQuestionId++;
            }
            else
            {
                _playTestNavigation.SetQuestion(_questions[currentQuestionId], true);
                currentQuestionId++;
            }
        }
    }
}
