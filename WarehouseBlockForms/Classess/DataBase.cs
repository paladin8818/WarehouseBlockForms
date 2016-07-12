/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 27.06.2016
 * Время: 13:19
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.Data.SQLite;
using System.IO;

namespace WarehouseBlockForms.Classess
{
	/// <summary>
	/// Description of DataBase.
	/// </summary>
	public class DataBase
	{
		private static string _db_path = @"warehouse.sqlite";
		private static SQLiteConnection _connection = null;
		
		private DataBase() {}
		
		public static SQLiteConnection Connect ()
		{
			if(!File.Exists(_db_path)) 
			{
				if(!createDb())
				{
					return null;
				}
			}
			if(_connection == null)
			{
				try
				{
					SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0}; Version=3;", _db_path));
					connection.Open();
					return connection;
				}
				catch (Exception ex)
				{
					Log.WriteError(ex.Message);
					return null;
				}
			}
			return _connection;
		}

        public static void Backup(string path, string postfix = "")
        {
            if(Directory.Exists(path))
            {
                string name = "\\backup_db_" + DateTime.Now.ToString("dd_MM_yyyy__HH_mm_ss") + postfix + ".sqlite";
                if(File.Exists(path + name))
                {
                    throw new Exception("File " + path + name + " exist!");
                }
                File.Copy(_db_path, (path + name));
                return;
            }
            throw new Exception("Directory " + path + " not exist!");
        }

        public static void Restore(string backupFile)
        {
            if(File.Exists(backupFile))
            {
                File.Copy(backupFile, _db_path, true);
            }
        }
		
		private static bool createDb () 
		{
			//oven table (печи)
			string query = "create table oven (" +
				"id integer primary key autoincrement not null," +
				"name text not null, " +
                "row_order integer not null unique);";
			//details table (детали)
			query += "create table details (" +
				"id integer primary key autoincrement not null," +
				"name text not null," +
				"vendor_code text not null unique," +
				"id_oven integer not null," +
                "row_order integer not null unique, " +
				"foreign key (id_oven) references oven (id) on delete cascade);";
			//recipients table (получатели)
			query += "create table recipients (" +
				"id integer primary key autoincrement not null," +
				"full_name text not null, " +
                "row_order integer not null unique);";
			//supply table (поступление деталей)
			query += "create table supply (" +
				"id integer primary key autoincrement not null," +
				"supply_date datetime not null);";
			//supply_details table (детали в поступлении)
			query += "create table supply_details (" +
				"id integer primary key autoincrement not null," +
				"details_count integer not null," +
				"id_details integer not null," +
				"id_supply integer not null," +
				"foreign key (id_details) references details (id) on delete cascade," +
				"foreign key (id_supply) references supply (id) on delete cascade);";
			//writeoff table (списание деталей)
			query += "create table writeoff (" +
				"id integer primary key autoincrement not null," +
				"writeoff_date datetime not null," +
				"app_number text not null unique," +
				"id_recipient integer not null," +
				"foreign key (id_recipient) references recipients (id) on delete cascade);";
			//writeoff_details table (детали в списании)
			query += "create table writeoff_details (" +
				"id integer primary key autoincrement not null," +
				"details_count integer not null," +
				"id_details integer not null," +
				"id_writeoff integer not null," +
				"foreign key (id_details) references details (id) on delete cascade," +
				"foreign key (id_writeoff) references writeoff (id) on delete cascade);";
			
			
			SQLiteConnection.CreateFile(_db_path);
			SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", _db_path));
			SQLiteCommand command = new SQLiteCommand(query, connection);
			try
			{
				connection.Open();
				command.ExecuteNonQuery();
				connection.Close();
				return true;
			}
			catch (Exception ex) 
			{
				Log.WriteError(ex.Message);
				return false;
			}
			
			
		}
		
	}
}
