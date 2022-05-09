using System.Linq;
using System.Windows.Controls;
using WorldTests.Client.Pages;
using WorldTests.Primitive.Models;

namespace WorldTests.Client.Utilities
{
    public class PlayTestNavigation
    {
        public int CorrectAnswers { get; set; }
        private QuestionModel _currentQuestion;
        private PlayTestPage _page;
        private bool _lastQuestion;

        public PlayTestNavigation(PlayTestPage playTestPage)
        {
            CorrectAnswers = 0;
            _page = playTestPage;
        }

        public bool NextQuestion(QuestionModel question, bool lastQuestion)
        {
            if (RadioButtonsCheckValidate())
            {
                if (CorrectAnswerSelect())
                {
                    CorrectAnswers++;
                }
                if (_lastQuestion)
                {
                    return true;
                }
                else
                {
                    _page.answer1.IsChecked = false;
                    _page.answer2.IsChecked = false;
                    _page.answer3.IsChecked = false;
                    _page.answer4.IsChecked = false;
                    SetQuestion(question, lastQuestion);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetQuestion(QuestionModel question, bool lastQuestion)
        {
            _lastQuestion = lastQuestion;
            _currentQuestion = question;
            _page.questionTextBlock.Text = _currentQuestion.Name;
            SetAnswers(_currentQuestion.Answers.ToArray());
            if (lastQuestion)
            {
                _page.nextButton.Content = "Finish";
            }
            else
            {
                _page.nextButton.Content = "Next";
            }
        }

        private bool CorrectAnswerSelect()
        {
            foreach (var child in _page.mainGrid.Children)
            {
                if (child is RadioButton)
                {
                    if ((child as RadioButton).IsChecked == true)
                    {
                        if ((bool)(child as RadioButton).Tag)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool RadioButtonsCheckValidate()
        {
            foreach (var child in _page.mainGrid.Children)
            {
                if (child is RadioButton)
                {
                    if ((child as RadioButton).IsChecked == true)
                        return true;
                }
            }
            return false;
        }

        private void SetAnswers(AnswerModel[] answers)
        {
            _page.answer1.Content = "A. " + answers[0].Name;
            _page.answer1.Tag = answers[0].IsCorrect;
            _page.answer2.Content = "B. " + answers[1].Name;
            _page.answer2.Tag = answers[1].IsCorrect;
            _page.answer3.Content = "C. " + answers[2].Name;
            _page.answer3.Tag = answers[2].IsCorrect;
            _page.answer4.Content = "D. " + answers[3].Name;
            _page.answer4.Tag = answers[3].IsCorrect;
        }
    }
}
