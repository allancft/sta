using System;
using System.Collections.Generic;

namespace STA.MODEL.Models
{
    public partial class TDEPARA
    {
        public int ANUM_DEPARA { get; set; }
        public string ANOM_ORIGEM { get; set; }
        public string ANOM_DESTINO { get; set; }
        public string ATXT_SERVIDOR_ORIGEM { get; set; }
        public string ATXT_USUARIO_ORIGEM { get; set; }
        public string ATXT_SENHA_ORIGEM { get; set; }
        public bool ABOL_USARSSL_ORIGEM { get; set; }
        public string ATXT_SERVIDOR_DESTINO { get; set; }
        public string ATXT_USUARIO_DESTINO { get; set; }
        public string ATXT_SENHA_DESTINO { get; set; }
        public bool ABOL_USARSSL_DESTINO { get; set; }
        public Nullable<bool> ABOL_ATIVO { get; set; }
        public string ATXT_DESC { get; set; }
        public Nullable<bool> ABOL_EXCLUIR_ORIGEM { get; set; }
        public string ATXT_PROTOCOLO_ORIGEM { get; set; }
        public string ATXT_PROTOCOLO_DESTINO { get; set; }
        public string ATXT_PORTA_ORIGEM { get; set; }
        public string ATXT_PORTA_DESTINO { get; set; }
    }
}
