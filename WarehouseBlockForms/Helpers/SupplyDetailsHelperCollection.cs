using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WarehouseBlockForms.Helpers
{
    class SupplyDetailsHelperCollection
    {
        private static SupplyDetailsHelperCollection _instance = null;
        private ObservableCollection<SupplyDetailsHelper> _collection = new ObservableCollection<SupplyDetailsHelper>();
        public ObservableCollection<SupplyDetailsHelper> Collection
        {
            get
            {
                return _collection;
            }
        }

        private SupplyDetailsHelperCollection () { }

        public static SupplyDetailsHelperCollection instance ()
        {
            if(_instance == null)
            {
                _instance = new SupplyDetailsHelperCollection();
            }
            return _instance;
        }

        public void add (SupplyDetailsHelper sd_helper)
        {
            _collection.Add(sd_helper);
            updateLastRow();
        }
        public void remove(SupplyDetailsHelper sd_helper)
        {
            if(_collection.Contains(sd_helper))
            {
                _collection.Remove(sd_helper);
                _collection.All(x => { x.RowIndex = 1; return true; });
                updateLastRow();
            }
        }
        public void clear ()
        {
            _collection.Clear();
        }
        public int getPos (SupplyDetailsHelper sd_helper)
        {
            if(_collection.Contains(sd_helper))
            {
                return _collection.IndexOf(sd_helper) + 1;
            }
            return -1;
        }

        public bool isAllCorrect ()
        {
            int incorrectcount = _collection.Where(x => (x.IdDetails == 0) || (x.DetailsCount == 0)).ToList().Count;

            //incorrectcount <= 1  т.к. последняя строка пустая
            if (incorrectcount <= 1 && _collection.Count > 1)
            {
                return true;
            }
            return false;
        }

        private void updateLastRow ()
        {
            _collection.ToList().ForEach(x => x.updateLastRow());
        }

    }
}
