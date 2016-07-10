using System.Windows;
using System.Windows.Controls;
using WarehouseBlockForms.Classess;


namespace WarehouseBlockForms.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для SecuritySettingsPage.xaml
    /// </summary>
    public partial class SecuritySettingsPage : Page
    {
        IniFile iniFile = new IniFile("settings.ini");
        public SecuritySettingsPage()
        {
            InitializeComponent();

            btnSave.Click += BtnSave_Click;


        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if(!PasswordValidate.Validate(tbxOldPassword.Password))
            {
                MessageBox.Show("Введите корректный старый пароль!");
                return;
            }
            if(tbxNewPassword.Password != tbxNewPasswordRepeat.Password)
            {
                MessageBox.Show("Новый пароль и подтверждение не совпадают!");
                return;
            }

            if(tbxNewPassword.Password.Length < 6)
            {
                MessageBox.Show("Пароль должен быть не менее 6 символов!");
                return;
            }

            string hashPassword = PasswordValidate.CalculateHash(tbxNewPassword.Password);
            iniFile.Write("phash", hashPassword);
            MessageBox.Show("Новый пароль успешно сохранен!");

            tbxNewPassword.Password = "";
            tbxNewPasswordRepeat.Password = "";
            tbxOldPassword.Password = "";
        }

    }
}
