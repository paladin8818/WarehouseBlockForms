using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Models.Base;

namespace WarehouseBlockForms.Models
{
    class ReportsSetting : Model
    {

        private string program_name;
        public string ProgramName
        {
            get
            {
                return program_name;
            }
            set
            {
                program_name = value;
                base.RaisePropertyChaned("ProgramName", value);
            }
        }
        private string report_name;
        public string ReportName
        {
            get
            {
                return report_name;
            }
            set
            {
                report_name = value;
                base.RaisePropertyChaned("ReportName", value);
            }
        }

        private string report_path;
        public string ReportPath
        {
            get
            {
                return report_path;
            }
            set
            {
                report_path = value;
                base.RaisePropertyChaned("ReportPath", value);
            }
        }

        private string period;
        public string Period
        {
            get
            {
                return period;
            }
            set
            {
                period = value;
                base.RaisePropertyChaned("Period", value);
            }
        }

        private int? day;
        public int? Day
        {
            get
            {
                return day;
            }
            set
            {
                day = value;
                base.RaisePropertyChaned("Day", value);
            }
        }

        private string time;
        public string Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                base.RaisePropertyChaned("Time", value);
            }
        }

        protected override string TableName
        {
            get
            {
                return "reports_setting";
            }
        }

        protected override IController controller()
        {
            return ReportsSettingController.instance();
        }

        protected override Dictionary<string, object> prepareRemoveParams()
        {
            throw new NotImplementedException();
        }

        protected override string prepareRemoveQuery()
        {
            throw new NotImplementedException();
        }

        protected override Dictionary<string, object> prepareSaveParams()
        {
            if (Id == 0)
            {
                throw new Exception("Добавление не разрешено!");
            }
            return new Dictionary<string, object>()
            {
                {"@report_name",  ReportName},
                {"@report_path",  ReportPath},
                {"@period",  Period},
                {"@day",  Day},
                {"@time",  Time},
                {"@id", Id }
            };
        }

        protected override string prepareSaveQuery()
        {
            if (Id == 0)
            {
                throw new Exception("Добавление не разрешено!");
            }
            return "update " + TableName + " set report_name = @report_name, report_path = @report_path, " +
                "period = @period, day = @day, time = @time where id = @id";
        }
    }
}
