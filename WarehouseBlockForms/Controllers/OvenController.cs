/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 06/27/2016
 * Время: 15:46
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Models;
using WarehouseBlockForms.Classes;

namespace WarehouseBlockForms.Controllers
{
	/// <summary>
	/// Description of OvenController.
	/// </summary>
	public class OvenController : Controller, IController
	{
		private static OvenController _instance = null;
		private List<Oven> _collection;
		public List<Oven> Collection { 
			get
			{
				return _collection;	
			}
			private set
			{
				_collection = value;	
			}
		}
		protected override string LoadQuery
		{
			get {
				return "select id, name from oven";
			}
		}
		
		private OvenController()
		{
			_collection = new List<Oven>();
			load();
		}
		
		protected override void populate(SQLiteDataReader reader)
		{
			Oven oven = new Oven();
			oven.Id = reader.GetInt32(0);
			oven.Name = reader.GetString(1);
			add(oven);
		}
		
		public static OvenController instance ()
		{
			if(_instance == null)
			{
				_instance = new OvenController();
			}
			return _instance;
		}
		
		public List<Oven> findByName(string name)
		{
			name = name.ToLower();
			return _collection.Where(x => x.Name.ToLower().IndexOf(name) != -1).ToList();
		}
		
		public bool @add<T>(T model)
		{
			try
			{
				Oven oven = model as Oven;
				_collection.Add(oven);
				return true;
			}
			catch (Exception ex)
			{
				Log.WriteError("Класс OvenController, строка 73: " + ex.Message);
				return false;
			}
		}
		
		public bool @remove<T>(T model)
		{
			try
			{
				Oven oven = model as Oven;
				if(_collection.Contains(oven)) 
				{
					_collection.Remove(oven);
				}
				return true;
			}
			catch (Exception ex)
			{
				Log.WriteError("Класс OvenController, строка 91: " + ex.Message);
				return false;
			}
		}
		
		protected override Dictionary<string, object> Parameters {
			get {
				return new Dictionary<string, object> ();
			}
		}
	}
}
