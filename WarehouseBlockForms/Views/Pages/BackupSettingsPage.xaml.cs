using System.Windows;
using System.Windows.Controls;
using WarehouseBlockForms.Classess;

namespace WarehouseBlockForms.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для BackupSettingsPage.xaml
    /// </summary>
    public partial class BackupSettingsPage : Page
    {
        IniFile iniFile = new IniFile("settings.ini");
        public BackupSettingsPage()
        {
            InitializeComponent();

            if (iniFile.KeyExists("backup_directory_path"))
            {
                tbxCurrentDirectoryPath.Text = iniFile.Read("backup_directory_path");
            }

            btnOpenDirectoryPath.Click += delegate
            {
                var dlg = new System.Windows.Forms.FolderBrowserDialog();
                System.Windows.Forms.DialogResult result = dlg.ShowDialog(this.GetIWin32Window());
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    saveReportsPath(dlg.SelectedPath);
                }
            };

        }

        private void saveReportsPath(string path)
        {
            tbxCurrentDirectoryPath.Text = path;
            iniFile.Write("backup_directory_path", path);
        }

        private void changeCreateBackupPeriod(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox cbx = sender as System.Windows.Controls.ComboBox;
            ComboBoxItem currentSelected = cbx.SelectedValue as ComboBoxItem;
            switch (currentSelected.Tag.ToString())
            {
                case "cbxValue_day":
                    g_day.Visibility = Visibility.Visible;
                    g_week.Visibility = Visibility.Collapsed;
                    g_month.Visibility = Visibility.Collapsed;
                    break;
                case "cbxValue_week":
                    g_day.Visibility = Visibility.Collapsed;
                    g_week.Visibility = Visibility.Visible;
                    g_month.Visibility = Visibility.Collapsed;
                    break;
                case "cbxValue_month":
                    g_day.Visibility = Visibility.Collapsed;
                    g_week.Visibility = Visibility.Collapsed;
                    g_month.Visibility = Visibility.Visible;
                    break;
            }
        }

    }
}
