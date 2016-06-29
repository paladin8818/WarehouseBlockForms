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
    class SupplyController : Controller, IController
    {

        private static SupplyController _instance = null;
        private ObservableCollection<Supply> _collection;
        public ObservableCollection<Supply> Collection
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

        public static SupplyController instance()
        {
            if (_instance == null)
            {
                _instance = new SupplyController();
            }
            return _instance;
        }

        private SupplyController()
        {
            _collection = new ObservableCollection<Supply>();
            load();
        }

        protected override string LoadQuery
        {
            get
            {
                return "select id, supply_date from supply";
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
                Supply supply = model as Supply;
                _collection.Add(supply);
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
                Supply supply = model as Supply;
                if(_collection.Contains(supply))
                {
                    _collection.Remove(supply);
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
            Supply supply = new Supply();
            supply.Id = reader.GetInt32(0);
            supply.SupplyDate = reader.GetDateTime(1);
            add(supply);
        }
    }
}
