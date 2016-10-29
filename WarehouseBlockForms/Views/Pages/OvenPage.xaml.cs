using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WarehouseBlockForms.Classess;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для OvenPage.xaml
    /// </summary>
    public partial class OvenPage : Page
    {
        public OvenPage()
        {
            InitializeComponent();
            DataContext = new { Collection = OvenController.instance().ViewSource.View };

            OvenController.instance().ViewSource.SortDescriptions.Add(new SortDescription(OrderSettings.OvenSortColumn, OrderSettings.OvenSortDirection));

            btnAdd.Click += delegate
            {
                OvenSaveWindow ovenSaveWindow = new OvenSaveWindow();
                ovenSaveWindow.ShowDialog();
            };

            btnEdit.Click += delegate
            {
                Oven selectedOven = (Oven)dgOven.SelectedItem;
                if (selectedOven == null)
                {
                    MessageBox.Show("Выберите редактируемую запись!");
                    return;
                }
                OvenSaveWindow ovenSaveWindow = new OvenSaveWindow(selectedOven);
                ovenSaveWindow.ShowDialog();
            };

            btnDelete.Click += delegate
            {
                Oven selectedOven = (Oven)dgOven.SelectedItem;
                if(selectedOven == null)
                {
                    MessageBox.Show("Выберите удаляемую запись!");
                    return;
                }
                if(MessageBox.Show("Подтвердите удаление!", "Подтвердите удаление!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    selectedOven.remove();
                }
            };

            btnUp.Click += delegate
            {
                Oven oven = dgOven.SelectedItem as Oven;
                if (oven == null) return;
                up(oven);
            };

            btnDown.Click += delegate
            {
                Oven oven = dgOven.SelectedItem as Oven;
                if (oven == null) return;
                down(oven);
            };
        }

        private void rowUp(object sender, RoutedEventArgs e)
        {
            Button upButton = sender as Button;
            if (upButton == null) return;

            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(upButton);
            if (dgrow == null) return;

            Oven oven = (Oven)dgrow.Item;
            if (oven == null) return;

            if(!oven.up())
            {
                MessageBox.Show("При выполнении запроса произошли ошибки!\nПодробнее см. в логе ошибок.");
            }

        }

        private void rowDown(object sender, RoutedEventArgs e)
        {
            Button downButton = sender as Button;
            if (downButton == null) return;

            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(downButton);
            if (dgrow == null) return;

            Oven oven = (Oven)dgrow.Item;
            if (oven == null) return;

            if (!oven.down())
            {
                MessageBox.Show("При выполнении запроса произошли ошибки!\nПодробнее см. в логе ошибок.");
            }
        }

        private void up(Oven oven)
        {
            if (oven == null) return;
            if (!oven.up())
            {
                MessageBox.Show("При выполнении запроса произошли ошибки!\nПодробнее см. в логе ошибок.");
            }
        }

        private void down (Oven oven)
        {
            if (oven == null) return;
            if (!oven.down())
            {
                MessageBox.Show("При выполнении запроса произошли ошибки!\nПодробнее см. в логе ошибок.");
            }
        }

        private void dgOven_Sorting(object sender, DataGridSortingEventArgs e)
        {
            IniFile iniFile = new IniFile("settings.ini");
            iniFile.Write("oven_sort_column", e.Column.SortMemberPath);
            OrderSettings.OvenSortColumn = e.Column.SortMemberPath;

            string sortDirection = e.Column.SortDirection.ToString();
            if(string.IsNullOrEmpty(sortDirection) || sortDirection == "Descending")
            {
                iniFile.Write("oven_sort_direction", "Ascending");
                OrderSettings.OvenSortDirection = ListSortDirection.Ascending;
            }
            else
            {
                iniFile.Write("oven_sort_direction", "Descending");
                OrderSettings.OvenSortDirection = ListSortDirection.Descending;
            }
        }
    }
}
