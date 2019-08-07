using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using STA.LOG;
using STA.DOMAIN.Enum;
using STA.MODEL.Models;
using STA.REPOSITORY;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace STA.DOMAIN
{
    public class DeParaDomain
    {
        public bool TransferirArquivosDePara()
        {
            try
            {

                Log.RegistrarLogInformacao("----- INICIO DE COPIA DE ARQUIVOS -----");
                Log.RegistrarLogInformacao("Pegando informações DE-PARA do banco");

                Repository<TDEPARA> deParaRepository = new Repository<TDEPARA>();
                List<TDEPARA> listDePara = deParaRepository.BuscarTodos().Where(m => m.ABOL_ATIVO == true).ToList();

                Log.RegistrarLogInformacao("Foram carregados " + listDePara.Count + " caminhos para copiar arquivos");

                foreach (var item in listDePara)
                {
                    Log.RegistrarLogInformacao("Iniciando copia de arquivos arquivos - DE: " + item.ANOM_ORIGEM + " - PARA: " + item.ANOM_DESTINO);
                    if (item.ATXT_SERVIDOR_ORIGEM == null && item.ATXT_SERVIDOR_DESTINO == null)
                    {
                        Log.RegistrarLogInformacao("Copia de arquivos locais");
                        CopiarPastasLocal(item);
                    }
                    else
                    {
                        //Caso tenha alguma credencial FTP ele precisará realizar a transferencia via FTP
                        Log.RegistrarLogInformacao("Copia de arquivos via FTP");
                        CopiarArquivosFTP(item);
                    }
                }
                Log.RegistrarLogInformacao("----- FIM DE COPIA DE ARQUIVOS -----");

                return true;
            }
            catch (Exception ex)
            {
                Log.RegistrarLogErro("OCORREU UM ERRO AO COPIAR OS ARQUIVOS");
                Log.RegistrarLogErro(ex.ToString());
                return false;
            }

        }

        void CriarPastaLocal(string pCaminho)
        {
            try
            {
                if (!System.IO.Directory.Exists(pCaminho))
                {
                    System.IO.Directory.CreateDirectory(pCaminho);
                    Log.RegistrarLogInformacao("Pasta " + pCaminho + " criada com sucesso");
                }
                else
                {
                    Log.RegistrarLogInformacao("Pasta " + pCaminho + " já existe");
                }
            }
            catch (Exception ex)
            {
                Log.RegistrarLogErro("OCORREU UM ERRO AO CRIAR A PASTA " + pCaminho);
                Log.RegistrarLogErro(ex.ToString());
            }
        }

        void CopiarPastasLocal(TDEPARA pDePara)
        {
            string caminhoDestino = pDePara.ANOM_DESTINO;
            string caminhoOrigem = pDePara.ANOM_ORIGEM;

            //Buscando dados da pasta raiz
            Log.RegistrarLogInformacao("Lendo pasta " + caminhoOrigem);
            DirectoryInfo Pastasraiz = new DirectoryInfo(caminhoOrigem);
            CopiarArquivosPorPastaLocal(Pastasraiz, caminhoOrigem, caminhoDestino);

            // Busca automaticamente todos os arquivos em todos os subdiretórios
            DirectoryInfo[] Pastas1 = new DirectoryInfo(pDePara.ANOM_ORIGEM).GetDirectories();
            foreach (DirectoryInfo pasta1 in Pastas1)
            {
                string destino1 = caminhoDestino + pasta1.Name;
                string origem1 = pDePara.ANOM_ORIGEM + pasta1.Name;
                Log.RegistrarLogInformacao("Lendo pasta " + origem1);
                CriarPastaLocal(destino1);
                CopiarArquivosPorPastaLocal(pasta1, origem1, destino1);
                DirectoryInfo[] Pastas2 = new DirectoryInfo(pDePara.ANOM_ORIGEM + pasta1.Name).GetDirectories();
                foreach (DirectoryInfo pasta2 in Pastas2)
                {
                    string destino2 = caminhoDestino + pasta1.Name + "\\" + pasta2.Name;
                    string origem2 = pDePara.ANOM_ORIGEM + pasta1.Name + "\\" + pasta2.Name;
                    Log.RegistrarLogInformacao("Lendo pasta " + origem2);
                    CriarPastaLocal(destino2);
                    CopiarArquivosPorPastaLocal(pasta2, origem2, destino2);
                    DirectoryInfo[] Pastas3 = new DirectoryInfo(origem2).GetDirectories();
                    foreach (DirectoryInfo pasta3 in Pastas3)
                    {
                        string destino3 = caminhoDestino + pasta1.Name + "\\" + pasta2.Name + "\\" + pasta3.Name;
                        string origem3 = pDePara.ANOM_ORIGEM + pasta1.Name + "\\" + pasta2.Name + "\\" + pasta3.Name;
                        Log.RegistrarLogInformacao("Lendo pasta " + origem3);
                        CriarPastaLocal(destino3);
                        CopiarArquivosPorPastaLocal(pasta3, origem3, destino3);
                        DirectoryInfo[] Pastas4 = new DirectoryInfo(origem3).GetDirectories();
                        foreach (DirectoryInfo pasta4 in Pastas4)
                        {
                            string destino4 = caminhoDestino + pasta1.Name + "\\" + pasta2.Name + "\\" + pasta3.Name + "\\" + pasta4.Name;
                            string origem4 = pDePara.ANOM_ORIGEM + pasta1.Name + "\\" + pasta2.Name + "\\" + pasta3.Name + "\\" + pasta4.Name;
                            Log.RegistrarLogInformacao("Lendo pasta " + origem4);
                            CriarPastaLocal(destino4);
                            CopiarArquivosPorPastaLocal(pasta4, origem4, destino4);
                            DirectoryInfo[] Pastas5 = new DirectoryInfo(origem4).GetDirectories();
                            foreach (DirectoryInfo pasta5 in Pastas5)
                            {
                                string destino5 = caminhoDestino + pasta1.Name + "\\" + pasta2.Name + "\\" + pasta3.Name + "\\" + pasta4.Name + "\\" + pasta5.Name;
                                string origem5 = pDePara.ANOM_ORIGEM + pasta1.Name + "\\" + pasta2.Name + "\\" + pasta3.Name + "\\" + pasta4.Name + "\\" + pasta5.Name;
                                Log.RegistrarLogInformacao("Lendo pasta " + origem5);
                                CriarPastaLocal(destino5);
                                CopiarArquivosPorPastaLocal(pasta5, origem5, destino5);
                            }
                        }
                    }
                }
            }
        }

        void CopiarArquivosPorPastaLocal(DirectoryInfo pOrigem, string pPastaOrigem, string pPastaDestino)
        {
            FileInfo[] Files = pOrigem.GetFiles("*", SearchOption.TopDirectoryOnly);
            Log.RegistrarLogInformacao("Foram encontrados " + Files.Count() + " arquivos para copiar");

            foreach (FileInfo File in Files)
            {
                pPastaOrigem = pPastaOrigem.EndsWith("\\") ? pPastaOrigem : pPastaOrigem + "\\";
                pPastaDestino = pPastaDestino.EndsWith("\\") ? pPastaDestino : pPastaDestino + "\\";

                string arquivoDe = pPastaOrigem + File.Name;
                string arquivoPara = pPastaDestino + File.Name;

                //Somente transfere arquivos maiores que 0 byte
                if (File.Length > 0)
                {
                    //Realiza cópia do arquivo
                    Log.RegistrarLogInformacao("Copiando arquivo para pastas: " + pPastaDestino + File.Name);
                    System.IO.File.Copy(pPastaOrigem + File.Name, pPastaDestino + File.Name, true);

                    //Verifica se os arquivos de origem e destido são iguais
                    FileInfo FileInfoPara = new FileInfo(pPastaDestino + File.Name);

                    if (File.Length == FileInfoPara.Length)
                    {
                        //Apaga arquivo de origem
                        Log.RegistrarLogInformacao("Apagando arquivo: " + pPastaOrigem + File.Name);
                        System.IO.File.Delete(arquivoDe);
                        Log.RegistrarLogInformacao("Arquivo apagado : " + pPastaDestino + File.Name);
                    }
                    else
                    {
                        Log.RegistrarLogErro("Arquivo de origem é diferente de arquivo de destino");
                        Log.RegistrarLogInformacao("Apagando arquivo copiado para o destino");
                        System.IO.File.Delete(arquivoPara);
                        Log.RegistrarLogInformacao("Arquivo copiado para o destino foi apagado");
                    }
                }
                else
                {
                    //caso o arquivo com 0KB esteja a mais do tempo configurado no config ele apaga o arquivo
                    DateTime dtUltimaModificacaoArquivo = File.LastWriteTime;
                    ArquivoDomain.ApagarArquivoVazio(File.FullName, dtUltimaModificacaoArquivo);
                    Log.RegistrarLogInformacao("Arquivo " + File.FullName + " com 0 byte não será copiado");
                }
            }
        }

        void CopiarArquivosFTP(TDEPARA pDePara)
        {
            if (pDePara.ATXT_SERVIDOR_ORIGEM != null && pDePara.ATXT_SERVIDOR_DESTINO != null)
            {
                //Caso ORIGEM e DESTINO sejam FTP primeiro faz o download do arquivo e depois sobe para o DESTINO
            }
            else if (pDePara.ATXT_SERVIDOR_ORIGEM != null && pDePara.ATXT_SERVIDOR_DESTINO == null)
            {
                //SOMENTE ORIGEM é um FTP então faz o download direto para a pasta de destino
                try
                {
                    Log.RegistrarLogInformacao("Origem é um FTP, ira ser feito o download do arquivo");
                    foreach (string item in GetListaArquivosFTP(pDePara, EnumTipoCredencial.De))
                    {
                        DownloadArquivoFTP(item, pDePara);
                    }
                }
                catch (Exception ex)
                {
                    Log.RegistrarLogErro("OCORREU UM ERRO AO COPIAR OS ARQUIVOS PARA FTP");
                    Log.RegistrarLogErro(ex.ToString());
                }
            }
            else if (pDePara.ATXT_SERVIDOR_ORIGEM == null && pDePara.ATXT_SERVIDOR_DESTINO != null)
            {
                //SOMENTE O DESTINO é um servidor ftp então, faz o UPLOAD de pasta física direto para a pasta no servidor FTP
                try
                {
                    Log.RegistrarLogInformacao("Destino é um FTP, ira ser feito o upload do arquivo");

                    DirectoryInfo Dir = new DirectoryInfo(pDePara.ANOM_ORIGEM);
                    //Busca automaticamente todos os arquivos em todos os subdiretórios
                    FileInfo[] Files = Dir.GetFiles("*", SearchOption.AllDirectories);

                    Log.RegistrarLogInformacao("Foram encontrados " + Files.Count() + " arquivos");

                    if (Files.Count() <= 0)
                    {
                        Log.RegistrarLogInformacao("A pasta " + pDePara.ANOM_ORIGEM + " não contém arquivos");
                    }

                    foreach (FileInfo File in Files)
                    {
                        Log.RegistrarLogInformacao("Copiando arquivo " + File.Name);
                        //Somente transfere arquivos maiores que 0 byte
                        if (File.Length > 0)
                        {
                            UploadArquivoFtp(File.Name, pDePara);
                            //FileTransferBusiness.TransferirArquivo(item.ANOM_DE, item.ANOM_PARA, File);
                        }
                        else
                        {
                            //caso o arquivo com 0KB esteja a mais do tempo configurado no config ele apaga o arquivo
                            DateTime dtUltimaModificacaoArquivo = File.LastWriteTime;
                            ArquivoDomain.ApagarArquivoVazio(File.FullName, dtUltimaModificacaoArquivo);
                            Log.RegistrarLogInformacao("Arquivo " + File.FullName + " com 0 byte não será copiado");
                        }
                    }
                }
                catch (Exception ex)
                {

                    Log.RegistrarLogErro("OCORREU UM ERRO AO COPIAR OS ARQUIVOS PARA FTP");
                    Log.RegistrarLogErro(ex.ToString());
                }

            }
        }

        /// <summary>
        /// Envia arquivo de uma pasta física para uma pasta FTP
        /// </summary>
        /// <param name="pNomeArquivoOrigem">Nome do arquivo que está na pasta de origem(física)</param>
        /// <param name="pCaminhoOrigem">Caminho físico onde está o arquivo que será feito o upload</param>
        /// <param name="pIdCredencial">ID da credencial que será usada para autenticar no servidor FTP</param>
        void UploadArquivoFtp(string pNomeArquivo, TDEPARA pDePara)
        {
            //TCREDENCIALFTP credencialFtpPara = new TCREDENCIALFTP(pDePara.ANUM_CREDENCIAL_PARA);
            string caminhoArquivoDestino = pDePara.ATXT_SERVIDOR_DESTINO + "/" + pDePara.ANOM_DESTINO + "/" + pNomeArquivo;
            string caminhoArquivoOrigem = pDePara.ANOM_ORIGEM + pNomeArquivo;
            Log.RegistrarLogInformacao("Iniciando Upload do arquivo " + caminhoArquivoDestino);
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(caminhoArquivoDestino);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(pDePara.ATXT_USUARIO_DESTINO, pDePara.ATXT_SENHA_DESTINO);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;
                //Habilitando SSL
                request.EnableSsl = true;
                //Passando por fora do proxy
                request.Proxy = null;

                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                var stream = File.OpenRead(caminhoArquivoOrigem);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();

                var reqStream = request.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);
                reqStream.Close();

                Log.RegistrarLogInformacao("Upload do arquivo " + caminhoArquivoDestino + " realizado com sucesso");


                try
                {
                    //Apaga arquivo de origem
                    Log.RegistrarLogInformacao("Apagando arquivo: " + caminhoArquivoOrigem);
                    System.IO.File.Delete(caminhoArquivoOrigem);
                    Log.RegistrarLogInformacao("Arquivo apagado : " + caminhoArquivoOrigem);
                }
                catch (Exception ex)
                {
                    Log.RegistrarLogErro("Ao realizar apagar o arquivo " + caminhoArquivoOrigem + " com a credencial " + pDePara.ANOM_DESTINO);
                    Log.RegistrarLogErro("Mensagem de erro: " + ex.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.RegistrarLogErro("Ao realizar o upload do arquivo " + pNomeArquivo + " com a credencial " + pDePara.ANOM_DESTINO);
                Log.RegistrarLogErro("Mensagem de erro: " + ex.ToString());
            }


        }

        /// <summary>
        /// Faz download de um arquivo em um servidor FTP e salva em uma pasta física
        /// </summary>
        /// <param name="pNomeArquivo">Nome do arquivo que será feito o download</param>
        /// <param name="pCaminho">Caminho no servidor FTP onde o arquivo (caminho completo)</param>
        /// <param name="pIdCredencial">ID da credenvial que será utilizada para conectar no servidor FTP</param>
        void DownloadArquivoFTP(string pNomeArquivo, TDEPARA pDePara)
        {
            try
            {
                //TCREDENCIALFTP credencialFtpDownload = new TCREDENCIALFTP(pDePara.ANUM_CREDENCIAL_DE);

                string caminhoArquidoFtp = pDePara.ATXT_SERVIDOR_ORIGEM + "/" + pDePara.ANOM_ORIGEM + "/" + pNomeArquivo;
                string caminhoArquivoLocal = pDePara.ANOM_DESTINO + "\\" + pNomeArquivo;

                Log.RegistrarLogInformacao("Iniciando download do arquivo " + caminhoArquidoFtp);

                //Cria comunicação com o servidor
                //definindo o arquivo para download
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(caminhoArquidoFtp);
                //Define que a ação vai ser de download
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                //Credenciais para o login (usuario, senha)
                request.Credentials = new NetworkCredential(pDePara.ATXT_USUARIO_ORIGEM, pDePara.ATXT_SENHA_ORIGEM);
                //modo passivo
                request.UsePassive = true;
                //dados binarios
                request.UseBinary = true;
                //setar o KeepAlive para true
                request.KeepAlive = true;
                //Habilitando SSL
                request.EnableSsl = true;
                //Passando por fora do proxy
                request.Proxy = null;

                //criando o objeto FtpWebResponse
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                //Criando a Stream para ler o arquivo
                Stream responseStream = response.GetResponseStream();

                byte[] buffer = new byte[2048];

                //Definir o local onde o arquivo será criado.
                FileStream newFile = new FileStream(caminhoArquivoLocal, FileMode.Create);
                //Ler o arquivo de origem
                int readCount = responseStream.Read(buffer, 0, buffer.Length);
                while (readCount > 0)
                {
                    //Escrever o arquivo
                    newFile.Write(buffer, 0, readCount);
                    readCount = responseStream.Read(buffer, 0, buffer.Length);
                }
                newFile.Close();
                responseStream.Close();
                response.Close();

                Log.RegistrarLogInformacao("Download do arquivo " + caminhoArquidoFtp + " realizado com sucesso");

                //APOS REALIZAR O DOWNLOAD DO ARQUIVO É NECESSÁRIO APAGAR NA PASTA
                if (!ApagarArquivoFTP(pDePara, pNomeArquivo, EnumTipoCredencial.De))
                {
                    new Exception("Ao tentar apagar o arquivo");
                }


            }
            catch (Exception ex)
            {
                Log.RegistrarLogErro("Ao realizar o download do arquivo " + pNomeArquivo + " com a credencial " + pDePara.ANOM_ORIGEM);
                Log.RegistrarLogErro("Mensagem de erro: " + ex.ToString());
            }


        }

        List<string> GetListaArquivosFTP(TDEPARA pDePara, Enum.EnumTipoCredencial pTipoCredencial)
        {
            string PastaListarAquivos;
            List<string> liArquivos = new List<string>();

            try
            {
                //TCREDENCIALFTP credencialFtp;
                if (pTipoCredencial == EnumTipoCredencial.De)
                    PastaListarAquivos = pDePara.ATXT_SERVIDOR_ORIGEM + pDePara.ANOM_ORIGEM;
                else
                    PastaListarAquivos = pDePara.ATXT_SERVIDOR_DESTINO + pDePara.ANOM_DESTINO;

                Log.RegistrarLogInformacao("Procurando arquivos na pasta " + PastaListarAquivos);
                //Cria comunicação com o servidor
                //Definir o diretório a ser listado
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(PastaListarAquivos);
                //Define que a ação vai ser de listar diretório
                request.Method = WebRequestMethods.Ftp.ListDirectory;


                //Credenciais para o login (usuario, senha)
                if (pTipoCredencial == EnumTipoCredencial.De)
                    request.Credentials = new NetworkCredential(pDePara.ATXT_USUARIO_ORIGEM, pDePara.ATXT_SENHA_ORIGEM);
                else
                    request.Credentials = new NetworkCredential(pDePara.ATXT_USUARIO_DESTINO, pDePara.ATXT_SENHA_DESTINO);

                //modo passivo
                request.UsePassive = true;
                //dados binarios
                request.UseBinary = true;
                //setar o KeepAlive para true
                request.KeepAlive = true;
                //Habilitando SSL
                request.EnableSsl = true;
                //Passando por fora do proxy
                request.Proxy = null;

                ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Log.RegistrarLogInformacao("Conexão realizada, obtendo arquivos");
                    //Criando a Stream para pegar o retorno
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        //Adicionar os arquivos na lista
                        liArquivos = reader.ReadToEnd().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    }
                }

                Log.RegistrarLogInformacao("Lista de arquivos carregada com sucesso");
            }
            catch (Exception ex)
            {
                string credencial = "";
                switch (pTipoCredencial)
                {
                    case EnumTipoCredencial.De:
                        credencial = pDePara.ATXT_SERVIDOR_ORIGEM + pDePara.ANOM_ORIGEM + " - " + pDePara.ATXT_USUARIO_ORIGEM;
                        break;
                    case EnumTipoCredencial.Para:
                        credencial = pDePara.ATXT_SERVIDOR_DESTINO + pDePara.ANOM_DESTINO + " - " + pDePara.ATXT_USUARIO_DESTINO;
                        break;
                }
                Log.RegistrarLogErro("Ao realizar o carregar lista de arquivos com a credencial " + credencial);
                Log.RegistrarLogErro("Mensagem de erro: " + ex.ToString());

            }

            return liArquivos;

        }

        private bool ApagarArquivoFTP(TDEPARA pDePara, string nomeArquivo, EnumTipoCredencial pTipoCredencial)
        {
            string caminhoArquivo = "";
            string arquivo = "";
            try
            {
                if (pTipoCredencial == EnumTipoCredencial.De)
                {
                    caminhoArquivo = pDePara.ATXT_SERVIDOR_ORIGEM;
                    arquivo = caminhoArquivo + pDePara.ANOM_ORIGEM + "/" + nomeArquivo;
                }
                else
                {
                    caminhoArquivo = pDePara.ATXT_SERVIDOR_DESTINO;
                    arquivo = caminhoArquivo + pDePara.ANOM_DESTINO + "/" + nomeArquivo;
                }

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(arquivo);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                if (pTipoCredencial == EnumTipoCredencial.De)
                    request.Credentials = new NetworkCredential(pDePara.ATXT_USUARIO_ORIGEM, pDePara.ATXT_SENHA_ORIGEM);
                else
                    request.Credentials = new NetworkCredential(pDePara.ATXT_USUARIO_DESTINO, pDePara.ATXT_SENHA_DESTINO);

                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;
                //Habilitando SSL
                request.EnableSsl = true;
                //Passando por fora do proxy
                request.Proxy = null;

                var responseFileDelete = (FtpWebResponse)request.GetResponse();
                responseFileDelete.Close();

                Log.RegistrarLogInformacao("Arquivo " + arquivo + " foi deletado com sucesso");

                return true;
            }
            catch (Exception ex)
            {
                Log.RegistrarLogErro("Ao tentar deletar o arquivo " + arquivo + " com a credencial " + pDePara.ANOM_DESTINO);
                Log.RegistrarLogErro("Mensagem de erro: " + ex.ToString());
                return false;
            }
        }
    }
}
