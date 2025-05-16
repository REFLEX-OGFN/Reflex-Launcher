using System.Windows;
using Volcano_Launcher.Pages;

namespace Volcano_Launcher
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigationVolcano.Content = new Home();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationVolcano.Content = new Home();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationVolcano.Content = new Library();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationVolcano.Content = new Donate();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationVolcano.Content = new Settings();
        }
    }
}
