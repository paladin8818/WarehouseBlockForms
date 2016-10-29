using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Models.Base;

namespace WarehouseBlockForms.Models
{
    public class Recipients : Model, IRowChangeable
    {
        public static readonly Dictionary<string, string> ClassDBFields =
            new Dictionary<string, string>()
            {
                {"Id", "id" },
                {"FullName", "full_name" },
                {"RowOrder", "row_order" }
            };

        private string full_name;
        public string FullName
        {
            get
            {
                return full_name;
            }
            set
            {
                full_name = value;

                base.RaisePropertyChaned("FullName", value);
            }
        }

        protected override string TableName
        {
            get
            {
                return "recipients";
            }
        }

        protected override IController controller()
        {
            return RecipientsController.instance();
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
                RowOrder = GetNewRowOrder();
                return new Dictionary<string, object>()
                {
                    { "@full_name", FullName},
                    {"@row_order",  RowOrder}
                };
            }
            return new Dictionary<string, object>()
            {
                {"@full_name", FullName },
                {"@id", Id },
                {"@row_order",  RowOrder}
            };
        }

        protected override string prepareSaveQuery()
        {
            if(Id == 0)
            {
                return "insert into " + TableName + " (full_name, row_order) values(@full_name, @row_order)";
            }
            return "update " + TableName + " set full_name = @full_name, row_order = @row_order where id = @id";
        }

        public static int GetNewRowOrder()
        {
            return (RecipientsController.instance().maxRowOrderIndex() + 1);
        }

        public bool up()
        {
            int index = RecipientsController.instance().prevRowOrderIndex(RowOrder);
            if (index == 0) return true;
            Recipients recipient = RecipientsController.instance().getByOrderIndex(index);

            if (orderChange(recipient))
            {
                RecipientsController.instance().ViewSource.View.Refresh();
                return true;
            }
            return false;
        }
        public bool down()
        {
            int index = RecipientsController.instance().nextRowOrderIndex(RowOrder);
            if (index == 0) return true;
            Recipients recipient = RecipientsController.instance().getByOrderIndex(index);
            if (orderChange(recipient))
            {
                RecipientsController.instance().ViewSource.View.Refresh();
                return true;
            }
            return false;
        }

    }
}
