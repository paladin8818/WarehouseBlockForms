using System;
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
        public BackupSettingsPage()
        {
            InitializeComponent();
        }

        private void btnCreateBackupNow_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dlg.ShowDialog(this.GetIWin32Window());
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    DataBase.Backup(dlg.SelectedPath);
                    MessageBox.Show("Резервная копия успешно создана!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("В ходе создания резервной копии произошли ошибки!\n" + ex.Message);
                }
                
            }
        }

        private void btnRestoreBackup_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show(
                "После восстановления резервной копии приложение будет закрыто. Продолжить?",
                "Восстановление резервной копии.", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "database file|*.sqlite";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        DataBase.Restore(ofd.FileName);
                        MessageBox.Show("Резервная копия восстановлена. Приложение будет закрыто.");
                        MainWindow.ExitApp();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Не удалось восстановить резервную копию БД\n" + ex.Message);
                    }
                }
            }
        }
    }
}
