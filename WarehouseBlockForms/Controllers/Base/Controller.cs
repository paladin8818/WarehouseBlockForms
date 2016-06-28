/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 27.06.2016
 * Время: 16:43
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.Collections.Generic;
using System.Data.SQLite;

using WarehouseBlockForms.Models.Base;

namespace WarehouseBlockForms.Controllers.Base
{
	/// <summary>
	/// Description of Controller.
	/// </summary>
	public abstract class Controller
	{
		protected abstract string LoadQuery { get; }
		protected abstract void populate (SQLiteDataReader reader);
		protected abstract Dictionary<string, object> Parameters { get; }
		protected bool load () 
		{
			return Model.load(LoadQuery, Parameters, new Action<SQLiteDataReader>((reader) => populate(reader)));
		}
	}
}
