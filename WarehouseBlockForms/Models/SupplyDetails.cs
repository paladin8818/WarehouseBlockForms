using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Models.Base;

namespace WarehouseBlockForms.Models
{
    class SupplyDetails : Model
    {

        private double details_count;
        private int id_details;
        private int id_supply;

        public double DetailsCount
        {
            get
            {
                return details_count;
            }
            set
            {
                details_count = value;
                base.RaisePropertyChaned("DetailsCount", value);
            }
        }

        public int IdDetails
        {
            get
            {
                return id_details;
            }
            set
            {
                id_details = value;
                base.RaisePropertyChaned("IdDetails", value);
            }
        }

        public int IdSupply
        {
            get
            {
                return id_supply;
            }
            set
            {
                id_supply = value;
                base.RaisePropertyChaned("IdSupply", value);
            }
        }


        protected override string TableName
        {
            get
            {
                return "supply_details";
            }
        }

        protected override IController controller()
        {
            return SupplyDetailsController.instance();
        }

        protected override Dictionary<string, object> prepareRemoveParams()
        {
            return new Dictionary<string, object>()
            {
                {"@id", Id }
            };
        }

        protected override string prepareRemoveQuery()
        {
            return "delete from " + TableName + " where id = @id";
        }

        protected override Dictionary<string, object> prepareSaveParams()
        {
            if(Id == 0)
            {
                return new Dictionary<string, object>()
                {
                    {"@details_count", DetailsCount },
                    {"@id_details", IdDetails },
                    {"@id_supply", IdSupply }
                };
            }
            return new Dictionary<string, object>()
            {
                {"@id", Id },
                {"@details_count", DetailsCount },
                {"@id_details", IdDetails },
                {"@id_supply", IdSupply }
            };
        }

        protected override string prepareSaveQuery()
        {
            if(Id == 0)
            {
                return "insert into " + TableName + " (details_count, id_details, id_supply) values(@details_count, @id_details, @id_supply)";
            }
            return "update " + TableName + " details_count = @details_count, id_details = @id_details, id_supply = @id_supply where id = @id";
        }
    }
}
