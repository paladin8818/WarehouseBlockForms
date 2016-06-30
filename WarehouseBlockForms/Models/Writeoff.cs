using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Models.Base;

namespace WarehouseBlockForms.Models
{
    public class Writeoff : Model
    {
        private DateTime writeoff_date;
        private string app_number;
        private int id_recipient;

        public DateTime WriteoffDate
        {
            get
            {
                return writeoff_date;
            }
            set
            {
                writeoff_date = value;
                base.RaisePropertyChaned("WriteoffDate", value);
            }
        }

        public string AppNumber
        {
            get
            {
                return app_number;
            }
            set
            {
                app_number = value;
                base.RaisePropertyChaned("AppNumber", value);
            }
        }

        public int IdRecipient
        {
            get
            {
                return id_recipient;
            }
            set
            {
                id_recipient = value;
                base.RaisePropertyChaned("IdRecipient", value);
            }
        }

        protected override string TableName
        {
            get
            {
                return "writeoff";
            }
        }

        protected override IController controller()
        {
            return WriteoffController.instance();
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
                    {"@writeoff_date", WriteoffDate },
                    {"@app_number", AppNumber },
                    {"@id_recipient", IdRecipient }
                };
            }
            return new Dictionary<string, object>()
            {
                {"@id", Id },
                {"@writeoff_date", WriteoffDate },
                {"@app_number", AppNumber },
                {"@id_recipient", IdRecipient }
            };
        }

        protected override string prepareSaveQuery()
        {
            if(Id == 0)
            {
                return "insert into " + TableName + " (writeoff_date, app_number, id_recipient) values (@writeoff_date, @app_number, @id_recipient)";
            }
            return "update " + TableName + " set writeoff_date = @writeoff_date, app_number = @app_number, id_recipient = @id_recipient where id = @id";
        }
    }
}
