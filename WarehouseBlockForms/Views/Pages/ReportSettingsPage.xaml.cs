using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using WarehouseBlockForms.Classess;

namespace WarehouseBlockForms.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReportSettingsPage.xaml
    /// </summary>
    public partial class ReportSettingsPage : Page
    {
        IniFile iniFile = new IniFile("settings.ini");
        public ReportSettingsPage()
        {
            InitializeComponent();

            if(iniFile.KeyExists("report_directory_path"))
            {
                tbxCurrentDirectoryPath.Text = iniFile.Read("report_directory_path");
            }
            if(iniFile.KeyExists("report_period", "REPORTS PERIOD"))
            {
                setPeriodSettings();
            }

            btnOpenDirectoryPath.Click += delegate
            {
                var dlg = new FolderBrowserDialog();
                System.Windows.Forms.DialogResult result = dlg.ShowDialog(this.GetIWin32Window());
                if(result == DialogResult.OK)
                {
                    saveReportsPath(dlg.SelectedPath);
                }
            };
        }

        private void saveReportsPath (string path)
        {
            tbxCurrentDirectoryPath.Text = path;
            iniFile.Write("report_directory_path", path);
        }

        private void changeCreateReportPeriod(object sender, SelectionChangedEventArgs e)
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

        private void btnSaveReportPeriod_Click(object sender, RoutedEventArgs e)
        {
            if(cbxReportPeriod.SelectedIndex == -1)
            {
                System.Windows.MessageBox.Show("Укажите период выгрузки");
                return;
            }
            string period = (cbxReportPeriod.SelectedValue as ComboBoxItem).Tag.ToString();
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

        private void saveDayPeriod ()
        {
            if(tbx_day_time.Text == "")
            {
                System.Windows.MessageBox.Show("Укажите время выгрузки!");
                return;
            }
            iniFile.Write("report_period", "day", "REPORTS PERIOD");
            iniFile.Write("time", tbx_day_time.Text, "REPORTS PERIOD");
            if(iniFile.KeyExists("day"))
            {
                iniFile.DeleteKey("day", "REPORTS PERIOD");
            }
        }

        private void saveWeekPeriod ()
        {
            if(cbx_week_day.SelectedIndex == -1)
            {
                System.Windows.MessageBox.Show("Укажите день выгрузки!");
                return;
            }
            if(tbx_week_time.Text == "")
            {
                System.Windows.MessageBox.Show("Укажите время выгрузки!");
                return;
            }
            iniFile.Write("report_period", "week", "REPORTS PERIOD");
            iniFile.Write("day", (1 + (int)cbx_week_day.SelectedIndex).ToString(), "REPORTS PERIOD");
            iniFile.Write("time", tbx_week_time.Text, "REPORTS PERIOD");
        }

        private void saveMonthPeriod ()
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
            iniFile.Write("report_period", "month", "REPORTS PERIOD");
            iniFile.Write("day", (1 + (int)cbx_month_day.SelectedIndex).ToString(), "REPORTS PERIOD");
            iniFile.Write("time", tbx_month_time.Text, "REPORTS PERIOD");
        }

        private void setPeriodSettings()
        {
            string report_period = iniFile.Read("report_period", "REPORTS PERIOD");
            switch(report_period)
            {
                case "day":
                    cbxReportPeriod.SelectedIndex = 0;
                    if (iniFile.KeyExists("time", "REPORTS PERIOD")) tbx_day_time.Text = iniFile.Read("time", "REPORTS PERIOD");
                    break;
                case "week":
                    cbxReportPeriod.SelectedIndex = 1;
                    if (iniFile.KeyExists("day", "REPORTS PERIOD")) cbx_week_day.SelectedIndex = (Int32.Parse(iniFile.Read("day", "REPORTS PERIOD")) - 1);
                    if (iniFile.KeyExists("time", "REPORTS PERIOD")) tbx_week_time.Text = iniFile.Read("time", "REPORTS PERIOD");
                    break;
                case "month":
                    cbxReportPeriod.SelectedIndex = 2;
                    if (iniFile.KeyExists("day", "REPORTS PERIOD")) cbx_month_day.SelectedIndex = (Int32.Parse(iniFile.Read("day", "REPORTS PERIOD")) - 1);
                    if (iniFile.KeyExists("time", "REPORTS PERIOD")) tbx_month_time.Text = iniFile.Read("time", "REPORTS PERIOD");
                    break;
            }
        }
    }
}
