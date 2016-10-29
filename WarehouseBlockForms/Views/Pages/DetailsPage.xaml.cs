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
using WarehouseBlockForms.Classess;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для DetailsPage.xaml
    /// </summary>
    public partial class DetailsPage : Page
    {
        public DetailsPage()
        {
            InitializeComponent();

            DataContext = new
            {
                Collection = DetailsController.instance().ViewSource.View
            };

            DetailsController.instance().ViewSource.SortDescriptions.Add(new SortDescription(OrderSettings.DetailsSortColumn, OrderSettings.DetailsSortDirection));

            btnAdd.Click += delegate
            {
                DetailsSaveWindow detailsSaveWindow = new DetailsSaveWindow();
                detailsSaveWindow.ShowDialog();
            };

            btnEdit.Click += delegate
            {
                Details selectedDetails = (Details)dgDetails.SelectedItem;
                if (selectedDetails == null)
                {
                    MessageBox.Show("Выберите редактируемую запись!");
                    return;
                }
                DetailsSaveWindow detailsSaveWindow = new DetailsSaveWindow(selectedDetails);
                detailsSaveWindow.ShowDialog();
            };

            btnDelete.Click += delegate
            {
                Details selectedDetails = (Details)dgDetails.SelectedItem;
                if (selectedDetails == null)
                {
                    MessageBox.Show("Выберите удаляемую запись!");
                    return;
                }
                if (MessageBox.Show("Подтвердите удаление!", "Подтвердите удаление!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    selectedDetails.remove();
                }
            };

            btnUp.Click += delegate
            {
                Details details = dgDetails.SelectedItem as Details;
                if (details == null) return;
                up(details);
            };

            btnDown.Click += delegate
            {
                Details details = dgDetails.SelectedItem as Details;
                if (details == null) return;
                down(details);
            };
        }

        private void rowUp(object sender, RoutedEventArgs e)
        {
            Button upButton = sender as Button;
            if (upButton == null) return;

            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(upButton);
            if (dgrow == null) return;

            Details detail = (Details)dgrow.Item;
            if (detail == null) return;

            if (!detail.up())
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

            Details detail = (Details)dgrow.Item;
            if (detail == null) return;

            if (!detail.down())
            {
                MessageBox.Show("При выполнении запроса произошли ошибки!\nПодробнее см. в логе ошибок.");
            }
        }

        private void up(Details details)
        {
            if (details == null) return;
            if (!details.up())
            {
                MessageBox.Show("При выполнении запроса произошли ошибки!\nПодробнее см. в логе ошибок.");
            }
        }

        private void down(Details details)
        {
            if (details == null) return;
            if (!details.down())
            {
                MessageBox.Show("При выполнении запроса произошли ошибки!\nПодробнее см. в логе ошибок.");
            }
        }

        private void dgDetails_Sorting(object sender, DataGridSortingEventArgs e)
        {
            IniFile iniFile = new IniFile("settings.ini");
            iniFile.Write("details_sort_column", e.Column.SortMemberPath);
            OrderSettings.DetailsSortColumn = e.Column.SortMemberPath;
            string sortDirection = e.Column.SortDirection.ToString();
            if (string.IsNullOrEmpty(sortDirection) || sortDirection == "Descending")
            {
                iniFile.Write("details_sort_direction", "Ascending");
                OrderSettings.DetailsSortDirection = ListSortDirection.Ascending;
            }
            else
            {
                iniFile.Write("details_sort_direction", "Descending");
                OrderSettings.DetailsSortDirection = ListSortDirection.Descending;
            }
        }
    }
}
