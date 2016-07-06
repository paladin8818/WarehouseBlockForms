using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Data;
using WarehouseBlockForms.Classes;
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
                return "select id, name, vendor_code, id_oven, row_order from details order by name asc";
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
                Log.WriteError("Класс DetailsController, строка 65, " + ex.Message);
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
            List<Details> details = _collection.ToList();
            
            details.Sort(DetailNameCompare);
            if (_collection.Contains(detail))
            {
                return _collection.IndexOf(detail) + 1;
            }
            return -1;
        }

        private static int DetailNameCompare (Details x, Details y)
        {
            return x.Name.CompareTo(y.Name);
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

        private void hide (Details detail)
        {
            detail.IsVisible = false;
        }

        private void show (Details detail)
        {
            detail.IsVisible = true;
        }

        public void showFromOvenId(int id_oven)
        {
            _collection.ToList().ForEach(x => show(x));
            if(id_oven != 0)
            {
                _collection.Where(x => x.IdOven != id_oven).ToList().ForEach(y => hide(y));
            }
            ViewSource.View.Refresh();
        }

        public void showFromText(string text)
        {
            
        }

    }
}
