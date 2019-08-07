using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using STA.DOMAIN;
using STA.DOMAIN.Recursos;
using STA.DOMAIN.Util;
using STA.MODEL;

namespace STA.UI.WEB.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpPost, AllowAnonymous]
        public ActionResult Autenticar(FormCollection formCollection)
        {
            UsuarioDomain usuarioDomain = new UsuarioDomain();

            string login = formCollection["Login"];
            string senha = formCollection["Senha"];

            login = login.ToLower().Replace("ccpr\\", "");

            //autenticação no Active Directory
            bool autenticado = usuarioDomain.AutenticarUsuarioAd(login, senha);

            //Se usuário autentica no AD corretamente então, realiza a autenticação no GSS
            if (autenticado)
            {
                CredencialModel credencialGss = new CredencialModel();
                credencialGss = usuarioDomain.AutenticarUsuarioGss(login);
                //usuarioDomain.SetUsuarioAtual(login);

                //Caso o perfil de acesso seja negado redireciona para login 
                if (credencialGss.PerfilAcesso.ToUpper() != "DIRETOR" 
                    && credencialGss.PerfilAcesso.ToUpper() != "ADMINISTRADOR" 
                    && credencialGss.PerfilAcesso.ToUpper() != "EXECUTOR" 
                    && credencialGss.PerfilAcesso.ToUpper() != "MASTER")
                {
                    @TempData["MessageErro"] = Mensagens.MSG_ErroSemAcessoGss;
                    return RedirectToAction("Autenticar");
                }

                //Seta o cookie de autenticação como o login do usuário
                FormsAuthentication.SetAuthCookie(login, false);
                CookieHelper.SetCookie("UsuarioAd", login);
                Session["UsuarioAd"] = login;
                //Caso 
                if (TempData["UrlRetorno"] == null)
                {
                    return RedirectToAction("Index", "DePara");
                }
                else
                {
                    return Redirect(TempData["UrlRetorno"].ToString());
                }
            }
            else
            {
                @TempData["MessageErro"] = Mensagens.MSG_UsuarioSenhaInvalidos;
                return RedirectToAction("Autenticar");
            }


        }

        [AllowAnonymous]
        public ActionResult Autenticar()
        {
            return View("Autenticar");
        }

        public ActionResult SairConfirmado()
        {
            FormsAuthentication.SignOut();
            CookieHelper.DeleteAllCookie();
            Session.Abandon();
            TempData["MessageSucesso"] = Mensagens.MSG_SairSucesso;
            return RedirectToAction("Autenticar", "Usuario");
        }

    }
}
