using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;
using WarehouseBlockForms.Reports.Base;

namespace WarehouseBlockForms.Reports
{
    class TurnReport : ExcelReport
    {
        public override string Path
        {
            get
            {
                string path = ReportsSettingController.instance().getPathByProgramName("TurnReport");
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
                return "Отчет оборота " + DateTime.Now.ToString("dd.MM.yyyy HH_mm") + " (" + Postfix + ")";
            }
        }

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Save(DateTime startDate, DateTime endDate)
        {

            ColumnCount = 7;
            columnsWidth.Add(1, 2.57);
            columnsWidth.Add(2, 8.14);
            columnsWidth.Add(3, 8.29);
            columnsWidth.Add(4, 34.29);
            columnsWidth.Add(5, 11.71);
            columnsWidth.Add(6, 9);
            columnsWidth.Add(7, 7.43);



            HeaderRow = new string[] { "№", "Печь", "Артикул", "Наименование", "Поступление", "Списание", "Остаток" };

            List<Details> details = DetailsController.instance().getSortedByOrderSetting();

            if (startDate == null)
            {
                int currentDay = DateTime.Now.Day;
                startDate = DateTime.Now.AddDays(-(currentDay - 1));
            }
            if (endDate == null)
            {
                endDate = DateTime.Now;
            }
            List<Supply> supplysForPeriod = SupplyController.instance().getByPeriod(startDate, endDate);
            List<Writeoff> writeoffsForPeriod = WriteoffController.instance().getByPeriod(startDate, endDate);

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

                reportRow.Style.Add(ReportRow.RowStyle.Border);

                reportData.Add(reportRow);
            }
            Data = reportData;
            return Create();
        }

        private int getSupplyForPeriod(Details detail, List<Supply> supplys)
        {
            return SupplyDetailsController.instance().getCountDetailByIdDetailAndSupplyList(detail.Id, supplys);
        }

        private int getWriteoffForPeriod(Details detail, List<Writeoff> writeoffs)
        {
            return WriteoffDetailsController.instance().getCountByIdDetailAndWriteoffList(detail.Id, writeoffs);
        }

    }
}