using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
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
            load();
        }

        protected override string LoadQuery
        {
            get
            {
                return "select id, name, vendor_code, id_oven from details";
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
            add(details);
        }
    }
}
