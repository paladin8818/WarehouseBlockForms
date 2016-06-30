using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WarehouseBlockForms.Helpers
{
    class WriteoffDetailsHelperCollection
    {
        private static WriteoffDetailsHelperCollection _instance = null;
        private ObservableCollection<WriteoffDetailsHelper> _collection = new ObservableCollection<WriteoffDetailsHelper>();
        public ObservableCollection<WriteoffDetailsHelper> Collection
        {
            get
            {
                return _collection;
            }
        }

        private WriteoffDetailsHelperCollection() { }

        public static WriteoffDetailsHelperCollection instance()
        {
            if (_instance == null)
            {
                _instance = new WriteoffDetailsHelperCollection();
            }
            return _instance;
        }

        public void add(WriteoffDetailsHelper wod_helper)
        {
            _collection.Add(wod_helper);
        }
        public void remove(WriteoffDetailsHelper wod_helper)
        {
            if (_collection.Contains(wod_helper))
            {
                _collection.Remove(wod_helper);
                _collection.All(x => { x.RowIndex = 1; return true; });
            }
        }
        public void clear()
        {
            _collection.Clear();
        }
        public int getPos(WriteoffDetailsHelper wod_helper)
        {
            if (_collection.Contains(wod_helper))
            {
                return _collection.IndexOf(wod_helper) + 1;
            }
            return -1;
        }

        public bool isAllCorrect()
        {
            int incorrectcount = _collection.Where(x => (x.IdDetails == 0) || (x.DetailsCount == 0)).ToList().Count;
            if (incorrectcount > 0)
            {
                return false;
            }
            return true;
        }
    }
}
