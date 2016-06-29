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
    /// Логика взаимодействия для OvenPage.xaml
    /// </summary>
    public partial class OvenPage : Page
    {
        public OvenPage()
        {
            InitializeComponent();
            DataContext = new { Collection = OvenController.instance().Collection };

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
    }
}
