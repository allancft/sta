using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STA.DOMAIN.Util;
using STA.UI.WEB.Util;

namespace STA.UI.WEB.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        public BaseController()
        {
            var listUnidade = new List<SelectListItem>();

            listUnidade.Add(new SelectListItem() { Text = "SIM", Value = "True" });
            listUnidade.Add(new SelectListItem() { Text = "NÃO", Value = "False" });

            ViewBag.SimNao = listUnidade;
        }

        public void DeParaDadosIniciais()
        {
            ViewBag.SimNao = Web.GetSelectListSimNao();
        }
        //public bool VerificaSeUsuarioEstaLogado()
        //{
        //    string usuarioAd = CookieHelper.GetCookie("UsuarioAd");
        //    if (Session["UsuarioAd"] == null)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        if (String.IsNullOrEmpty(usuarioAd))
        //            CookieHelper.SetCookie("UsuarioAd", Session["UsuarioAd"].ToString());

        //        return true;
        //    }
        //}

    }
}
