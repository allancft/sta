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
        private bool emExecucao;
        private System.Timers.Timer _timer = new System.Timers.Timer();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log.RegistrarLogInformacao("SERVIÇO FOI INICIADO");
            Log.RegistrarLogInformacao("INTERVALO DE EXECUÇÕES : " + ConfigurationManager.AppSettings["IntervaloServico"] + " SEGUNDOS");
            int Intervalo = (1000 * Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloServico"]));

            _timer.AutoReset = true;
            _timer.Interval = Intervalo;
            _timer.Elapsed += OnElapsedEvent;
            _timer.Start();
        }
        private void OnElapsedEvent(object sender, ElapsedEventArgs e)
        {
            ExecutaTimer();
        }
        protected override void OnStop()
        {
            Log.RegistrarLogInformacao("SERVIÇO FOI FINALIZADO");
            base.OnStop();
            _timer.Stop();
        }
        private void ExecutaTimer()
        {
            if (this.emExecucao == false)
            {
                try
                {
                    this.emExecucao = true;
                    DeParaDomain dePara = new DeParaDomain();
                    dePara.TransferirArquivosDePara();
                }
                catch (Exception ex)
                {
                    Log.RegistrarLogErro("SEGUE ERRO: " + ex.ToString());
                    this.emExecucao = false;
                }
                finally
                {
                    Log.RegistrarLogInformacao("FIM DO PROCESSAMENTO DO SERVIÇO");
                    this.emExecucao = false;
                }
            }
            else
            {
                //Log.RegistrarLogInformacao("SERVIÇO AINDA ESTÁ EM EXECUÇÃO, AGUARDANDO FINALIZAR PARA INICIAR NOVAMENTE");
            }
        }
        //private void TimerContador_Tick(object sender, EventArgs e)
        //{
        //    this.ExecutaTimer();
        //}
        //private void TimerContador_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    this.ExecutaTimer();
        //}
    }
}
