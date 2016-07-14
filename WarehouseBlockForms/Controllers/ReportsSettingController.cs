using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using WarehouseBlockForms.Classess;
using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Controllers
{
    class ReportsSettingController : Controller, IController
    {

        private static ReportsSettingController _instance = null;
        private ObservableCollection<ReportsSetting> _collection;
        public ObservableCollection<ReportsSetting> Collection
        {
            get
            {
                return _collection;
            }
            private set
            {
                _collection = value;
            }
        }

        public static ReportsSettingController instance()
        {
            if (_instance == null)
            {
                _instance = new ReportsSettingController();
            }
            return _instance;
        }

        private ReportsSettingController()
        {
            _collection = new ObservableCollection<ReportsSetting>();
            load();
        }

        protected override string LoadQuery
        {
            get
            {
                return "select id, program_name, report_name, report_path, period, day, time from reports_setting";
            }
        }

        protected override Dictionary<string, object> Parameters
        {
            get
            {
                return new Dictionary<string, object>();
            }
        }

        public bool add<T>(T model)
        {
            try
            {
                ReportsSetting reportsSetting = model as ReportsSetting;
                _collection.Add(reportsSetting);
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message);
                return false;
            }
        }

        public bool remove<T>(T model)
        {
            try
            {
                ReportsSetting reportsSetting = model as ReportsSetting;
                if (_collection.Contains(reportsSetting))
                {
                    _collection.Remove(reportsSetting);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message);
                return false;
            }
        }

        protected override void populate(SQLiteDataReader reader)
        {
            ReportsSetting reportsSetting = new ReportsSetting();
            reportsSetting.Id = reader.GetInt32(0);
            reportsSetting.ProgramName = reader.GetString(1);
            reportsSetting.ReportName = reader.GetString(2);
            if(!reader.IsDBNull(3))
            {
                reportsSetting.ReportPath = reader.GetString(3);
            }
            if (!reader.IsDBNull(4))
            {
                reportsSetting.Period = reader.GetString(4);
            }
            if(!reader.IsDBNull(5))
            {
                reportsSetting.Day = reader.GetInt32(5);
            }
            if (!reader.IsDBNull(6))
            {
                reportsSetting.Time = reader.GetString(6);
            }
            add(reportsSetting);
        }

        public string getPathByProgramName (string program_name)
        {
            ReportsSetting reportsSetting = _collection.Where(x => x.ProgramName == program_name).FirstOrDefault();
            if(reportsSetting != null)
            {
                return reportsSetting.ReportPath;
            }
            return "";
        }

    }
}
