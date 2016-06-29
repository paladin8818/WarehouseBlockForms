using System.Windows;
using WarehouseBlockForms.Classess;

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
                openApp();
            };
            btnCancel.Click += delegate
            {
                Close();
            };
        }

        private void openApp ()
        {
            if(rbtnUserMode.IsChecked == true)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            else
            {
                if(tbxPassword.Password == "")
                {
                    MessageBox.Show("Пароль не может быть пустым!");
                    return;
                }
                if(PasswordValidate.Validate(tbxPassword.Password))
                {
                    MainWindow mainWindow = new MainWindow(true);
                    mainWindow.Show();
                    Close();
                }
            }
        }
    }
}
