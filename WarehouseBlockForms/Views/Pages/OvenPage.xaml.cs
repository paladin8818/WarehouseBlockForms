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
            OvenController.instance().ViewSource.SortDescriptions.Add(new SortDescription("RowOrder", ListSortDirection.Ascending));

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
    }
}
