using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Classess;
using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Helpers;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Controllers
{
    class WriteoffController : Controller, IController
    {

        private static WriteoffController _instance = null;
        private ObservableCollection<Writeoff> _collection;
        public ObservableCollection<Writeoff> Collection
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

        public static WriteoffController instance()
        {
            if (_instance == null)
            {
                _instance = new WriteoffController();
            }
            return _instance;
        }

        private WriteoffController()
        {
            _collection = new ObservableCollection<Writeoff>();
            load();
        }

        protected override string LoadQuery
        {
            get
            {
                return "select id, writeoff_date, id_recipient from writeoff";
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
                Writeoff writeoff = model as Writeoff;
                _collection.Add(writeoff);
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
                Writeoff writeoff = model as Writeoff;
                if(_collection.Contains(writeoff))
                {
                    _collection.Remove(writeoff);
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
            Writeoff writeoff = new Writeoff();
            writeoff.Id = reader.GetInt32(0);
            writeoff.WriteoffDate = reader.GetDateTime(1);
            writeoff.IdRecipient = reader.GetInt32(2);
            add(writeoff);
        }

        public List<Writeoff> getByPeriod(DateTime startDate, DateTime endDate)
        {
            return _collection.Where(x => (x.WriteoffDate >= startDate) && (x.WriteoffDate <= endDate)).ToList();
        }

    }
}
