using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            List<Writeoff> writeoffForPeriod = WriteoffController.instance().getByPeriod(startDate, endDate);
            List<ReportRow> reportData = new List<ReportRow>();
            foreach (Writeoff writeoff in writeoffForPeriod)
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
                reportRow.Row.Add(writeoff.Id.ToString());
                reportRow.Row.Add("Дата");
                reportRow.Row.Add(writeoff.WriteoffDate.ToString("dd.MM.yyyy"));
                reportRow.Row.Add("№ заявки");
                reportRow.Row.Add(writeoff.AppNumber);
                reportRow.Row.Add("Получатель");
                reportRow.Row.Add(RecipientsController.instance().getById(writeoff.IdRecipient).FullName);

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

                List<WriteoffDetails> writeoffDetails = WriteoffDetailsController.instance().getByIdWriteoff(writeoff.Id);
                for (int i = 0; i < writeoffDetails.Count; i++)
                {
                    Details currentDetail = DetailsController.instance().getById(writeoffDetails[i].IdDetails);
                    if (currentDetail == null) continue;

                    ReportRow detailRow = new ReportRow();

                    detailRow.Row.Add((i + 1).ToString());
                    detailRow.Row.Add(currentDetail.OvenName);
                    detailRow.Row.Add(currentDetail.VendorCode);
                    detailRow.Row.Add(currentDetail.Name);
                    detailRow.Row.Add(writeoffDetails[i].DetailsCount.ToString());

                    reportData.Add(detailRow);

                }
            }

            Data = reportData;
            return Create();
        }
    }
}
