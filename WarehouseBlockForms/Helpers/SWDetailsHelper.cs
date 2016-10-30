using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Helpers
{
    class SWDetailsHelper
    {
        public string Name { get; private set; }
        public string VendorCode { get; private set; }
        public double Count { get; private set; }
        public string OvenName { get; private set; }

        private SWDetailsHelper() { }

        public static List<SWDetailsHelper> GetListByEntity(Supply supply)
        {
            List<SupplyDetails> supplyDetails =
                SupplyDetailsController.instance().Collection.Where(x => x.IdSupply == supply.Id).ToList();

            List<SWDetailsHelper> swdHelperList = new List<SWDetailsHelper>();

            for(int i = 0; i < supplyDetails.Count; i++)
            {
                SWDetailsHelper swdHelper = new SWDetailsHelper();
                swdHelper.Count = supplyDetails[i].DetailsCount;

                Details detail = DetailsController.instance().getById(supplyDetails[i].IdDetails);
                if (detail == null) continue;
                swdHelper.Name = detail.Name;
                swdHelper.OvenName = detail.OvenName;
                swdHelperList.Add(swdHelper);

            }

            return swdHelperList;
        }

        public static List<SWDetailsHelper> GetListByEntity(Writeoff writeoff)
        {
            List<WriteoffDetails> writeoffDetails =
                WriteoffDetailsController.instance().Collection.Where(x => x.IdWriteoff == writeoff.Id).ToList();

            List<SWDetailsHelper> swdHelperList = new List<SWDetailsHelper>();

            for (int i = 0; i < writeoffDetails.Count; i++)
            {
                SWDetailsHelper swdHelper = new SWDetailsHelper();
                swdHelper.Count = writeoffDetails[i].DetailsCount;

                Details detail = DetailsController.instance().getById(writeoffDetails[i].IdDetails);
                if (detail == null) continue;
                swdHelper.Name = detail.Name;
                swdHelper.OvenName = detail.OvenName;
                swdHelperList.Add(swdHelper);

            }

            return swdHelperList;
        }

    }
}
