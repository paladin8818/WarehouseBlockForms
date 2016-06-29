using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
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
            load();
        }

        protected override string LoadQuery
        {
            get
            {
                return "select id, full_name from recipients";
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
            add(recipient);
        }
    }
}
