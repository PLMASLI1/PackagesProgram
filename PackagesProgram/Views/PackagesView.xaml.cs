using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace PackagesProgram.Views
{

    public partial class PackagesView : Window
    {
        public PackagesView()
        {
            InitializeComponent();
        }

        private void NumberValidator(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
