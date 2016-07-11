using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarehouseBlockForms.Reports.Base
{
    public class ReportRow
    {
        public enum RowStyle
        {
            Bold,
            Italic,
            TextAlignLeft,
            TextAlignRight,
            TextAlignCenter,
            Selection
        }
        public List<string> Row = new List<string>();
        public List<RowStyle> Style = new List<RowStyle>();
    }
}
