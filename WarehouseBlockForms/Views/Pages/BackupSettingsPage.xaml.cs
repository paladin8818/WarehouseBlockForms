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
        IniFile iniFile = new IniFile("settings.ini");
        public BackupSettingsPage()
        {
            InitializeComponent();

            if (iniFile.KeyExists("backup_directory_path"))
            {
                tbxCurrentDirectoryPath.Text = iniFile.Read("backup_directory_path");
            }
            if (iniFile.KeyExists("backup_period", "BACKUP PERIOD"))
            {
                setPeriodSettings();
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



        private void setPeriodSettings()
        {
            string report_period = iniFile.Read("backup_period", "BACKUP PERIOD");
            switch (report_period)
            {
                case "day":
                    cbxBackupPeriod.SelectedIndex = 0;
                    if (iniFile.KeyExists("time", "BACKUP PERIOD")) tbx_day_time.Text = iniFile.Read("time", "BACKUP PERIOD");
                    break;
                case "week":
                    cbxBackupPeriod.SelectedIndex = 1;
                    if (iniFile.KeyExists("day", "BACKUP PERIOD")) cbx_week_day.SelectedIndex = (Int32.Parse(iniFile.Read("day", "BACKUP PERIOD")) - 1);
                    if (iniFile.KeyExists("time", "BACKUP PERIOD")) tbx_week_time.Text = iniFile.Read("time", "BACKUP PERIOD");
                    break;
                case "month":
                    cbxBackupPeriod.SelectedIndex = 2;
                    if (iniFile.KeyExists("day", "BACKUP PERIOD")) cbx_month_day.SelectedIndex = (Int32.Parse(iniFile.Read("day", "BACKUP PERIOD")) - 1);
                    if (iniFile.KeyExists("time", "BACKUP PERIOD")) tbx_month_time.Text = iniFile.Read("time", "BACKUP PERIOD");
                    break;
            }
        }

        private void btnSavePeriodBackup_Click(object sender, RoutedEventArgs e)
        {
            if (cbxBackupPeriod.SelectedIndex == -1)
            {
                System.Windows.MessageBox.Show("Укажите период выгрузки");
                return;
            }
            string period = (cbxBackupPeriod.SelectedValue as ComboBoxItem).Tag.ToString();
            switch (period)
            {
                case "cbxValue_day":
                    saveDayPeriod();
                    break;
                case "cbxValue_week":
                    saveWeekPeriod();
                    break;
                case "cbxValue_month":
                    saveMonthPeriod();
                    break;
            }
        }

        private void saveDayPeriod()
        {
            if (tbx_day_time.Text == "")
            {
                System.Windows.MessageBox.Show("Укажите время выгрузки!");
                return;
            }
            iniFile.Write("backup_period", "day", "BACKUP PERIOD");
            iniFile.Write("time", tbx_day_time.Text, "BACKUP PERIOD");
            if (iniFile.KeyExists("day"))
            {
                iniFile.DeleteKey("day", "BACKUP PERIOD");
            }
            MessageBox.Show("Расписание резервного копирования сохранено.");
        }

        private void saveWeekPeriod()
        {
            if (cbx_week_day.SelectedIndex == -1)
            {
                System.Windows.MessageBox.Show("Укажите день выгрузки!");
                return;
            }
            if (tbx_week_time.Text == "")
            {
                System.Windows.MessageBox.Show("Укажите время выгрузки!");
                return;
            }
            iniFile.Write("backup_period", "week", "BACKUP PERIOD");
            iniFile.Write("day", (1 + (int)cbx_week_day.SelectedIndex).ToString(), "BACKUP PERIOD");
            iniFile.Write("time", tbx_week_time.Text, "BACKUP PERIOD");
            MessageBox.Show("Расписание резервного копирования сохранено.");
        }

        private void saveMonthPeriod()
        {
            if (cbx_month_day.SelectedIndex == -1)
            {
                System.Windows.MessageBox.Show("Укажите день выгрузки!");
                return;
            }
            if (tbx_month_time.Text == "")
            {
                System.Windows.MessageBox.Show("Укажите время выгрузки!");
                return;
            }
            iniFile.Write("backup_period", "month", "BACKUP PERIOD");
            iniFile.Write("day", (1 + (int)cbx_month_day.SelectedIndex).ToString(), "BACKUP PERIOD");
            iniFile.Write("time", tbx_month_time.Text, "BACKUP PERIOD");
            MessageBox.Show("Расписание резервного копирования сохранено.");
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
