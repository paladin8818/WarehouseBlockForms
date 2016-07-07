using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Helpers
{
    class SWCollection
    {
        private static SWCollection _instance = null;
        private ObservableCollection<SWHelper> _collection = new ObservableCollection<SWHelper>();
        public ObservableCollection<SWHelper> Collection
        {
            get
            {
                return _collection;
            }
        }
        public CollectionViewSource ViewSource { get; set; }
        private SWCollection()
        {
            //Добавление в соответствующих контроллерах
            SupplyController.instance().Collection.ToList().ForEach(x => add(SWHelper.Create(x)));
            WriteoffController.instance().Collection.ToList().ForEach(x => add(SWHelper.Create(x)));
            ViewSource = new CollectionViewSource();
            ViewSource.Source = _collection;


            SupplyController.instance().Collection.CollectionChanged += SupplyCollectionChanged;
            WriteoffController.instance().Collection.CollectionChanged += WriteoffCollectionChanged;
        }

        private void WriteoffCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add
                && e.NewItems != null)
            {
                for (int i = 0; i < e.NewItems.Count; i++)
                {
                    Writeoff writeoff = e.NewItems[i] as Writeoff;
                    add(SWHelper.Create(writeoff));
                }
            }
        }

        private void SupplyCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add 
                && e.NewItems != null)
            {
                for(int i = 0; i < e.NewItems.Count; i++)
                {
                    Supply supply = e.NewItems[i] as Supply;
                    add(SWHelper.Create(supply));
                }
            }
        }

        public static SWCollection instance()
        {
            if (_instance == null)
            {
                _instance = new SWCollection();
            }
            return _instance;
        }

        public void add(SWHelper swHelper)
        {
            _collection.Add(swHelper);
        }

        public void add(Supply supply)
        {
            _collection.Add(SWHelper.Create(supply));
        }

        public void add(Writeoff writeoff)
        {
            _collection.Add(SWHelper.Create(writeoff));
        }

    }
}
