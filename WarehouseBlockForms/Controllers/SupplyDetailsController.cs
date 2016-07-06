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
    class SupplyDetailsController : Controller, IController
    {

        private static SupplyDetailsController _instance = null;
        private ObservableCollection<SupplyDetails> _collection;
        public ObservableCollection<SupplyDetails> Collection
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

        public static SupplyDetailsController instance()
        {
            if (_instance == null)
            {
                _instance = new SupplyDetailsController();
            }
            return _instance;
        }

        private SupplyDetailsController()
        {
            _collection = new ObservableCollection<SupplyDetails>();
            load();
        }

        protected override string LoadQuery
        {
            get
            {
                return "select id, details_count, id_details, id_supply from supply_details";
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
                SupplyDetails supplyDetails = model as SupplyDetails;
                _collection.Add(supplyDetails);

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
                SupplyDetails supplyDetails = model as SupplyDetails;
                if(_collection.Contains(supplyDetails))
                {
                    _collection.Remove(supplyDetails);
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
            SupplyDetails supplyDetails = new SupplyDetails();
            supplyDetails.Id = reader.GetInt32(0);
            supplyDetails.DetailsCount = reader.GetInt32(1);
            supplyDetails.IdDetails = reader.GetInt32(2);
            supplyDetails.IdSupply = reader.GetInt32(3);
            add(supplyDetails);
        }

        public List<SupplyDetails> getByIdDetail (int id_detail)
        {
            return _collection.Where(x => x.IdDetails == id_detail).ToList();
        }

        public int allSupply (int id_detail)
        {
            return getByIdDetail(id_detail).Sum(x => x.DetailsCount);
        }

    }
}
