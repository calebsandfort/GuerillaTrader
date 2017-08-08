using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GuerillaTrader.Entities;
using GuerillaTrader.Framework;

namespace GuerillaTrader.Web.Framework
{
    public static class Extensions
    {
        public static List<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static List<ListItem> EnumToListItems<T>(bool includeNone = false)
            where T : struct
        {
            List<ListItem> list = new List<ListItem>();
            foreach(Enum item in Enum.GetValues(typeof(T)))
            {
                if(item.ToString() != "None" || (item.ToString() == "None" && includeNone))
                    list.Add(new ListItem { Display = item.GetDisplay(), Value = ((int)(object)item).ToString() });
            }

            return list;
        }

        public static List<SelectListItem> EnumToSelectListItems<T>(bool includeNone = false)
            where T : struct
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (Enum item in Enum.GetValues(typeof(T)))
            {
                if (item.ToString() != "None" || (item.ToString() == "None" && includeNone))
                    list.Add(new SelectListItem { Text = item.GetDisplay(), Value = ((int)(object)item).ToString() });
            }

            return list;
        }

        public static List<SelectListItem> GetTradingSetups()
        {
            return Extensions.EnumToSelectListItems<TradingSetups>();
        }        
    }
}