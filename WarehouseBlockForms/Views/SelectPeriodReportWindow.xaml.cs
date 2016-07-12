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
using System.Windows.Shapes;

namespace WarehouseBlockForms.Views
{
    /// <summary>
    /// Логика взаимодействия для SelectPeriodReportWindow.xaml
    /// </summary>
    public partial class SelectPeriodReportWindow : Window
    {
        Action<DateTime, DateTime> callback;
        public SelectPeriodReportWindow(Action<DateTime, DateTime> callback)
        {
            InitializeComponent();
            this.callback = callback;

            int currentDay = DateTime.Now.Day;
            dtpStart.SelectedDate = DateTime.Now.AddDays(-(currentDay - 1));
            dtpEnd.SelectedDate = DateTime.Now;

            btnCancel.Click += delegate
            {
                Close();
            };
            btnOk.Click += delegate
            {
                if(dtpStart.SelectedDate.Value == null)
                {
                    MessageBox.Show("Введите дату с");
                    return;
                }
                if (dtpEnd.SelectedDate.Value == null)
                {
                    MessageBox.Show("Введите дату по");
                    return;
                }
                callback((DateTime)dtpStart.SelectedDate.Value, (DateTime)dtpEnd.SelectedDate.Value);
                Close();
            };
            
        }
    }
}
