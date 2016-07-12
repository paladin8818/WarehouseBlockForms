using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WarehouseBlockForms.Helpers;
using WarehouseBlockForms.Reports;
namespace WarehouseBlockForms.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для SheetPage.xaml
    /// </summary>
    public partial class SheetPage : Page
    {
        public SheetPage()
        {
            InitializeComponent();

            DataContext = new {
                SWCollection = SWCollection.instance().ViewSource.View
            };

            SWCollection.instance().ViewSource.SortDescriptions.Add(new SortDescription("OperationDate", ListSortDirection.Descending));
            btnCreateTurnReport.Click += delegate
            {

                SelectPeriodReportWindow sprWindow = new SelectPeriodReportWindow(new Action<DateTime, DateTime>((d1, d2) => { createTurnReport(d1, d2); }));
                sprWindow.ShowDialog();
            };

            btnCreateSupplyReport.Click += delegate
            {
                SelectPeriodReportWindow sprWindow = new SelectPeriodReportWindow(new Action<DateTime, DateTime>((d1, d2) => { createSupplyReport(d1, d2); }));
                sprWindow.ShowDialog();
            };

            btnCreateWriteoffReport.Click += delegate
            {
                SelectPeriodReportWindow sprWindow = new SelectPeriodReportWindow(new Action<DateTime, DateTime>((d1, d2) => { createWriteoffReport(d1, d2); }));
                sprWindow.ShowDialog();
            };
        }

        private void dgAvailability_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SWHelper swHelper = dgAvailability.SelectedItem as SWHelper;
            if (swHelper == null) return;
            dgDetailed.ItemsSource = swHelper.CurrentDetails;
        }

        private void createTurnReport(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                TurnReport report = new TurnReport();
                report.Postfix = "сформирован вручную";
                if (startDate == null)
                {
                    int currentDay = DateTime.Now.Day;
                    startDate = DateTime.Now.AddDays(-(currentDay - 1));
                }
                if (endDate == null)
                {
                    endDate = DateTime.Now;
                }
                report.H1 = report.ReportName;

                if (report.Save((DateTime)startDate, (DateTime)endDate))
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

        private void createSupplyReport(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                SupplyReport report = new SupplyReport();
                report.Postfix = "сформирован вручную";
                if (startDate == null)
                {
                    int currentDay = DateTime.Now.Day;
                    startDate = DateTime.Now.AddDays(-(currentDay - 1));
                }
                if (endDate == null)
                {
                    endDate = DateTime.Now;
                }

                report.H1 = "Журнал поступления с " + ((DateTime)startDate).ToString("dd.MM.yyyy") +
                    " по " + ((DateTime)endDate).ToString("dd.MM.yyyy") + ", сформирован вручную " + DateTime.Now.ToString("dd.MM.yyyy");
                if (report.Save((DateTime)startDate, (DateTime)endDate))
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

        private void createWriteoffReport(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                WriteoffReport report = new WriteoffReport();
                report.Postfix = "сформирован вручную";
                if (startDate == null)
                {
                    int currentDay = DateTime.Now.Day;
                    startDate = DateTime.Now.AddDays(-(currentDay - 1));
                }
                if (endDate == null)
                {
                    endDate = DateTime.Now;
                }

                report.H1 = "Журнал списания с " + ((DateTime)startDate).ToString("dd.MM.yyyy") +
                    " по " + ((DateTime)endDate).ToString("dd.MM.yyyy") + ", сформирован вручную " + DateTime.Now.ToString("dd.MM.yyyy");
                if (report.Save((DateTime)startDate, (DateTime)endDate))
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
