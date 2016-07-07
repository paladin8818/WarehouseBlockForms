/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 06/27/2016
 * Время: 14:37
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.Collections.Generic;
using System.Data.SQLite;

using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Classes;
using System.Diagnostics;
using System.ComponentModel;

namespace WarehouseBlockForms.Models.Base
{
	/// <summary>
	/// Description of Model.
	/// </summary>
	public abstract class Model : INotifyPropertyChanged
	{
        private int id;
        private int rowOrder;
        private bool isVisible = true;
        private Dictionary<string, bool> filters = new Dictionary<string, bool>();

		public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                RaisePropertyChaned("Id", value);
            }
        }
        public int RowOrder
        {
            get
            {
                return rowOrder;
            }
            set
            {
                rowOrder = value;
                RaisePropertyChaned("RowOrder", value);
            }
        }
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
                RaisePropertyChaned("IsVisible", value);
            }
        }
		
        protected abstract string TableName { get; }

        protected void RaisePropertyChaned (string name, object v)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected abstract string prepareSaveQuery ();
		protected abstract string prepareRemoveQuery ();
		
		protected abstract Dictionary<string, object> prepareSaveParams();
		protected abstract Dictionary<string, object> prepareRemoveParams();
		
		protected abstract IController controller ();
		
		public bool save ()
		{
			if(execQuery (prepareSaveQuery(), prepareSaveParams()))
			{
				if(Id == 0) 
				{
                    int id = lastInsertId();
					if(id != 0)
                    {
                        Id = id;
                        return controller().add(this);
                    }
                    return false;
				}
                return true;
			}
			return false;
		}
		
		public bool remove () 
		{
			if(execQuery(prepareRemoveQuery(), prepareRemoveParams()))
			{
				controller().remove(this);
			}
			return false;
		}
		
		public static bool load (string query,
		                         Dictionary<string, object> parameters,
		                         Action<SQLiteDataReader> callback)
		{
			SQLiteConnection connection = DataBase.Connect();
			if(connection == null)
			{
				return false;
			}
			SQLiteCommand command = new SQLiteCommand(query, connection);
			prepareParams(command, parameters);
            try
            {
            	SQLiteDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    callback(reader);      
                }
                reader.Close();
                return true;
            }
            catch(Exception ex)
            {
                Log.WriteError("Класс Model, строка 79: " + ex.Message);
                return false;
            }
		}
		
		protected bool execQuery (string query,
		                        Dictionary<string, object> parameters)
		{
			SQLiteConnection connection = DataBase.Connect();
			if(connection == null)
			{
				return false;
			}
			SQLiteCommand command = new SQLiteCommand(query, connection);
			prepareParams(command, parameters);
			try
			{
                command.ExecuteNonQuery();
				return true;
			}
			catch (Exception ex) {
				Log.WriteError("Класс Model, строка 101: " + ex.Message);
				return false;
			}
		}
		
        protected int lastInsertId ()
        {
            //TODO: кастыль получения id
            SQLiteConnection connection = DataBase.Connect();
            if (connection == null)
            {
                return 0;
            }
            SQLiteCommand command = new SQLiteCommand("select max(id) from " + TableName, connection);
            try
            {
                Int64 id = (Int64)command.ExecuteScalar();
                return (int)id;
            }
            catch (Exception ex)
            {
                Log.WriteError("Класс Model, строка 129: " + ex.Message);
                return 0;
            }
        }

		public static void prepareParams (SQLiteCommand command,
		                              Dictionary<string, object> parameters)
		{
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
		}

        protected bool orderChange (Model model)
        {
            if (model == null) return true;
            int currentRowIndex = RowOrder;
            int newRowIndex = model.RowOrder;

            RowOrder = 0;
            model.RowOrder = currentRowIndex;

            if(save() && model.save())
            {
                RowOrder = newRowIndex;
                if(save())
                {
                    return true;
                }
                RowOrder = currentRowIndex;
                model.RowOrder = newRowIndex;
                return false;
            }
            RowOrder = currentRowIndex;
            model.RowOrder = newRowIndex;
            return false;
        }

        public void setFilter (string propertyName, bool visible)
        {
            if(filters.ContainsKey(propertyName))
            {
                filters[propertyName] = visible;
            }
            else
            {
                filters.Add(propertyName, visible);
            }
            updateFilter();
        }

        public void setFilters(string[] properties, bool visible)
        {
            for(int i = 0; i < properties.Length; i++)
            {
                setFilter(properties[i], visible);
            }
        }

        private void updateFilter ()
        {
            foreach(KeyValuePair<string, bool> filterValue in filters)
            {
                if (filterValue.Value == false)
                {
                    IsVisible = false;
                    return;
                }
            }
            isVisible = true;
        }

	}
}
