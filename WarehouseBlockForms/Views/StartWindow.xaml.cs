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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WarehouseBlockForms.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();

            rbtnUserMode.IsChecked = true;

            row_rbtnUserModeSelect.MouseUp += delegate
            {
                rbtnUserMode.IsChecked = true;
            };
            row_rbtnAdministratorModeSelect.MouseUp += delegate
            {
                rbtnAdministratorMode.IsChecked = true;
            };
            rbtnAdministratorMode.Checked += delegate
            {
                dpPassword.Visibility = Visibility.Visible;
            };
            rbtnUserMode.Checked += delegate
            {
                dpPassword.Visibility = Visibility.Collapsed;
            };
            btnOk.Click += delegate
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            };
            btnCancel.Click += delegate
            {
                Close();
            };
        }
    }
}
