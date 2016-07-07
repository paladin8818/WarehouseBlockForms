using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Windows.Data;
using WarehouseBlockForms.Classes;
using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Controllers
{
    class RecipientsController : Controller, IController
    {
        private static RecipientsController _instance = null;
        private ObservableCollection<Recipients> _collection;
        public ObservableCollection<Recipients> Collection
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

        public static RecipientsController instance()
        {
            if (_instance == null)
            {
                _instance = new RecipientsController();
            }
            return _instance;
        }

        private RecipientsController()
        {
            _collection = new ObservableCollection<Recipients>();
            ViewSource = new CollectionViewSource();
            ViewSource.Source = _collection;

            load();
        }

        protected override string LoadQuery
        {
            get
            {
                return "select id, full_name, row_order from recipients";
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
                Recipients recipient = model as Recipients;
                _collection.Add(recipient);
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
                Recipients recipient = model as Recipients;
                if(_collection.Contains(recipient))
                {
                    _collection.Remove(recipient);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message);
                return false;
            }
        }

        protected override void populate(SQLiteDataReader reader)
        {
            Recipients recipient = new Recipients();
            recipient.Id = reader.GetInt32(0);
            recipient.FullName = reader.GetString(1);
            recipient.RowOrder = reader.GetInt32(2);
            add(recipient);
        }

        public Recipients getById(int id)
        {
            return _collection.Where(x => x.Id == id).FirstOrDefault();
        }

        public int maxRowOrderIndex()
        {
            Recipients recipient = _collection.OrderByDescending(x => x.RowOrder).FirstOrDefault();
            if (recipient == null)
            {
                return 0;
            }
            return recipient.RowOrder;
        }

        public int prevRowOrderIndex(int currentIndex)
        {
            Recipients recipient = _collection.Where(x => x.RowOrder < currentIndex).OrderByDescending(y => y.RowOrder).FirstOrDefault();
            if (recipient == null)
            {
                return 0;
            }
            return recipient.RowOrder;
        }

        public int nextRowOrderIndex(int currentIndex)
        {
            Recipients recipient = _collection.Where(x => x.RowOrder > currentIndex).OrderBy(y => y.RowOrder).FirstOrDefault();
            if (recipient == null)
            {
                return 0;
            }
            return recipient.RowOrder;
        }

        public Recipients getByOrderIndex(int orderIndex)
        {
            return _collection.Where(x => x.RowOrder == orderIndex).FirstOrDefault();
        }

    }
}
