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
using System.Windows.Shapes;

namespace PackagesProgram.Views
{
    /// <summary>
    /// Logika interakcji dla klasy PackagesView.xaml
    /// </summary>
    public partial class PackagesView : Window
    {
        public PackagesView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            PackagesProgram.DatabaseDataSet databaseDataSet = ((PackagesProgram.DatabaseDataSet)(this.FindResource("databaseDataSet")));
            // Załaduj dane do tabeli PackagesIds. Możesz modyfikować ten kod w razie potrzeby.
            PackagesProgram.DatabaseDataSetTableAdapters.PackagesIdsTableAdapter databaseDataSetPackagesIdsTableAdapter = new PackagesProgram.DatabaseDataSetTableAdapters.PackagesIdsTableAdapter();
            databaseDataSetPackagesIdsTableAdapter.Fill(databaseDataSet.PackagesIds);
            System.Windows.Data.CollectionViewSource packagesIdsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("packagesIdsViewSource")));
            packagesIdsViewSource.View.MoveCurrentToFirst();
        }
    }
}
