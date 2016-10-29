using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WarehouseBlockForms.Classess;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IniFile iniFile = new IniFile("settings.ini");
        private bool administratorMode = false;
        private bool realClosing = false;
        private static MainWindow _mainWindow;

        private System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();

        public MainWindow()
        {

            InitializeComponent();

            if(CheckSingleProcess() == false)
            {
                realClosing = true;
                Close();
                return;
            }

            _mainWindow = this;

            //Notify

            notifyIcon.Text = "Склад блок-форм";
            notifyIcon.Icon = new System.Drawing.Icon("logo.ico");

            notifyIcon.Click += delegate
            {
                this.Visibility = Visibility.Visible;
                ShowInTaskbar = true;
            };

            if(DataBase.Connect() == null)
            {
                MessageBox.Show("Ошибка открытия базы данных.\nРабота приложения будет остановлена.");
                Close();
                return;
            }

            //loadModels();

            
            setWorkPages();
            setSettingsPages();

            setSortSettings();

            btnLoginAdmin.Click += delegate
            {
                Login();
            };

            Task.Factory.StartNew(new Action(() =>
            {
                Thread.Sleep(300000);
                while (true)
                {
                    createAvailabilityReportPeriod();
                    createTurnReportPeriod();
                    createSupplyRepoertPeriod();
                    createWriteoffReportPeriod();
                    createBackup();
                    Thread.Sleep(300000);
                }
            }));
        }

        private void createAvailabilityReportPeriod ()
        {
            ReportsSetting reportSetting = ReportsSettingController.instance().getByProgramName("AvailabilityReport");
            if (reportSetting.NextDateCreated == null) return;
            DateTime nextDate = (DateTime)reportSetting.NextDateCreated;
            DateTime current = DateTime.Now;
            if (current < nextDate) return;

            Reports.AvailabilityReport report = new Reports.AvailabilityReport();
            report.Postfix = "сформирован автоматически";
            report.H1 = "Отчет наличия на " + nextDate.ToString("dd.MM.yyyy") + " (сформирован автоматически, " + current.ToString("dd.MM.yyyy HH:mm:ss") + ")";
            if (report.Save())
            {
                reportSetting.save();
            }
        }

        private void createTurnReportPeriod()
        {
            ReportsSetting reportSetting = ReportsSettingController.instance().getByProgramName("TurnReport");
            if (reportSetting.NextDateCreated == null) return;
            DateTime nextDate = (DateTime)reportSetting.NextDateCreated;
            DateTime current = DateTime.Now;
            if (current < nextDate) return;

            DateTime prev = new DateTime(current.Year, current.Month, 1, 0, 0, 0);
            Reports.TurnReport report = new Reports.TurnReport();
            report.Postfix = "сформирован автоматически " + current.ToString("dd.MM.yyyy HH_mm_ss");
            report.H1 = "Отчет оборота с " + prev.ToString("dd.MM.yyyy") + " по " + nextDate.ToString("dd.MM.yyyy") + " (сформирован автоматически, " + current.ToString("dd.MM.yyyy HH:mm:ss") + ")";
            if (report.Save(prev, nextDate))
            {
                reportSetting.save();
            }
        }

        private void createSupplyRepoertPeriod()
        {
            ReportsSetting reportSetting = ReportsSettingController.instance().getByProgramName("SupplyReport");
            if (reportSetting.NextDateCreated == null) return;
            DateTime nextDate = (DateTime)reportSetting.NextDateCreated;
            DateTime current = DateTime.Now;
            if (current < nextDate) return;

            DateTime prev = new DateTime(current.Year, current.Month, 1, 0, 0, 0);

            Reports.SupplyReport report = new Reports.SupplyReport();
            report.Postfix = "сформирован автоматически " + current.ToString("dd.MM.yyyy HH_mm_ss");
            report.H1 = "Журнал поступления с " + prev.ToString("dd.MM.yyyy") + " по " + nextDate.ToString("dd.MM.yyyy") + " (сформирован автоматически, " + current.ToString("dd.MM.yyyy HH:mm:ss") + ")";
            if (report.Save(prev, nextDate))
            {
                reportSetting.save();
            }
        }
        private void createWriteoffReportPeriod()
        {
            ReportsSetting reportSetting = ReportsSettingController.instance().getByProgramName("WriteoffReport");
            if (reportSetting.NextDateCreated == null) return;
            DateTime nextDate = (DateTime)reportSetting.NextDateCreated;
            DateTime current = DateTime.Now;
            if (current < nextDate) return;

            DateTime prev = new DateTime(current.Year, current.Month, 1, 0, 0, 0);

            Reports.WriteoffReport report = new Reports.WriteoffReport();
            report.Postfix = "сформирован автоматически, " + current.ToString("dd.MM.yyyy HH_mm_ss");
            report.H1 = "Журнал списания с " + prev.ToString("dd.MM.yyyy") + " по " + nextDate.ToString("dd.MM.yyyy") + " (сформирован автоматически, " + current.ToString("dd.MM.yyyy HH:mm:ss") + ")";
            if (report.Save(prev, nextDate))
            {
                reportSetting.save();
            }
        }

        private void createBackup()
        {
            ReportsSetting reportSetting = ReportsSettingController.instance().getByProgramName("DBBackup");
            if (reportSetting.NextDateCreated == null) return;
            DateTime nextDate = (DateTime)reportSetting.NextDateCreated;
            DateTime current = DateTime.Now;
            if (current < nextDate) return;
            try
            {
                DataBase.Backup(reportSetting.ReportPath);
                reportSetting.save();
            }
            catch (Exception ex)
            {
                Log.WriteError("В ходе создания резервной копии произошли ошибки!\n" + ex.Message);
            }
        }

        private void setWorkPages()
        {
            Uri supplyPageUri = new Uri("/Views/Pages/SupplyPage.xaml", UriKind.Relative);
            frameSupply.Source = supplyPageUri;

            Uri writeoffPageUri = new Uri("/Views/Pages/WriteoffPage.xaml", UriKind.Relative);
            frameWriteoff.Source = writeoffPageUri;

            Uri availabilityPageUri = new Uri("/Views/Pages/AvailabilityPage.xaml", UriKind.Relative);
            frameAvailability.Source = availabilityPageUri;

            Uri sheetPageUri = new Uri("/Views/Pages/SheetPage.xaml", UriKind.Relative);
            frameSheet.Source = sheetPageUri;
        }

        private void setSettingsPages ()
        {
            Uri ovenPageUri = new Uri("/Views/Pages/OvenPage.xaml", UriKind.Relative);
            frameSettingsOven.Source = ovenPageUri;

            Uri detailsPageUri = new Uri("/Views/Pages/DetailsPage.xaml", UriKind.Relative);
            frameSettingsDetails.Source = detailsPageUri;

            Uri recipientsPageUri = new Uri("/Views/Pages/RecipientsPage.xaml", UriKind.Relative);
            frameSettingsRecipients.Source = recipientsPageUri;

            Uri reportPageUri = new Uri("/Views/Pages/ReportSettingsPage.xaml", UriKind.Relative);
            frameSettingsReports.Source = reportPageUri;

            Uri backupPageUri = new Uri("/Views/Pages/BackupSettingsPage.xaml", UriKind.Relative);
            frameSettingsBackup.Source = backupPageUri;

            Uri securityPageUri = new Uri("/Views/Pages/SecuritySettingsPage.xaml", UriKind.Relative);
            frameSettingsSecurity.Source = securityPageUri;
        }

        private void Login ()
        {
            if (tbxPassword.Password == "")
            {
                MessageBox.Show("Введите пароль!");
                return;
            }
            if (PasswordValidate.Validate(tbxPassword.Password))
            {
                grdLoginAdmin.Visibility = Visibility.Collapsed;
                tcSettings.Visibility = Visibility.Visible;
                administratorMode = true;
            }
            else
            {
                MessageBox.Show("Введенный пароль некорректен!");
            }
        }

        private void tbxPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Login();
            }
        }

        private void baseTabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(administratorMode == true 
                && e.Source is TabControl && tiSettings.IsSelected == false)
            {
                administratorMode = false;
                grdLoginAdmin.Visibility = Visibility.Visible;
                tcSettings.Visibility = Visibility.Collapsed;
                tbxPassword.Password = "";
            }
        }

        public static void ExitApp()
        {
            _mainWindow.realClosing = true;
            _mainWindow.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!realClosing)
            {
                e.Cancel = true;
                ShowInTaskbar = false;
                Visibility = Visibility.Hidden;
                notifyIcon.Visible = true;
                notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                notifyIcon.BalloonTipText = "Программа свернута в трей";
                notifyIcon.BalloonTipTitle = "Программа 'Склад блок-форм' свернута в трей";
                notifyIcon.ShowBalloonTip(10);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            realClosing = true;
            Close();
        }

        private bool CheckSingleProcess ()
        {
            Process pThis = Process.GetCurrentProcess();

            Process[] ps = Process.GetProcessesByName(pThis.ProcessName);

            bool flag = false;

            foreach (Process p in ps)

                if (p.StartInfo.FileName == pThis.StartInfo.FileName)

                    if (flag)

                        return false;

                    else

                        flag = true;

            return true;
        }


        private void loadModels()
        {
            OvenController.instance();
            DetailsController.instance();
            RecipientsController.instance();
            SupplyController.instance();
            SupplyDetailsController.instance();
            WriteoffController.instance();
            WriteoffDetailsController.instance();
            ReportsSettingController.instance();
        }

        private void setSortSettings ()
        {
            if (iniFile.KeyExists("oven_sort_column"))
            {
                string o_column = iniFile.Read("oven_sort_column");
                if (!string.IsNullOrEmpty(o_column))
                {
                    OrderSettings.OvenSortColumn = o_column;
                }
            }
            if (iniFile.KeyExists("oven_sort_direction"))
            {
                string o_direction = iniFile.Read("oven_sort_direction");
                if (o_direction == "Descending")
                {
                    OrderSettings.OvenSortDirection = ListSortDirection.Descending;
                }
            }

            if (iniFile.KeyExists("details_sort_column"))
            {
                string o_column = iniFile.Read("details_sort_column");
                if (!string.IsNullOrEmpty(o_column))
                {
                    OrderSettings.DetailsSortColumn = o_column;
                }
            }
            if (iniFile.KeyExists("details_sort_direction"))
            {
                string o_direction = iniFile.Read("details_sort_direction");
                if (o_direction == "Descending")
                {
                    OrderSettings.DetailsSortDirection = ListSortDirection.Descending;
                }
            }

            if (iniFile.KeyExists("recipients_sort_column"))
            {
                string o_column = iniFile.Read("recipients_sort_column");
                if (!string.IsNullOrEmpty(o_column))
                {
                    OrderSettings.RecipientsSortColumn = o_column;
                }
            }
            if (iniFile.KeyExists("recipients_sort_direction"))
            {
                string o_direction = iniFile.Read("recipients_sort_direction");
                if (o_direction == "Descending")
                {
                    OrderSettings.RecipientsSortDirection = ListSortDirection.Descending;
                }
            }
        }

    }
}