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
                Collection = DetailsController.instance().Collection
            };

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
        }
    }
}
