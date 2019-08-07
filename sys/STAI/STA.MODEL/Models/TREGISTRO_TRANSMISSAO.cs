using System;
using System.Collections.Generic;

namespace STA.MODEL.Models
{
    public partial class TREGISTRO_TRANSMISSAO
    {
        public decimal ANUM_SEQU_REGISTRO { get; set; }
        public string ATIP_PROTOCOLO { get; set; }
        public decimal ABOL_VALIDACAO_PROTOCOLO { get; set; }
        public System.DateTime ADAT_PROTOCOLO { get; set; }
        public decimal ABOL_EXCLUSAO { get; set; }
        public decimal ANUM_SEQU_FORNECEDOR { get; set; }
        public virtual TFORNECEDOR TFORNECEDOR { get; set; }
    }
}
