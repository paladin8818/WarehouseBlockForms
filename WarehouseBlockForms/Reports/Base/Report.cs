using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarehouseBlockForms.Reports.Base
{
    public abstract class Report
    {
        public abstract string Path { get; }
        public abstract string ReportName { get; }
        public string Postfix { get; set; }


        

        public string H1 { get; set; }
        public string H2 { get; set; }
        public string H3 { get; set; }

        public string[] HeaderRow { get; set; }
        public string[][] Data { get; set; }

        public string F1 { get; set; }
        public string F2 { get; set; }
        public string F3 { get; set; } 

        public abstract bool Save();
    }
}
