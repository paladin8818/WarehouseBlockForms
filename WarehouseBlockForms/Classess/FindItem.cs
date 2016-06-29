/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 21.04.2016
 * Время: 17:00
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.Windows;
using System.Windows.Media;

namespace WarehouseBlockForms.Classess
{
	/// <summary>
	/// Description of FindParent.
	/// </summary>
	public static class FindItem
	{
		public static Parent FindParentItem<Parent>(DependencyObject child)
		            where Parent : DependencyObject
		{
		   DependencyObject parentObject = child;
		
		   //We are not dealing with Visual, so either we need to fnd parent or
		   //get Visual to get parent from Parent Heirarchy.
		   while (!((parentObject is System.Windows.Media.Visual)
		           || (parentObject is System.Windows.Media.Media3D.Visual3D)))
		   {
		       if (parentObject is Parent || parentObject == null)
		       {
		           return parentObject as Parent;
		       }
		       else
		       {
		          parentObject = (parentObject as FrameworkContentElement).Parent;
		       }
		    }
		
		    //We have not found parent yet , and we have now visual to work with.
		    parentObject = VisualTreeHelper.GetParent(parentObject);
		
		    //check if the parent matches the type we're looking for
		    if (parentObject is Parent || parentObject == null)
		    {
		       return parentObject as Parent;
		    }
		    else
		    {
		        //use recursion to proceed with next level
		        return FindParentItem<Parent>(parentObject);
		    }
		}
		
		public static UIElement FindUid(this DependencyObject parent, string uid)
		{
		    var count = VisualTreeHelper.GetChildrenCount(parent);
		    if (count == 0) return null;
		
		    for (int i = 0; i < count; i++)
		    {
		        var el = VisualTreeHelper.GetChild(parent, i) as UIElement;
		        if (el == null) continue;
		
		        if (el.Uid == uid) return el;
		
		        el = el.FindUid(uid);
		        if (el != null) return el;
		    }
		    return null;
		}
	}
}
