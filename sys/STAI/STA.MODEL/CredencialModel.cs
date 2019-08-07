using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STA.MODEL
{
    public class CredencialModel
    {
        public string Nome { get; set; }
        public int CodigoUsuarioGss { get; set; }
        public string PerfilAcesso { get; set; }
        public string Email { get; set; }
        public string Aplicacao { get; set; }
        public string Login { get; set; }
        public int IdSetor { get; set; }
        public string NomeSetor { get; set; }
        public string AplicacaoBloqueada { get; set; }
        public string AplicacaoPublica { get; set; }
    }
}
