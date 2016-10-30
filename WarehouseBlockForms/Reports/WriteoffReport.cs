using System;
using System.Collections.Generic;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;
using WarehouseBlockForms.Reports.Base;

namespace WarehouseBlockForms.Reports
{
    class WriteoffReport : ExcelReport
    {
        public override string Path
        {
            get
            {
                string path = ReportsSettingController.instance().getPathByProgramName("WriteoffReport");
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
                return "Журнал списания " + DateTime.Now.ToString("dd.MM.yyyy HH_mm") + " (" + Postfix + ")";
            }
        }

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Save(DateTime startDate, DateTime endDate)
        {

            setLandscapeOrientation();

            ColumnCount = 9;

            columnsWidth.Add(1, 14);
            columnsWidth.Add(2, 9.29);
            columnsWidth.Add(3, 11.29);
            columnsWidth.Add(4, 14.29);
            columnsWidth.Add(5, 10.86);

            columnsWidth.Add(6, 5.29);
            columnsWidth.Add(7, 11);
            columnsWidth.Add(8, 29.86);
            columnsWidth.Add(9, 16.86);

            List<Writeoff> writeoffForPeriod = WriteoffController.instance().getByPeriod(startDate, endDate);
            List<ReportRow> reportData = new List<ReportRow>();

            int currentRowIndex = 3;

            merge(1, 2, 1, 9);

            foreach (Writeoff writeoff in writeoffForPeriod)
            {
                ReportRow emptyRow = new ReportRow();
                emptyRow.Row.Add("");
                emptyRow.Row.Add("");
                emptyRow.Row.Add("");
                emptyRow.Row.Add("");
                emptyRow.Row.Add("");
                reportData.Add(emptyRow);
                currentRowIndex++;

                ReportRow reportRow = new ReportRow();

                reportRow.Row.Add("№ накладной");
                reportRow.Row.Add(writeoff.Id.ToString());
                reportRow.Row.Add("Дата");
                reportRow.Row.Add(writeoff.WriteoffDate.ToString("dd.MM.yyyy"));
                reportRow.Row.Add("");
                reportRow.Row.Add("");
                reportRow.Row.Add("Получатель");
                reportRow.Row.Add(RecipientsController.instance().getById(writeoff.IdRecipient).FullName);

                selection(currentRowIndex, currentRowIndex, 1, 9);

                reportData.Add(reportRow);
                currentRowIndex++;

                reportRow.Style.Add(ReportRow.RowStyle.Bold);
                reportRow.Style.Add(ReportRow.RowStyle.TextAlignCenter);


                ReportRow reportRowHead = new ReportRow();

                reportRowHead.Row.Add("№");
                reportRowHead.Row.Add("Производитель");
                reportRowHead.Row.Add("");
                reportRowHead.Row.Add("Наименование");
                reportRowHead.Row.Add("");
                reportRowHead.Row.Add("");
                reportRowHead.Row.Add("");
                reportRowHead.Row.Add("");
                reportRowHead.Row.Add("Количество");

                merge(currentRowIndex, currentRowIndex, 2, 3);
                merge(currentRowIndex, currentRowIndex, 4, 8);
                border(currentRowIndex, currentRowIndex, 1, 9);

                reportRowHead.Style.Add(ReportRow.RowStyle.Bold);
                reportRowHead.Style.Add(ReportRow.RowStyle.TextAlignCenter);

                reportData.Add(reportRowHead);
                currentRowIndex++;

                List<WriteoffDetails> writeoffDetails = WriteoffDetailsController.instance().getByIdWriteoff(writeoff.Id);
                for (int i = 0; i < writeoffDetails.Count; i++)
                {
                    Details currentDetail = DetailsController.instance().getById(writeoffDetails[i].IdDetails);
                    if (currentDetail == null) continue;

                    ReportRow detailRow = new ReportRow();

                    detailRow.Row.Add((i + 1).ToString());

                    detailRow.Row.Add(currentDetail.OvenName);
                    detailRow.Row.Add("");
                    merge(currentRowIndex, currentRowIndex, 2, 3);

                    detailRow.Row.Add(currentDetail.Name);
                    detailRow.Row.Add("");
                    detailRow.Row.Add("");
                    detailRow.Row.Add("");
                    detailRow.Row.Add("");
                    merge(currentRowIndex, currentRowIndex, 4, 8);

                    detailRow.Row.Add(writeoffDetails[i].DetailsCount.ToString());


                    border(currentRowIndex, currentRowIndex, 1, 9);

                    reportData.Add(detailRow);
                    currentRowIndex++;
                }
            }

            Data = reportData;
            return Create();
        }
    }
}
