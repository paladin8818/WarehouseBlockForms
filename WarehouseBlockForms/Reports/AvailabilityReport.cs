using System;
using WarehouseBlockForms.Reports.Base;

namespace WarehouseBlockForms.Reports
{
    class AvailabilityReport : ExcelReport
    {
        public override string Path
        {
            get
            {
                return "d:";
            }

        }

        public override string ReportName
        {
            get
            {
                return "Отчет о наличии " + DateTime.Now.ToString("dd.MM.yyyy") + " (" + Postfix + ")" ;
            }
        }


        public override bool Save()
        {
            return Create();
        }


    }
}
