using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace STA.LOG
{
    public class Log
    {
        public static void FormataStringParaDuasCasas(ref string pString)
        {
            if (pString.Length == 1)
                pString = "0" + pString;
        }

        public static void RegistrarLogInformacao(string pMensagem)
        {
            string mes = DateTime.Now.Month.ToString();
            FormataStringParaDuasCasas(ref mes);

            string dia = DateTime.Now.Day.ToString();
            FormataStringParaDuasCasas(ref dia);

            string ano = DateTime.Now.Year.ToString();

            string hora = DateTime.Now.Hour.ToString();
            FormataStringParaDuasCasas(ref hora);

            string minuto = DateTime.Now.Minute.ToString();
            FormataStringParaDuasCasas(ref minuto);

            string segundos = DateTime.Now.Second.ToString();
            FormataStringParaDuasCasas(ref segundos);
            
            string mensagemLog = ano + mes + dia + hora + minuto + segundos + ": " + pMensagem;

            //Caminho onde o log será salvo
            string caminhoLog = ConfigurationManager.AppSettings["PastaLog"];

            //Nome do arquivo de log montado segundo as regras:
            string nomeArquivoLog = "STA_APISUL." + ano + mes + dia +".TXT";

            //Caminho completo do arquivo de log
            string arquivoLog = caminhoLog + nomeArquivoLog;

            if (!System.IO.File.Exists(arquivoLog))
                System.IO.File.Create(arquivoLog).Close();

            System.IO.TextWriter arquivo = System.IO.File.AppendText(arquivoLog);
            arquivo.WriteLine(mensagemLog);

            arquivo.Close();
        }
        public static void RegistrarLogErro(string pMensagem)
        {
            RegistrarLogInformacao("ERRO - " + pMensagem);
        }
    }
}
