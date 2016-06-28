/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 06/27/2016
 * Время: 16:32
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;

namespace WarehouseBlockForms.Controllers.Base
{
	/// <summary>
	/// Description of IController.
	/// </summary>
	public interface IController
	{
		bool add<T> (T model);
		bool remove<T> (T model);
	}
}
