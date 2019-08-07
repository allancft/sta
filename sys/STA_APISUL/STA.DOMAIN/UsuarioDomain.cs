using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using STA.MODEL;

namespace STA.DOMAIN
{
    public class UsuarioDomain
    {
        private DirectoryEntry _directoryEntry;

        private DirectoryEntry SearchRoot
        {
            get
            {
                if (_directoryEntry == null)
                {
                    _directoryEntry = new DirectoryEntry(Global.CaminhoAd, Global.UsuarioMasterAd, Global.SenhaMasterAd, AuthenticationTypes.Secure);
                }
                return _directoryEntry;
            }
        }

        private static String GetProperty(DirectoryEntry userDetail, String propertyName)
        {
            if (userDetail.Properties.Contains(propertyName))
            {
                return userDetail.Properties[propertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public bool AutenticarUsuarioAd(string usuario, string senha)
        {

            // faz cominicação com o active directory.
            using (
                DirectoryEntry directoryUsuario = new DirectoryEntry(Global.CaminhoAd, usuario, senha,
                    AuthenticationTypes.Secure))
            {
                try
                {
                    if (!String.IsNullOrEmpty(directoryUsuario.Name))
                    {
                        //Realiza a busca para ter os dados do AD.
                        DirectorySearcher directorySearch = new DirectorySearcher(directoryUsuario);
                        directorySearch.Filter = "(&(objectClass=user)(sAMAccountName=" + usuario + "))";
                        SearchResult results = directorySearch.FindOne();

                        string loginAd = directoryUsuario.Username;
                        string NomeCompleto = results.Properties["cn"][0].ToString();

                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (DirectoryServicesCOMException)
                {
                    return false;
                }
            }

        }

        public CredencialModel AutenticarUsuarioGss(string pLoginActiveDirectory)
        {
            CredencialModel credencialModel = new CredencialModel();

            string credencial = "";

            //** Caso o login/usuário logado não seja do domínio CCPR.
            if (!string.IsNullOrEmpty(pLoginActiveDirectory))
            {
                // Cria instancia do WebService de autenticação
                STA.DOMAIN.GssWebService.ServiceSoapClient wsRetorno = new STA.DOMAIN.GssWebService.ServiceSoapClient();

                try
                {
                    //faz a verificação e retorna a credencial do usuario
                    int idIntranetNoGss = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["IdAplicacaoGss"]);
                    credencial = wsRetorno.GetCredencial(pLoginActiveDirectory, idIntranetNoGss);

                    string[] myCred = credencial.Split(new Char[] { ',' });

                    credencialModel.CodigoUsuarioGss = Int32.Parse(myCred[0].Trim());
                    credencialModel.Nome = myCred[1].Trim();
                    credencialModel.Login = myCred[2].Trim();
                    credencialModel.Email = myCred[3].Trim();
                    credencialModel.IdSetor = myCred[4].Trim() == "" ? 0 : Int32.Parse(myCred[4].Trim());
                    credencialModel.NomeSetor = myCred[5].Trim();
                    credencialModel.Aplicacao = myCred[6].Trim();
                    credencialModel.PerfilAcesso = myCred[7].Trim();
                    credencialModel.AplicacaoBloqueada = myCred[8].ToUpper();
                    credencialModel.AplicacaoPublica = myCred[9].ToUpper();
                }
                catch (STA.DOMAIN.Util.Exception)
                {
                    credencialModel.AplicacaoBloqueada = "TRUE";
                }
            }
            else
            {
                credencialModel.AplicacaoBloqueada = "TRUE";
            }
            return credencialModel;
        }
    }
}
