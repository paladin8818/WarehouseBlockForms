using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Helpers
{
    class SWHelper
    {
        public int Id { get; private set; }

        public string TName { get; private set; }

        public DateTime OperationDate { get; private set; }

        public string AppNumber { get; private set; }

        public int? IdRecipient { get; private set; }

        public List<SWDetailsHelper> CurrentDetails
        {
            get
            {
                return (TSupply != null) ? SWDetailsHelper.GetListByEntity(TSupply) : SWDetailsHelper.GetListByEntity(TWriteoff);
            }
        }

        public string TId
        {
            get
            {
                return (TSupply != null) ? "П" + Id.ToString() : "С" + Id.ToString();
            }
        }

        private Supply TSupply { get; set; }
        private Writeoff TWriteoff { get; set; }

        public Recipients Recipient
        {
            get
            {
                if(IdRecipient != null)
                {
                    return RecipientsController.instance().getById((int)IdRecipient);
                }
                return null;
            }
        }

        private SWHelper () { }

        public static SWHelper Create (Writeoff writeoff)
        {
            SWHelper swHelper = new SWHelper();

            swHelper.Id = writeoff.Id;
            swHelper.TName = writeoff.TName;
            swHelper.OperationDate = writeoff.WriteoffDate;
            swHelper.AppNumber = writeoff.AppNumber;
            swHelper.IdRecipient = writeoff.IdRecipient;

            swHelper.TWriteoff = writeoff;

            return swHelper;
        }

        public static SWHelper Create (Supply supply)
        {
            SWHelper swHelper = new SWHelper();

            swHelper.Id = supply.Id;
            swHelper.TName = supply.TName;
            swHelper.OperationDate = supply.SupplyDate;

            swHelper.TSupply = supply;

            return swHelper;
        }

    }
}
