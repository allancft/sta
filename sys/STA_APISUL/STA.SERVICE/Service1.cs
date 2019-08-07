using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;
using STA.LOG;
using STA.DOMAIN;

namespace STA.SERVICE
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log.RegistrarLogInformacao("SERVIÇO FOI INICIADO");
            Log.RegistrarLogInformacao("INTERVALO DE EXECUÇÕES : " + ConfigurationManager.AppSettings["IntervaloServico"] + " SEGUNDOS");
            int Intervalo = (1000 * Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloServico"]));

            System.Timers.Timer timer = new System.Timers.Timer(Intervalo);
            timer.Enabled = true;

            timer.Elapsed += new ElapsedEventHandler(ExecutaTimer);
        }

        protected override void OnStop()
        {
            Log.RegistrarLogInformacao("SERVIÇO FOI FINALIZADO");
        }

        private static void ExecutaTimer(object source, ElapsedEventArgs e)
        {
            DeParaDomain dePara = new DeParaDomain();
            dePara.TransferirArquivosDePara();
        }
    }
}
