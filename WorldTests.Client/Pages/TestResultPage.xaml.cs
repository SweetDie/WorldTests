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

namespace WorldTests.Client.Pages
{
    /// <summary>
    /// Interaction logic for TestResultPage.xaml
    /// </summary>
    public partial class TestResultPage : Page
    {
        private readonly MainWindow _mainWindow;

        public TestResultPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void AnotherTestButtonClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.navigation.GoToTestsListPage(sender, e);
        }

        private void MainMenuButtonClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.navigation.GoToMainPage(sender, e);
        }
    }
}
