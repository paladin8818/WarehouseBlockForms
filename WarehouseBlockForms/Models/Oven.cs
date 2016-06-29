﻿/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 06/27/2016
 * Время: 14:39
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Controllers.Base;
using WarehouseBlockForms.Models.Base;

namespace WarehouseBlockForms.Models
{
	public class Oven : Model
	{
        private string name;
		public string Name {
            get
            {
                return name;
            }
            set
            {
                name = value;
                base.RaisePropertyChaned("name", value);

                DetailsController.instance().Collection.Where(x => x.IdOven == Id).All(z => { z.updateOvens(); return true; });

            }
        }

        protected override string TableName
        {
            get
            {
                return "oven";
            }
        }

        public Oven() {}
		
		protected override string prepareSaveQuery()
		{
			if(Id == 0)
			{
				return "insert into oven (name) values (@name)";
			}
			return "update oven set name = @name where id = @id";
		}
		
		protected override string prepareRemoveQuery()
		{
			return "delete from oven where id = @id";
		}
		
		
		protected override Dictionary<string, object> prepareSaveParams()
		{
			if(Id == 0) 
			{
				return new Dictionary<string, object> () {
					{"@name", Name}
				};
			}
			return new Dictionary<string, object> () {
				{"@id", Id},
				{"@name", Name}
			};
		}
		
		protected override Dictionary<string, object> prepareRemoveParams()
		{
			return new Dictionary<string, object> () {
				{"@id", Id}
			};
		}
		
		protected override IController controller()
		{
			return OvenController.instance();
		}
	}
}
