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
using WarehouseBlockForms.Classess;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace WarehouseBlockForms.Controllers
{
	/// <summary>
	/// Description of OvenController.
	/// </summary>
	public class OvenController : Controller, IController
	{
		private static OvenController _instance = null;
		private ObservableCollection<Oven> _collection = new ObservableCollection<Oven>();

        public CollectionViewSource ViewSource { get; set; }

		public ObservableCollection<Oven> Collection { 
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
                if(OrderSettings.OvenSortDirection == System.ComponentModel.ListSortDirection.Ascending)
				    return "select id, name, row_order from oven order by " + Oven.ClassDBFields[OrderSettings.OvenSortColumn] + " asc";
                else
                    return "select id, name, row_order from oven order by " + Oven.ClassDBFields[OrderSettings.OvenSortColumn] + " desc";
            }
		}
		
		private OvenController()
		{
            ViewSource = new CollectionViewSource();
            ViewSource.Source = _collection;
            load();
        }
		
		protected override void populate(SQLiteDataReader reader)
		{
			Oven oven = new Oven();
			oven.Id = reader.GetInt32(0);
			oven.Name = reader.GetString(1);
            oven.RowOrder = reader.GetInt32(2);
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

        public Oven getById (int id)
        {
            return _collection.Where(x => x.Id == id).FirstOrDefault();
        }

        public int maxRowOrderIndex ()
        {
            Oven oven = _collection.OrderByDescending(x => x.RowOrder).FirstOrDefault();
            if(oven == null)
            {
                return 0;
            }
            return oven.RowOrder;
        }

        public int prevRowOrderIndex (int currentIndex)
        {
            Oven oven = _collection.Where(x => x.RowOrder < currentIndex).OrderByDescending(y => y.RowOrder).FirstOrDefault();
            if(oven == null)
            {
                return 0;
            }
            return oven.RowOrder;
        }

        public int nextRowOrderIndex (int currentIndex)
        { 
            Oven oven = _collection.Where(x => x.RowOrder > currentIndex).OrderBy(y => y.RowOrder).FirstOrDefault();
            if(oven == null)
            {
                return 0;
            }
            return oven.RowOrder;
        }

        public Oven getByOrderIndex (int orderIndex)
        {
            return _collection.Where(x => x.RowOrder == orderIndex).FirstOrDefault();
        }
	}
}
