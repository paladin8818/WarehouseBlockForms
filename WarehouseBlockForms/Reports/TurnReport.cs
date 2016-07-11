using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Reports.Base;

namespace WarehouseBlockForms.Reports
{
    class TurnReport : ExcelReport
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
                return "Отчет оборота " + DateTime.Now.ToString("dd.MM.yyyy HH_mm") + " (" + Postfix + ")";
            }
        }

        public override bool Save()
        {
            return Create();
        }
    }
}