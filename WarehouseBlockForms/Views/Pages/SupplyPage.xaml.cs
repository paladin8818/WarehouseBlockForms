using System;
using System.Windows;
using System.Windows.Controls;
using WarehouseBlockForms.Classess;
using WarehouseBlockForms.Helpers;
using WarehouseBlockForms.Models;
using Xceed.Wpf.Toolkit;

namespace WarehouseBlockForms.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для SupplyPage.xaml
    /// </summary>
    public partial class SupplyPage : Page
    {

        SupplyDetailsHelperCollection sdh_collection = SupplyDetailsHelperCollection.instance();

        public SupplyPage()
        {
            InitializeComponent();

            DataContext = new
            {
                Collection = sdh_collection.Collection
            };

            btnAddRow.Click += delegate
            {
                SupplyDetailsHelper supplyDetails = new SupplyDetailsHelper();
                sdh_collection.add(supplyDetails);
            };

            btnSaveSupply.Click += delegate
            {
                saveSupply();
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

            sdh_collection.remove(supplyDetailHelper);
        }

        private void saveSupply ()
        {
            if(sdh_collection.Collection.Count == 0)
            {
                System.Windows.MessageBox.Show("Невозможно сохранить пустое поступление!");
                return;
            }
            if(System.Windows.MessageBox.Show("Подтвердите поступление", "Подтвердите поступление", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            if(!sdh_collection.isAllCorrect())
            {
                System.Windows.MessageBox.Show("Проверьте введенные данные!\nОдна или более строк заполнены некорректно!");
                return;
            }

            Supply supply = new Supply();
            supply.SupplyDate = DateTime.Now;

            if(supply.save())
            {
                for(int i = 0; i < sdh_collection.Collection.Count; i++)
                {
                    SupplyDetailsHelper sdh = sdh_collection.Collection[i];
                    SupplyDetails supplyDetails = new SupplyDetails();
                    supplyDetails.IdDetails = sdh.IdDetails;
                    supplyDetails.IdSupply = supply.Id;
                    supplyDetails.DetailsCount = sdh.DetailsCount;
                    if(!supplyDetails.save())
                    {
                        System.Windows.MessageBox.Show("При сохранении произошли ошибки.");
                        supply.remove();
                        return;
                    }
                }
                sdh_collection.clear();
            }
            else
            {
                System.Windows.MessageBox.Show("При сохранении произошли ошибки.");
                return;
            }

        }

        private void countValueChange(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DecimalUpDown updown = sender as DecimalUpDown;
            if (updown == null) return;

            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(updown);
            if (dgrow == null) return;

            SupplyDetailsHelper supplyDetailHelper = (SupplyDetailsHelper)dgrow.Item;
            if (supplyDetailHelper == null) return;

            supplyDetailHelper.DetailsCount = (int)updown.Value;
        }
    }
}
