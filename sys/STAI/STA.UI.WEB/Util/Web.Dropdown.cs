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
        
        public static List<SelectListItem> GetSelectListProtocolo()
        {
            var list = new List<SelectListItem>();

            list.Add(new SelectListItem() { Text = "", Value = "" });
            list.Add(new SelectListItem() { Text = "NORMAL", Value = "NORMAL" });
            list.Add(new SelectListItem() { Text = "FTP", Value = "FTP" });
            list.Add(new SelectListItem() { Text = "SFTP", Value = "SFTP" });
            
            return list;
        }
        
    }
}