using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Controllers
{
    class ReportsCreatedController : Controller, IController
    {

        private static ReportsCreatedController _instance = null;
        private ObservableCollection<ReportsCreated> _collection;
        public ObservableCollection<ReportsCreated> Collection
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

        public static ReportsCreatedController instance()
        {
            if (_instance == null)
            {
                _instance = new ReportsCreatedController();
            }
            return _instance;
        }

        private ReportsCreatedController()
        {
            _collection = new ObservableCollection<ReportsCreated>();
            load();
        }

        protected override string LoadQuery
        {
            get
            {
                return "";
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
            return true;
        }

        public bool remove<T>(T model)
        {
            return true;
        }

        protected override void populate(SQLiteDataReader reader)
        {
        }
    }
}
