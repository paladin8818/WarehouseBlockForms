using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;
using WarehouseBlockForms.Reports.Base;

namespace WarehouseBlockForms.Reports
{
    class SupplyReport : ExcelReport
    {
        public override string Path
        {
            get
            {
                string path = ReportsSettingController.instance().getPathByProgramName("SupplyReport");
                if (path != "")
                {
                    return path;
                }
                return DefaultPath;
            }
        }

        public override string ReportName
        {
            get
            {
                return "Журнал поступления " + DateTime.Now.ToString("dd.MM.yyyy HH_mm") + " (" + Postfix + ")";
            }
        }

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Save(DateTime startDate, DateTime endDate)
        {

            ColumnCount = 5;

            columnsWidth.Add(1, 13.14);
            columnsWidth.Add(2, 8.29);
            columnsWidth.Add(3, 8.29);
            columnsWidth.Add(4, 42.57);
            columnsWidth.Add(5, 10.86);

            List<Supply> supplysForPeriod = SupplyController.instance().getByPeriod(startDate, endDate);
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
                reportRowHead.Style.Add(ReportRow.RowStyle.Border);

                List<SupplyDetails> supplyDetails = SupplyDetailsController.instance().getByIdSupply(supply.Id);
                for (int i = 0; i < supplyDetails.Count; i++)
                {
                    Details currentDetail = DetailsController.instance().getById(supplyDetails[i].IdDetails);
                    if (currentDetail == null) continue;

                    ReportRow detailRow = new ReportRow();

                    detailRow.Row.Add((i + 1).ToString());
                    detailRow.Row.Add(currentDetail.OvenName);
                    detailRow.Row.Add(currentDetail.VendorCode);
                    detailRow.Row.Add(currentDetail.Name);
                    detailRow.Row.Add(supplyDetails[i].DetailsCount.ToString());

                    detailRow.Style.Add(ReportRow.RowStyle.Border);

                    reportData.Add(detailRow);

                }
            }

            Data = reportData;
            return Create();
        }
    }
}
