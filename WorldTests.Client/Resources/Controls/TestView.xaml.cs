using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WorldTests.Client.Resources.Controls
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class TestView : UserControl
    {
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
            "Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TestView));

        private string _testName;
        private string _questionsCount;

        public Guid Id { get; set; }

        public string TestName
        {
            get => TestName;
            set
            {
                _testName = value;
                textName.Text = _testName;
            }
        }

        public string QuestionsCount
        {
            get => _questionsCount;
            set
            {
                _questionsCount = value;
                questionsCount.Text = _questionsCount;
            }
        }

        public TestView()
        {
            InitializeComponent();
        }
        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        void button_Click(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(ClickEvent));
    }
}
