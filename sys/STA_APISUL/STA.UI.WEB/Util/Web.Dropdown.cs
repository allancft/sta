using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STA.UI.WEB.Util
{
    public class Web
    {
        public static List<SelectListItem> GetSelectListSimNao()
        {
            var list = new List<SelectListItem>();

            list.Add(new SelectListItem() { Text = "SIM", Value = "true" });
            list.Add(new SelectListItem() { Text = "NÃO", Value = "false" });
            
            return list;
        }
    }
}