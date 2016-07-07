using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WarehouseBlockForms.Helpers;

namespace WarehouseBlockForms.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для SheetPage.xaml
    /// </summary>
    public partial class SheetPage : Page
    {
        public SheetPage()
        {
            InitializeComponent();

            DataContext = new {
                SWCollection = SWCollection.instance().ViewSource.View
            };
            SWCollection.instance().ViewSource.SortDescriptions.Add(new SortDescription("OperationDate", ListSortDirection.Descending));
        }

        private void dgAvailability_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SWHelper swHelper = dgAvailability.SelectedItem as SWHelper;
            if (swHelper == null) return;
            dgDetailed.ItemsSource = swHelper.CurrentDetails;
        }
    }
}
