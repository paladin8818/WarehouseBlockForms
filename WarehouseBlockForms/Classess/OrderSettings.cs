using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WarehouseBlockForms.Classess
{
    public static class OrderSettings
    {
        public static string OvenSortColumn = "RowOrder";
        public static ListSortDirection OvenSortDirection = ListSortDirection.Ascending;

        public static string DetailsSortColumn = "RowOrder";
        public static ListSortDirection DetailsSortDirection = ListSortDirection.Ascending;

        public static string RecipientsSortColumn = "RowOrder";
        public static ListSortDirection RecipientsSortDirection = ListSortDirection.Ascending;
    }
}
