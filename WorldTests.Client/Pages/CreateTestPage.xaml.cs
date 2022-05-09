using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using WorldTests.Client.Utilities;
using WorldTests.Client.Validations;
using WorldTests.Primitive;
using WorldTests.Primitive.Models;

namespace WorldTests.Client.Pages
{
    /// <summary>
    /// Interaction logic for CreateTestPage.xaml
    /// </summary>
    public partial class CreateTestPage : Page
    {
        private readonly MainWindow _mainWindow;
        private QuestionModel _question;
        private List<AnswerModel> _answers;
        private CreateTestsManager _manager;

        public CreateTestPage(MainWindow mainWindow)
        {
            InitializeComponent();
            CreateClasses();
            _mainWindow = mainWindow;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            ClearFields();
            _manager = null;
            _mainWindow.navigation.GoToMainPage(sender, e);
        }

        private void ClearFields()
        {
            testNameTextBox.Text = string.Empty;
            questionTextBox.Text = string.Empty;
            answer1TextBox.Text = string.Empty;
            answer2TextBox.Text = string.Empty;
            answer3TextBox.Text = string.Empty;
            answer4TextBox.Text = string.Empty;
        }

        private void AddQuestionButtonClick(object sender, RoutedEventArgs e)
        {
            if (ValidateEmptyFields())
            {
                RadioButtonsCheck();
                _question.TestId = _manager.TestModel.Id;
                foreach (var answer in _answers)
                {
                    answer.QuestionId = _question.Id;
                    _question.Answers.Add(answer);
                }
                _manager.AddQuestion(_question);
                AnswerRadioButton1.IsChecked = true;
                CreateClasses();
                _mainWindow.StatusLabelTimer("Question added");
            }
        }

        private void RadioButtonsCheck()
        {
            if (AnswerRadioButton1.IsChecked == true)
            {
                _answers[0].IsCorrect = true;
            }
            else if (AnswerRadioButton2.IsChecked == true)
            {
                _answers[1].IsCorrect = true;
            }
            else if (AnswerRadioButton3.IsChecked == true)
            {
                _answers[2].IsCorrect = true;
            }
            else if (AnswerRadioButton4.IsChecked == true)
            {
                _answers[3].IsCorrect = true;
            }
        }

        private bool ValidateEmptyFields()
        {
            questionTextBox.Text = questionTextBox.Text;
            answer1TextBox.Text = answer1TextBox.Text;
            answer2TextBox.Text = answer2TextBox.Text;
            answer3TextBox.Text = answer3TextBox.Text;
            answer4TextBox.Text = answer4TextBox.Text;

            var questionValidation = new QuestionValidation();
            var answerValidation = new AnswerValidation();
            if (!questionValidation.Validate(_question).IsValid)
                return false;
            foreach (var answer in _answers)
            {
                if (!answerValidation.Validate(answer).IsValid)
                    return false;
            }
            return true;
        }

        private void CreateClasses()
        {
            _answers = new List<AnswerModel>
            {
                new AnswerModel{ Id = Guid.NewGuid(), IsCorrect = false },
                new AnswerModel{ Id = Guid.NewGuid(), IsCorrect = false },
                new AnswerModel{ Id = Guid.NewGuid(), IsCorrect = false },
                new AnswerModel{ Id = Guid.NewGuid(), IsCorrect = false }
            };
            _question = new QuestionModel { Id = Guid.NewGuid() };

            answer1TextBox.DataContext = _answers[0];
            answer2TextBox.DataContext = _answers[1];
            answer3TextBox.DataContext = _answers[2];
            answer4TextBox.DataContext = _answers[3];
            questionTextBox.DataContext = _question;

            _manager ??= new CreateTestsManager();

            testNameTextBox.DataContext = _manager.TestModel;
        }

        private void AnswerTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var answer = textBox.DataContext as AnswerModel;
            answer.Name = textBox.Text;
        }

        private void QuestionTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var answer = textBox.DataContext as QuestionModel;
            answer.Name = textBox.Text;
        }

        private void SaveTestButtonClick(object sender, RoutedEventArgs e)
        {
            testNameTextBox.Text = testNameTextBox.Text;
            if (!Validation.GetHasError(testNameTextBox))
            {
                var communication = new СommunicationServerClient(Command.SaveTest, _manager.TestModel);
                var sendData = JsonSerializer.Serialize(communication, typeof(СommunicationServerClient));
                var sendBytes = Encoding.UTF8.GetBytes(sendData);
                _mainWindow.Stream.Write(sendBytes);

                _manager = null;
                CreateClasses();
                _mainWindow.StatusLabelTimer("Test saved");
            }
        }
    }
}
