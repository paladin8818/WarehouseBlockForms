using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Classess;
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
                RaisePropertyChaned("ProgramName", value);
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
                RaisePropertyChaned("ReportName", value);
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
                RaisePropertyChaned("ReportPath", value);
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
                RaisePropertyChaned("Period", value);
                RaisePropertyChaned("PeriodIsWeek", null);
                RaisePropertyChaned("PeriodIsMonth", null);
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
                RaisePropertyChaned("Day", value);
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
                RaisePropertyChaned("Time", value);
            }
        }

        private DateTime? next_date_created;
        public DateTime? NextDateCreated
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

        public bool PeriodIsWeek
        {
            get
            {
                if(Period == "period_week")
                {
                    return true;
                }
                return false;
            }
            set
            {
                RaisePropertyChaned("PeriodIsWeek", null);
            }
        }

        public bool PeriodIsMonth
        {
            get
            {
                if (Period == "period_month")
                {
                    return true;
                }
                return false;
            }
            set
            {
                RaisePropertyChaned("PeriodIsMonth", null);
            }
        }


        private DateTime? calculateNextDate()
        {
            if (Period == null || Period == "") return null;
            if (Time == null || Time == "") return null;
            if (Period != "period_day" && Day == null) return null; 
            switch(Period)
            {
                case "period_day": return calculateNextDateDay();
                case "period_week": return calculateNextDateWeek();
                case "period_month": return calculateNextDateMonth();
            }
            return null;
        }

        public override bool save()
        {
            NextDateCreated = calculateNextDate();
            return base.save();
        }

        private DateTime calculateNextDateDay()
        {
            string[] hh_mm = Time.Split(':');
            DateTime currentDate = DateTime.Now;
            DateTime nextDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day,
                int.Parse(hh_mm[0]), int.Parse(hh_mm[1]), 0);
            if(nextDate < currentDate)
            {
                return nextDate.AddDays(1);
            }
            return nextDate;
        }

        private DateTime calculateNextDateWeek()
        {
            string[] hh_mm = Time.Split(':');
            DateTime currentDate = DateTime.Now;
            DateTime nextDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day,
                int.Parse(hh_mm[0]), int.Parse(hh_mm[1]), 0);


            if ((int)nextDate.DayOfWeek > Day)
            {
                int nextDay = ((7 - ((int)nextDate.DayOfWeek)) + (int)Day);
                return nextDate.AddDays(Math.Abs(nextDay));
            }
            else if((int)nextDate.DayOfWeek < Day)
            {
                int nextDay = (((int)nextDate.DayOfWeek) - (int)Day);
                return nextDate.AddDays(Math.Abs(nextDay));
            }
            else
            {
                if (nextDate < currentDate)
                {
                    return nextDate.AddDays(7);
                }
                return nextDate;
            }
        }

        private DateTime calculateNextDateMonth()
        {
            string[] hh_mm = Time.Split(':');
            DateTime nextDate;
            DateTime currentDate = DateTime.Now;
            if(Day == 0)
            {
                nextDate = new DateTime(currentDate.Year, currentDate.Month+1, 1,
                    int.Parse(hh_mm[0]), int.Parse(hh_mm[1]), 0).AddDays(-1);
            }
            else
            {
                nextDate = new DateTime(currentDate.Year, currentDate.Month, (int)Day,
                                int.Parse(hh_mm[0]), int.Parse(hh_mm[1]), 0);
            }
            if (nextDate < currentDate)
            {
                return nextDate.AddMonths(1);
            }
            return nextDate;
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
                {"@next_date_created",  NextDateCreated},
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
                "period = @period, day = @day, time = @time, next_date_created = @next_date_created where id = @id";
        }
    }
}
