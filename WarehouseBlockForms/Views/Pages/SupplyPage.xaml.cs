using System.Windows.Controls;
using WarehouseBlockForms.Classess;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Helpers;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для SupplyPage.xaml
    /// </summary>
    public partial class SupplyPage : Page
    {
        public SupplyPage()
        {
            InitializeComponent();

            DataContext = new
            {
                OvenCollection = OvenController.instance().Collection
            };

            btnAddRow.Click += delegate
            {
                SupplyDetailsHelper supplyDetails = new SupplyDetailsHelper();
                supplyDetails.RowIndex = dgSupply.Items.Count + 1;
                dgSupply.Items.Add(supplyDetails);
            };
        }

        private void ovenSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;
            if (comboBox.SelectedIndex == -1) return;
            if (comboBox.SelectedValue == null) return;

            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(comboBox);
            if (dgrow == null) return;

            SupplyDetailsHelper supplyDetailHelper = (SupplyDetailsHelper)dgrow.Item;
            if (supplyDetailHelper == null) return;

            supplyDetailHelper.IdOven = (int)comboBox.SelectedValue;
            supplyDetailHelper.IdDetails = 0;
        }

        private void detailsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;
            if (comboBox.SelectedIndex == -1) return;
            if (comboBox.SelectedValue == null) return;

            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(comboBox);
            if (dgrow == null) return;

            SupplyDetailsHelper supplyDetailHelper = (SupplyDetailsHelper)dgrow.Item;
            if (supplyDetailHelper == null) return;

            supplyDetailHelper.IdDetails = (int)comboBox.SelectedValue;
        }

        private void removeRecord(object sender, System.Windows.RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(button);
            if (dgrow == null) return;

            SupplyDetailsHelper supplyDetailHelper = (SupplyDetailsHelper)dgrow.Item;
            if (supplyDetailHelper == null) return;

        }
    }
}
