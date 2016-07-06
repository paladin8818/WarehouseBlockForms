using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Models.Base;

namespace WarehouseBlockForms.Models
{
    public class Details : Model, IRowChangeable
    {

        private string name;
        private string vendor_code;
        private int id_oven;
        private bool is_visible = true;

        public int RowIndex
        {
            get
            {
                return DetailsController.instance().getPos(this);
            }
            set
            {
                RaisePropertyChaned("RowIndex", null);
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                base.RaisePropertyChaned("name", value);
            }
        }

        public string VendorCode
        {
            get
            {
                return vendor_code;
            }
            set
            {
                vendor_code = value;
                base.RaisePropertyChaned("VendorCode", value);
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
                base.RaisePropertyChaned("IdOven", value);
                base.RaisePropertyChaned("OvenName", value);
            }
        }

        public Oven DetailOven
        {
            get
            {
                return OvenController.instance().getById(IdOven);
            }
        }

        public string OvenName
        {
            get
            {
                Oven oven = DetailOven;
                if (oven != null)
                {
                    return oven.Name;
                }
                return "";
            }
        }


        public int CurrentCount
        {
            get
            {
                int currentCount = (SupplyDetailsController.instance().allSupply(Id)
                    - WriteoffDetailsController.instance().allWriteoff(Id));
                return (currentCount < 0) ? 0 : currentCount;
            }
            set
            {
                base.RaisePropertyChaned("CurrentCount", null);
            }
        }

        public bool IsVisible {
            get
            {
                return is_visible;
            }
            set
            {
                is_visible = value;
                base.RaisePropertyChaned("IsVisible", value);
            }
        }

        public void updateOvens ()
        {
            base.RaisePropertyChaned("OvenName", null);
        }

        protected override string TableName
        {
            get
            {
                return "details";
            }
        }

        protected override IController controller()
        {
            return DetailsController.instance();
        }

        protected override Dictionary<string, object> prepareRemoveParams()
        {
            return new Dictionary<string, object>()
            {
                { "@id", Id }
            };
        }

        protected override string prepareRemoveQuery()
        {
            return "delete from "+TableName+" where id = @id";
        }

        protected override Dictionary<string, object> prepareSaveParams()
        {
            if(Id == 0)
            {
                RowOrder = GetNewRowOrder();
                return new Dictionary<string, object>()
                {
                    {"@name", Name},
                    {"@vendor_code", VendorCode },
                    {"@id_oven", IdOven },
                    {"@row_order",  RowOrder}
                };
            }
            return new Dictionary<string, object>()
            {
                {"@name", Name},
                {"@vendor_code", VendorCode },
                {"@id_oven", IdOven },
                {"@id", Id },
                {"@row_order",  RowOrder}
            };
        }

        protected override string prepareSaveQuery()
        {
            if(Id == 0)
            {
                return "insert into " + TableName + " (name, vendor_code, id_oven, row_order) values(@name, @vendor_code, @id_oven, @row_order)";
            }
            return "update " + TableName + " set name = @name, vendor_code = @vendor_code, id_oven = @id_oven, row_order = @row_order where id = @id ";
        }

        public static int GetNewRowOrder()
        {
            return (DetailsController.instance().maxRowOrderIndex() + 1);
        }


        public bool up()
        {
            int index = DetailsController.instance().prevRowOrderIndex(RowOrder);
            if (index == 0) return true;
            Details detail = DetailsController.instance().getByOrderIndex(index);

            if (orderChange(detail))
            {
                DetailsController.instance().ViewSource.View.Refresh();
                return true;
            }
            return false;
        }
        public bool down()
        {
            int index = DetailsController.instance().nextRowOrderIndex(RowOrder);
            if (index == 0) return true;
            Details detail = DetailsController.instance().getByOrderIndex(index);
            if (orderChange(detail))
            {
                DetailsController.instance().ViewSource.View.Refresh();
                return true;
            }
            return false;
        }

    }
}
