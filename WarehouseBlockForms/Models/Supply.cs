using System;
using System.Collections.Generic;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Models.Base;

namespace WarehouseBlockForms.Models
{
    class Supply : Model
    {

        private DateTime supply_date;

        public string TName
        {
            get
            {
                return "Поступление";
            }
        }

        public DateTime SupplyDate
        {
            get
            {
                return supply_date;
            }
            set
            {
                supply_date = value;
                base.RaisePropertyChaned("SupplyDate", value);
            }
        }

        protected override string TableName
        {
            get
            {
                return "supply";
            }
        }

        protected override IController controller()
        {
            return SupplyController.instance();
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
                    {"@supply_date",  SupplyDate}
                };
            }
            return new Dictionary<string, object>()
            {
                {"@supply_date",  SupplyDate},
                {"@id", Id }
            };
        }

        protected override string prepareSaveQuery()
        {
            if(Id == 0)
            {
                return "insert into " + TableName + " (supply_date) values(@supply_date)";
            }
            return "update " + TableName + " set supply_date = @supply_date where id = @id";
        }
    }
}
