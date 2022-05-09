using System.Windows;
using System.Windows.Controls;

namespace WorldTests.Client.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private readonly MainWindow _mainWindow;

        public MainPage(MainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
        }

        private void PlayTestsButtonClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.navigation.GoToTestsListPage(sender, e);
        }

        private void ButtonsDemoChip_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("CLick");
        }

        private void ButtonsDemoChip_OnDeleteClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete CLick");
        }

        private void QuitButtonClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.Close();
        }

        private void CreateTestButtonClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.navigation.GoToCreateTestPage(sender, e);
        }
    }
}
