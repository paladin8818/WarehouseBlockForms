using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Data;
using WarehouseBlockForms.Classess;
using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Controllers
{
    class DetailsController : Controller, IController
    {

        private static DetailsController _instance = null;
        private ObservableCollection<Details> _collection;
        public ObservableCollection<Details> Collection
        {
            get
            {
                return _collection;
            }
            private set
            {
                _collection = value;
            }
        }
        public CollectionViewSource ViewSource { get; set; }

        public static DetailsController instance ()
        {
            if(_instance == null)
            {
                _instance = new DetailsController();
            }
            return _instance;
        }

        private DetailsController ()
        {
            _collection = new ObservableCollection<Details>();

            ViewSource = new CollectionViewSource();
            ViewSource.Source = _collection;
            ViewSource.Filter += ViewSource_Filter;
            load();
        }

        private void ViewSource_Filter(object sender, FilterEventArgs e)
        {
            Details detail = e.Item as Details;
            if (detail == null) return;
            e.Accepted = detail.IsVisible;
        }

        protected override string LoadQuery
        {
            get
            {
                if (OrderSettings.DetailsSortDirection == System.ComponentModel.ListSortDirection.Ascending)
                    return "select id, name, vendor_code, id_oven, row_order from details order by " + Details.ClassDBFields[OrderSettings.DetailsSortColumn] + " asc";
                else
                    return "select id, name, vendor_code, id_oven, row_order from details order by " + Details.ClassDBFields[OrderSettings.DetailsSortColumn] + " desc";
            }
        }

        protected override Dictionary<string, object> Parameters
        {
            get
            {
                return new Dictionary<string, object>();
            }
        }

        public bool add<T>(T model)
        {
            try
            {
                Details details = model as Details;
                _collection.Add(details);
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message);
                return false;
            }
        }

        public bool remove<T>(T model)
        {
            try
            {
                Details details = model as Details;
                if(_collection.Contains(details))
                {
                    _collection.Remove(details);
                }
                return true;
            }
            catch(Exception ex)
            {
                Log.WriteError(ex.Message);
                return false;
            }
        }

        public Details getById (int id)
        {
            return _collection.Where(x => x.Id == id).FirstOrDefault();
        }

        protected override void populate(SQLiteDataReader reader)
        {
            Details details = new Details();
            details.Id = reader.GetInt32(0);
            details.Name = reader.GetString(1);
            details.VendorCode = reader.GetString(2);
            details.IdOven = reader.GetInt32(3);
            details.RowOrder = reader.GetInt32(4);
            add(details);
        }

        public int getPos(Details detail)
        {
            List<Details> details = getSortedByOrderSetting();
            if (details.Contains(detail))
            {
                return details.IndexOf(detail) + 1;
            }
            return -1;
        }

        public List<Details> getSortedByOrderSetting ()
        {
            List<Details> details = _collection.ToList();
            switch(OrderSettings.DetailsSortColumn)
            {
                case "Id": details.Sort(DetailsIdCompare); break;
                case "Name": details.Sort(DetailsNameCompare); break;
                case "VendorCode": details.Sort(DetailsVendorCodeCompare); break;
                case "IdOven": details.Sort(DetailsIdOvenCompare); break;
                case "RowOrder": details.Sort(DetailsRowOrderCompare); break;
            }
            return details;
        }

        public List<Details> GetSortedList (List<Details> details)
        {
            switch (OrderSettings.DetailsSortColumn)
            {
                case "Id": details.Sort(DetailsIdCompare); break;
                case "Name": details.Sort(DetailsNameCompare); break;
                case "VendorCode": details.Sort(DetailsVendorCodeCompare); break;
                case "IdOven": details.Sort(DetailsIdOvenCompare); break;
                case "RowOrder": details.Sort(DetailsRowOrderCompare); break;
            }
            return details;
        }

        private static int DetailsIdCompare(Details x, Details y)
        {
            if (OrderSettings.DetailsSortDirection == System.ComponentModel.ListSortDirection.Descending)
                return y.Id.CompareTo(x.Id);
            else
                return x.Id.CompareTo(y.Id);
        }

        private static int DetailsNameCompare(Details x, Details y)
        {
            if (OrderSettings.DetailsSortDirection == System.ComponentModel.ListSortDirection.Descending)
                return y.Name.CompareTo(x.Name);
            else
                return x.Name.CompareTo(y.Name);
        }

        private static int DetailsVendorCodeCompare(Details x, Details y)
        {
            if (OrderSettings.DetailsSortDirection == System.ComponentModel.ListSortDirection.Descending)
                return y.VendorCode.CompareTo(x.VendorCode);
            else
                return x.VendorCode.CompareTo(y.VendorCode);
        }

        private static int DetailsIdOvenCompare(Details x, Details y)
        {
            if (OrderSettings.DetailsSortDirection == System.ComponentModel.ListSortDirection.Descending)
                return y.IdOven.CompareTo(x.IdOven);
            else
                return x.IdOven.CompareTo(y.IdOven);
        }

        private static int DetailsRowOrderCompare (Details x, Details y)
        {
            if (OrderSettings.DetailsSortDirection == System.ComponentModel.ListSortDirection.Descending)
                return y.RowOrder.CompareTo(x.RowOrder);
            else
                return x.RowOrder.CompareTo(y.RowOrder);
        }



        public int maxRowOrderIndex()
        {
            Details detail = _collection.OrderByDescending(x => x.RowOrder).FirstOrDefault();
            if (detail == null)
            {
                return 0;
            }
            return detail.RowOrder;
        }

        public int prevRowOrderIndex(int currentIndex)
        {
            Details detail = _collection.Where(x => x.RowOrder < currentIndex).OrderByDescending(y => y.RowOrder).FirstOrDefault();
            if (detail == null)
            {
                return 0;
            }
            return detail.RowOrder;
        }

        public int nextRowOrderIndex(int currentIndex)
        {
            Details detail = _collection.Where(x => x.RowOrder > currentIndex).OrderBy(y => y.RowOrder).FirstOrDefault();
            if (detail == null)
            {
                return 0;
            }
            return detail.RowOrder;
        }

        public Details getByOrderIndex(int orderIndex)
        {
            return _collection.Where(x => x.RowOrder == orderIndex).FirstOrDefault();
        }

        public void showFromOvenId(int id_oven)
        {
            _collection.ToList().ForEach(x => x.setFilter("IdOven", true));
            if(id_oven != 0)
            {
                _collection.Where(x => x.IdOven != id_oven).ToList().ForEach(y => y.setFilter("IdOven", false));
            }
            ViewSource.View.Refresh();
        }

        public void showFromText(string text)
        {
            string[] properties = new string[] {"Name", "VendorCode"};
            _collection.ToList().ForEach(x => x.setFilters(properties, false));
            text = text.ToLower();
            _collection.Where(x => (x.Name.ToLower().IndexOf(text) != -1) | (x.VendorCode.ToLower().IndexOf(text) != -1) )
                .ToList().ForEach(y => y.setFilters(properties, true));

            ViewSource.View.Refresh();
        }

    }
}
