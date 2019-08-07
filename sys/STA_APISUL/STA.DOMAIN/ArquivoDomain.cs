using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using STA.LOG;

namespace STA.DOMAIN
{
    public static class ArquivoDomain
    {
        public static void ApagarArquivoVazio(string pNomeArquivo, DateTime pDataArquivo)
        {
            //Valor de limite para guardar arquivos com 0 byte
            int qtdHorasLimite = Convert.ToInt32(ConfigurationManager.AppSettings["ApagarArquivos"]);

            //Pegando o tempo de diferença entre a data da última modificação do arquivo e agora
            TimeSpan diferenca = DateTime.Now.Subtract(pDataArquivo);

            //Tempo limite de arquivos 0byte ficam na pasta de origem
            TimeSpan tempoLimete = new TimeSpan(qtdHorasLimite, 0, 0);

            if (diferenca > tempoLimete)
            {
                File.Delete(pNomeArquivo);
                Log.RegistrarLogInformacao("Arquivo " + pNomeArquivo + " com 0 byte foi apagado.");
            }

        }
    }
}


