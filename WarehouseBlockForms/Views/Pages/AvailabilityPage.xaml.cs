using System;
using System.Collections.Generic;
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
using WarehouseBlockForms.Controllers;

namespace WarehouseBlockForms.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AvailabilityPage.xaml
    /// </summary>
    public partial class AvailabilityPage : Page
    {
        public AvailabilityPage()
        {
            InitializeComponent();

            DataContext = new
            {
                Collection = DetailsController.instance().ViewSource.View
            };
            cbxOvenFilter.ItemsSource = OvenController.instance().Collection;

            cbxOvenFilter.SelectionChanged += delegate
            {
                int selectedValue = (cbxOvenFilter.SelectedIndex != -1) ? (int)cbxOvenFilter.SelectedValue : 0;
                DetailsController.instance().showFromOvenId(selectedValue);
            };
            btnClearOvenFilter.Click += delegate
            {
                cbxOvenFilter.SelectedIndex = -1;
            };
            tbxFastFilter.TextChanged += delegate
            {
                DetailsController.instance().showFromText(tbxFastFilter.Text);
            };
            btnClearFastFilter.Click += delegate
            {
                tbxFastFilter.Text = "";
            };
        }
    }
}
