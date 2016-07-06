/*
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
	public class Oven : Model, IRowChangeable
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
				return "insert into oven (name, row_order) values (@name, @row_order)";
			}
			return "update oven set name = @name, row_order = @row_order where id = @id";
		}
		
		protected override string prepareRemoveQuery()
		{
			return "delete from oven where id = @id";
		}
		
		
		protected override Dictionary<string, object> prepareSaveParams()
		{
			if(Id == 0) 
			{
                RowOrder = GetNewRowOrder();
                return new Dictionary<string, object> () {
					{"@name", Name},
                    {"@row_order",  RowOrder}
				};
			}
			return new Dictionary<string, object> () {
				{"@id", Id},
				{"@name", Name},
                {"@row_order", RowOrder }
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

        public static int GetNewRowOrder ()
        {
            return (OvenController.instance().maxRowOrderIndex() + 1);
        }


        public bool up()
        {
            int index = OvenController.instance().prevRowOrderIndex(RowOrder);
            if (index == 0) return true;
            Oven oven = OvenController.instance().getByOrderIndex(index);

            if(orderChange(oven))
            {
                OvenController.instance().ViewSource.View.Refresh();
                return true;
            }
            return false;
        }
        public bool down ()
        {
            int index = OvenController.instance().nextRowOrderIndex(RowOrder);
            if (index == 0) return true;
            Oven oven = OvenController.instance().getByOrderIndex(index);
            if (orderChange(oven))
            {
                OvenController.instance().ViewSource.View.Refresh();
                return true;
            }
            return false;
        }

 
	}
}
