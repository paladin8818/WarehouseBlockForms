using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Models.Base;

namespace WarehouseBlockForms.Models
{
    public class Recipients : Model
    {
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
                return new Dictionary<string, object>()
                {
                    { "@full_name", FullName}
                };
            }
            return new Dictionary<string, object>()
            {
                {"@full_name", FullName },
                {"@id", Id }
            };
        }

        protected override string prepareSaveQuery()
        {
            if(Id == 0)
            {
                return "insert into " + TableName + " (full_name) values(@full_name)";
            }
            return "update " + TableName + " set full_name = @full_name where id = @id";
        }
    }
}
