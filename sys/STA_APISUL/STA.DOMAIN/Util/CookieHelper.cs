using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace STA.DOMAIN.Util
{
    public class CookieHelper
    {

        public static void SetCookie(string nome, string valor, int cookieExpireDate = 30)
        {
            //Criando cookie
            HttpCookie myCookie = new HttpCookie(nome);

            //criptografando cookie
            byte[] valorEmBytes = Encoding.Default.GetBytes(valor);

            //setando o valor do cookie
            myCookie.Value = Convert.ToBase64String(valorEmBytes);
            myCookie.Expires = DateTime.MinValue;//DateTime.Now.AddMinutes(cookieExpireDate);
            HttpContext.Current.Response.Cookies.Add(myCookie);

        }

        public static string GetCookie(string nome)
        {
            string cookieCriptografado = HttpContext.Current.Request.Cookies[nome] != null ? HttpContext.Current.Request.Cookies[nome].Value : "";
            string cookie = "";
            if (!String.IsNullOrEmpty(cookieCriptografado))
            {
                cookie = DecodeFrom64(cookieCriptografado);
            }
            return cookie;
        }

        public static void DeleteCookie(string nome)
        {

            HttpCookie myCookie = new HttpCookie(nome);
            myCookie.Expires = DateTime.Now.AddDays(-1d);
            HttpContext.Current.Response.Cookies.Add(myCookie);


            //HttpContext.Current.Request.Cookies.Remove(nome);
        }

        public static void DeleteAllCookie()
        {
            DeleteCookie(".ASPXAUTH");
            DeleteCookie("UsuarioAd");
            DeleteCookie("__RequestVerificationToken");
            DeleteCookie("divProjeto");
            DeleteCookie("ASP.NET_SessionId");
        }

        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes
                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        static public string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes
                = System.Convert.FromBase64String(encodedData);
            string returnValue =
               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

    }
}
