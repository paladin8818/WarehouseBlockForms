using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using WarehouseBlockForms.Classess;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;
using Xceed.Wpf.Toolkit;

namespace WarehouseBlockForms.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReportSettingsPage.xaml
    /// </summary>
    public partial class ReportSettingsPage : Page
    {
        public ReportSettingsPage()
        {
            InitializeComponent();
            DataContext = new
            {
                Collection = ReportsSettingController.instance().Collection
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;
            if (button == null) return;
            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(button);
            if (dgrow == null) return;

            ReportsSetting reportsSetting = (ReportsSetting)dgrow.Item;
            if (reportsSetting == null) return;

            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dlg.ShowDialog(this.GetIWin32Window());
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                reportsSetting.ReportPath = dlg.SelectedPath;
                //reportsSetting.save();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox comboBox = sender as System.Windows.Controls.ComboBox;
            if (comboBox == null) return;
            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(comboBox);
            if (dgrow == null) return;

            ReportsSetting reportsSetting = (ReportsSetting)dgrow.Item;
            if (reportsSetting == null) return;
            if (comboBox.SelectedValue == null) return;
            if (reportsSetting.Period == comboBox.SelectedValue.ToString()) return;
            reportsSetting.Period = comboBox.SelectedValue.ToString();


            reportsSetting.Day = null;
            reportsSetting.Time = "";
        }

        private void cbxWeekDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox comboBox = sender as System.Windows.Controls.ComboBox;
            if (comboBox == null) return;
            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(comboBox);
            if (dgrow == null) return;

            ReportsSetting reportsSetting = (ReportsSetting)dgrow.Item;
            if (reportsSetting == null) return;
            if (comboBox.SelectedValue == null) return;
            if (reportsSetting.Period == comboBox.SelectedValue.ToString()) return;

            reportsSetting.Day = Int32.Parse(comboBox.SelectedValue.ToString());

            reportsSetting.Time = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;
            if (button == null) return;
            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(button);
            if (dgrow == null) return;

            ReportsSetting reportsSetting = (ReportsSetting)dgrow.Item;
            if (reportsSetting == null) return;
            if(!reportsSetting.save())
            {
                System.Windows.MessageBox.Show("Не удалось сохранить настройку отчета. Подробности см. в логе ошибок!");
            }
            else
            {
                System.Windows.MessageBox.Show("Настройка успешно сохранена!");
                DateTime? nextDate = reportsSetting.calculateNextDate();
                if(nextDate != null)
                {
                    DateTime date = (DateTime)nextDate;
                    System.Windows.MessageBox.Show(date.ToString("dd.MM.yyyy HH:mm:ss"));
                    //System.Windows.MessageBox.Show(((int)DateTime.Now.DayOfWeek).ToString());

                }
            }
        }

        private void tbxTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TimePicker timePicker = sender as TimePicker;
            if (timePicker == null) return;
            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(timePicker);
            if (dgrow == null) return;

            ReportsSetting reportsSetting = (ReportsSetting)dgrow.Item;
            if (reportsSetting == null) return;
            reportsSetting.Time = timePicker.Text;

        }
    }
}
