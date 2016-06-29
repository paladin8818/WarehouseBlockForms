using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Helpers
{
    class SupplyDetailsHelper : INotifyPropertyChanged
    {

        private int id_oven;
        private int id_detail;
        private int details_count = 1;

        public int RowIndex { get; set; }

        public ObservableCollection<Oven> Ovens
        {
            get
            {
                return OvenController.instance().Collection;
            }
        }
        public List<Details> Details
        {
            get
            {
                return DetailsController.instance().Collection.Where(x => x.IdOven == IdOven).ToList();
            }
        }

        public int IdOven
        {
            get
            {
                return id_oven;
            }
            set
            {
                id_oven = value;
                RaisePropertyChaned("Details", value);
            }
        }
        public int IdDetails
        {
            get
            {
                return id_detail;
            }
            set
            {
                id_detail = value;
                RaisePropertyChaned("VendorCode", null);
                RaisePropertyChaned("DetailsCount", null);
            }
        }

        public string VendorCode
        {
            get
            {
                Details detail = DetailsController.instance().getById(IdDetails);
                return (detail == null) ? "" : detail.VendorCode;
            }
        }

        public int DetailsCount
        {
            get
            {
                return details_count;
            }
            set
            {
                details_count = value;
                RaisePropertyChaned("DetailsCount", value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChaned(string name, object v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
