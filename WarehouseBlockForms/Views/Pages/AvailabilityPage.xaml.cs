using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;
using WarehouseBlockForms.Reports;
using WarehouseBlockForms.Reports.Base;

namespace WarehouseBlockForms.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AvailabilityPage.xaml
    /// </summary>
    public partial class AvailabilityPage : Page
    {
        public AvailabilityPage()
        {
            InitializeComponent();

            DataContext = new
            {
                Collection = DetailsController.instance().ViewSource.View
            };
            cbxOvenFilter.ItemsSource = OvenController.instance().Collection;

            cbxOvenFilter.SelectionChanged += delegate
            {
                int selectedValue = (cbxOvenFilter.SelectedIndex != -1) ? (int)cbxOvenFilter.SelectedValue : 0;
                DetailsController.instance().showFromOvenId(selectedValue);
            };
            btnClearOvenFilter.Click += delegate
            {
                cbxOvenFilter.SelectedIndex = -1;
            };
            tbxFastFilter.TextChanged += delegate
            {
                DetailsController.instance().showFromText(tbxFastFilter.Text);
            };
            btnClearFastFilter.Click += delegate
            {
                tbxFastFilter.Text = "";
            };

            btnCreateAvailabilityReport.Click += delegate
            {
                createAvailabilityReport();
            };

        }

        private void createAvailabilityReport ()
        {
            try
            {
                AvailabilityReport report = new AvailabilityReport();
                report.Postfix = "сформирован вручную";
                report.H1 = report.ReportName;
                if (report.Save())
                {
                    MessageBox.Show("Отчет сохранен!");
                }
                else
                {
                    MessageBox.Show("При формировании отчета произошли ошибки!\nПодробности см. в логе ошибок.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
