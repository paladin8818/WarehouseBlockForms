﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Helpers
{
    class WriteoffDetailsHelper : INotifyPropertyChanged
    {

        private int id_oven;
        private int id_detail;
        private double details_count = 0;

        public int RowIndex
        {
            get
            {
                return WriteoffDetailsHelperCollection.instance().getPos(this);
            }
            set
            {
                RaisePropertyChaned("RowIndex", null);
            }
        }

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
                List<Details> details = DetailsController.instance().Collection.Where(x => x.IdOven == IdOven).ToList();
                details.Sort(DetailRowOrderCompare);

                List<Details> currentDetails = WriteoffDetailsHelperCollection.instance()
                    .Collection.Where(x => x.IdOven == IdOven).Select(z => z.Detail).ToList();
                //Если уже установлена деталь и таже печь
                if (Detail != null && Detail.IdOven == IdOven)
                {
                    List<Details> existDetail = details.Except(currentDetails).ToList();
                    existDetail.Add(Detail);
                    return existDetail;
                }
                return details.Except(currentDetails).ToList();
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
                RaisePropertyChaned("DetailsCount", null);
                RaisePropertyChaned("Detail", null);
                RaisePropertyChaned("MinCount", null);
                RaisePropertyChaned("MaxCount", null);
            }
        }

        public double DetailsCount
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


        public int MinCount
        {
            get
            {
                if (Detail == null)
                {
                    return 0;
                }
                return 1;
            }
            set
            {
                RaisePropertyChaned("MinCount", null);
            }
        }

        public double MaxCount
        {
            get
            {
                if(Detail == null)
                {
                    return 0;
                }
                return Detail.CurrentCount;
            }
            set
            {
                RaisePropertyChaned("MaxCount", null);
            }
        }

        public Details Detail
        {
            get
            {
                return DetailsController.instance().getById(IdDetails);
            }
        }

        public bool IsLastRow
        {
            get
            {
                if (RowIndex == WriteoffDetailsHelperCollection.instance().Collection.Count)
                {
                    return true;
                }
                return false;
            }
            set
            {
                RaisePropertyChaned("IsLastRow", null);
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

        public void updateLastRow()
        {
            RaisePropertyChaned("IsLastRow", null);
        }

        //Details comparator
        private static int DetailRowOrderCompare(Details x, Details y)
        {
            if (x.RowOrder > y.RowOrder) return 1;
            if (x.RowOrder < y.RowOrder) return -1;
            return 0;
        }

    }
}
