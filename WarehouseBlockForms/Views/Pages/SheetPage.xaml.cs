using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Helpers;
using WarehouseBlockForms.Models;
using WarehouseBlockForms.Reports;
using WarehouseBlockForms.Reports.Base;

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
                createTurnReport();
            };

            btnCreateSupplyReport.Click += delegate
            {
                createSupplyReport();
            };

            btnCreateWriteoffReport.Click += delegate
            {
                createWriteoffReport();
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

                report.H1 = report.ReportName;
                report.HeaderRow = new string[] { "№", "Печь", "Артикул", "Наименование", "Поступление", "Списание", "Остаток" };

                List<Details> details = DetailsController.instance().getSortedByRowOrder();

                if (startDate == null)
                {
                    int currentDay = DateTime.Now.Day;
                    startDate = DateTime.Now.AddDays(-(currentDay - 1));
                }
                if (endDate == null)
                {
                    endDate = DateTime.Now;
                }
                List<Supply> supplysForPeriod = SupplyController.instance().getByPeriod((DateTime)startDate, (DateTime)endDate);
                List<Writeoff> writeoffsForPeriod = WriteoffController.instance().getByPeriod((DateTime)startDate, (DateTime)endDate);

                List<ReportRow> reportData = new List<ReportRow>();
                for (int i = 0; i < details.Count; i++)
                {
                    Details detail = details[i];
                    ReportRow reportRow = new ReportRow();

                    reportRow.Row.Add(detail.RowIndex.ToString());
                    reportRow.Row.Add(detail.OvenName);
                    reportRow.Row.Add(detail.VendorCode);
                    reportRow.Row.Add(detail.Name);
                    reportRow.Row.Add(getSupplyForPeriod(detail, supplysForPeriod).ToString());
                    reportRow.Row.Add(getWriteoffForPeriod(detail, writeoffsForPeriod).ToString());
                    reportRow.Row.Add(detail.CurrentCount.ToString());

                    reportData.Add(reportRow);
                }
                report.Data = reportData;

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

                List<Supply> supplysForPeriod = SupplyController.instance().getByPeriod((DateTime)startDate, (DateTime)endDate);

                List<ReportRow> reportData = new List<ReportRow>();

                foreach (Supply supply in supplysForPeriod)
                {

                    ReportRow emptyRow = new ReportRow();
                    emptyRow.Row.Add("");
                    emptyRow.Row.Add("");
                    emptyRow.Row.Add("");
                    emptyRow.Row.Add("");
                    emptyRow.Row.Add("");
                    reportData.Add(emptyRow);

                    ReportRow reportRow = new ReportRow();

                    reportRow.Row.Add("№ накладной");
                    reportRow.Row.Add(supply.Id.ToString());
                    reportRow.Row.Add("Дата");
                    reportRow.Row.Add(supply.SupplyDate.ToString("dd.MM.yyyy"));

                    reportData.Add(reportRow);
                    reportRow.Style.Add(ReportRow.RowStyle.Bold);
                    reportRow.Style.Add(ReportRow.RowStyle.TextAlignCenter);
                    reportRow.Style.Add(ReportRow.RowStyle.Selection);

                    ReportRow reportRowHead = new ReportRow();

                    reportRowHead.Row.Add("№");
                    reportRowHead.Row.Add("Печь");
                    reportRowHead.Row.Add("Артикул");
                    reportRowHead.Row.Add("Наименование");
                    reportRowHead.Row.Add("Количество");

                    reportData.Add(reportRowHead);
                    reportRowHead.Style.Add(ReportRow.RowStyle.Bold);
                    reportRowHead.Style.Add(ReportRow.RowStyle.TextAlignCenter);

                    List<SupplyDetails> supplyDetails = SupplyDetailsController.instance().getByIdSupply(supply.Id);
                    for (int i = 0; i < supplyDetails.Count; i++)
                    {
                        Details currentDetail = DetailsController.instance().getById(supplyDetails[i].IdDetails);
                        if (currentDetail == null) continue;

                        ReportRow detailRow = new ReportRow();

                        detailRow.Row.Add((i+1).ToString());
                        detailRow.Row.Add(currentDetail.OvenName);
                        detailRow.Row.Add(currentDetail.VendorCode);
                        detailRow.Row.Add(currentDetail.Name);
                        detailRow.Row.Add(supplyDetails[i].DetailsCount.ToString());

                        reportData.Add(detailRow);

                    }
                }

                report.Data = reportData;

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

        private void createWriteoffReport(DateTime? startDate = null, DateTime? endDate = null)
        {

        }

        private int getSupplyForPeriod (Details detail, List<Supply> supplys)
        {
            return SupplyDetailsController.instance().getCountDetailByIdDetailAndSupplyList(detail.Id, supplys);
        }

        private int getWriteoffForPeriod (Details detail, List<Writeoff> writeoffs)
        {
            return WriteoffDetailsController.instance().getCountByIdDetailAndWriteoffList(detail.Id, writeoffs);
        }

    }
}
