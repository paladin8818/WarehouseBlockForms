using System;
using System.Collections.Generic;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;
using WarehouseBlockForms.Reports.Base;

namespace WarehouseBlockForms.Reports
{
    class AvailabilityReport : ExcelReport
    {

        public override string Path
        {
            get
            {
                string path = ReportsSettingController.instance().getPathByProgramName("AvailabilityReport");
                if(path != "")
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
                return "Отчет о наличии " + DateTime.Now.ToString("dd.MM.yyyy HH_mm") + " (" + Postfix + ")" ;
            }
        }


        public override bool Save()
        {
            ColumnCount = 5;

            columnsWidth.Add(1, 2.57);
            columnsWidth.Add(2, 8.29);
            columnsWidth.Add(3, 8.29);
            columnsWidth.Add(4, 53);
            columnsWidth.Add(5, 11.57);

            HeaderRow = new string[] { "№", "Печь", "Артикул", "Наименование", "Количество" };

            List<Details> details = DetailsController.instance().getSortedByRowOrder();
            List<ReportRow> reportData = new List<ReportRow>();
            for (int i = 0; i < details.Count; i++)
            {
                ReportRow reportRow = new ReportRow();
                Details detail = details[i];
                reportRow.Row.Add(detail.RowIndex.ToString());
                reportRow.Row.Add(detail.OvenName);
                reportRow.Row.Add("");
                reportRow.Row.Add(detail.Name);
                reportRow.Row.Add(detail.CurrentCount.ToString());
                reportData.Add(reportRow);

                reportRow.Style.Add(ReportRow.RowStyle.Border);
            }
            Data = reportData;
            return Create();
        }

        public override bool Save(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
