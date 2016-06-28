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
using System.Data.SqlClient;

using WarehouseBlockForms.Controllers.Base;

namespace WarehouseBlockForms.Models.Base
{
	/// <summary>
	/// Description of Model.
	/// </summary>
	public abstract class Model
	{
		public int Id { get; set; }
		private int LastInsertId { get; set; }
		
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
					Id = LastInsertId;
					controller().add(this);
				}
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
			query += "; select last_insert_rowid()";
			SQLiteConnection connection = DataBase.Connect();
			if(connection == null)
			{
				return false;
			}
			SQLiteCommand command = new SQLiteCommand(query, connection);
			prepareParams(command, parameters);
			try
			{
				LastInsertId = (Int32)command.ExecuteScalar();
				return true;
			}
			catch (Exception ex) {
				Log.WriteError("Класс Model, строка 101: " + ex.Message);
				return false;
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
	}
}
