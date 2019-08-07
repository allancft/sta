using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STA.UI.WEB.Util;

namespace STA.UI.WEB.Controllers
{
    public class LogController : Controller
    {


        /// <summary>
        /// Retornar os arquivos de log encontrados na pasta padrão
        /// </summary>
        /// <returns></returns>
        [Autenticacao]
        public ActionResult Exibir()
        {
            string caminhoLog = ConfigurationManager.AppSettings["PastaLog"];
            //string[] lines = System.IO.File.ReadAllLines(caminhoLog);
            try
            {
                DirectoryInfo DirDe = new DirectoryInfo(caminhoLog);
                FileInfo[] Files = DirDe.GetFiles("*", SearchOption.TopDirectoryOnly);
                return View(Files);
            }
            catch (Exception)
            {
                FileInfo[] Files = new FileInfo[0];
                return View(Files);
                throw;
            }
        }

        public ActionResult ExibirArquivoLog(string nomeArquivo)
        {
            string caminhoLog = ConfigurationManager.AppSettings["PastaLog"];
            string[] lines = System.IO.File.ReadAllLines(caminhoLog + nomeArquivo);

            return View(lines);
        }

    }
}
