using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Models.Base;

namespace WarehouseBlockForms.Models
{
    class ReportsCreated : Model
    {

        private int id_report_setting;
        private DateTime next_date_created;
        private bool is_created;

        public int IdReportSetting
        {
            get
            {
                return id_report_setting;
            }
            set
            {
                id_report_setting = value;
                RaisePropertyChaned("IdReportSetting", value);
            }
        }

        public DateTime NextDateCreated
        {
            get
            {
                return next_date_created;
            }
            set
            {
                next_date_created = value;
                RaisePropertyChaned("NextDateCreated", value);
            }
        }

        public bool IsCreated
        {
            get
            {
                return is_created;
            }
            set
            {
                is_created = value;
                RaisePropertyChaned("IsCreated", value);
            }
        }


        protected override string TableName
        {
            get
            {
                return "reports_created";
            }
        }

        protected override IController controller()
        {
            return ReportsCreatedController.instance();
        }

        protected override Dictionary<string, object> prepareRemoveParams()
        {
            throw new Exception("Удаление запрещено!");
        }

        protected override string prepareRemoveQuery()
        {
            throw new Exception("Удаление запрещено!");
        }

        protected override Dictionary<string, object> prepareSaveParams()
        {
            if (Id == 0)
            {
                return new Dictionary<string, object>()
                {
                    { "@id_report_setting",  IdReportSetting},
                    { "@next_date_created",  NextDateCreated},
                    { "@is_created",  IsCreated}
                };
            }
            return new Dictionary<string, object>()
            {
                {"@id_report_setting",  IdReportSetting},
                {"@next_date_created",  NextDateCreated},
                {"@is_created",  IsCreated},
                {"@id", Id }
            };
        }

        protected override string prepareSaveQuery()
        {
            if (Id == 0)
            {
                return "insert into " + TableName + " (id_report_setting, next_date_created, is_created) values(@id_report_setting, @next_date_created, @is_created)";
            }
            return "update " + TableName + " set is_created = @is_created where id = @id";
        }
    }
}
