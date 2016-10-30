using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Classess;
using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Controllers
{
    class WriteoffDetailsController : Controller, IController
    {

        private static WriteoffDetailsController _instance = null;
        private ObservableCollection<WriteoffDetails> _collection;
        public ObservableCollection<WriteoffDetails> Collection
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

        public static WriteoffDetailsController instance()
        {
            if (_instance == null)
            {
                _instance = new WriteoffDetailsController();
            }
            return _instance;
        }

        private WriteoffDetailsController()
        {
            _collection = new ObservableCollection<WriteoffDetails>();
            load();
        }

        protected override string LoadQuery
        {
            get
            {
                return "select id, details_count, id_details, id_writeoff from writeoff_details";
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
                WriteoffDetails writeoffDetails = model as WriteoffDetails;
                _collection.Add(writeoffDetails);

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
                WriteoffDetails writeoffDetails = model as WriteoffDetails;
                if(_collection.Contains(writeoffDetails))
                {
                    _collection.Remove(writeoffDetails);
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
            WriteoffDetails writeoffDetails = new WriteoffDetails();
            writeoffDetails.Id = reader.GetInt32(0);
            writeoffDetails.DetailsCount = reader.GetDouble(1);
            writeoffDetails.IdDetails = reader.GetInt32(2);
            writeoffDetails.IdWriteoff = reader.GetInt32(3);
            add(writeoffDetails);
        }

        public List<WriteoffDetails> getByIdDetail(int id_detail)
        {
            return _collection.Where(x => x.IdDetails == id_detail).ToList();
        }

        public List<WriteoffDetails> getByIdWriteoff(int id_writeoff)
        {
            return _collection.Where(x => x.IdWriteoff == id_writeoff).ToList();
        }

        public double allWriteoff(int id_detail)
        {
            return getByIdDetail(id_detail).Sum(x => x.DetailsCount);
        }


        public double getCountByIdDetailAndWriteoffList(int id_detail, List<Writeoff> writeoffList)
        {
            double detailsCount = 0;
            for (int i = 0; i < writeoffList.Count; i++)
            {
                detailsCount += _collection.Where(x => (x.IdWriteoff == writeoffList[i].Id) && (x.IdDetails == id_detail)).Sum(x => x.DetailsCount);
            }
            return detailsCount;
        }

    }
}
