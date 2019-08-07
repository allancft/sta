using System;
using System.Collections.Generic;

namespace STA.MODEL.Models
{
    public partial class TFORNECEDOR
    {
        public TFORNECEDOR()
        {
            this.TREGISTRO_TRANSMISSAO = new List<TREGISTRO_TRANSMISSAO>();
        }

        public decimal ANUM_SEQU_FORNECEDOR { get; set; }
        public string ACOD_FORNECEDOR_BAAN { get; set; }
        public string ANOM_FORNECEDOR { get; set; }
        public decimal ANUM_CNPJ_CPF { get; set; }
        public decimal ANUM_TEL_FORNECEDOR { get; set; }
        public string ADES_EMAIL_FORNECEDOR { get; set; }
        public string ANOM_RESPONSAVEL_FORNECEDOR { get; set; }
        public string ADES_DIRETORIO_FORNECEDOR { get; set; }
        public string ADES_SENHA_FORNECEDOR { get; set; }
        public string AFTP_EXT_ARQU { get; set; }
        public string ADES_EMAIL_RESPONSAVEL { get; set; }
        public Nullable<decimal> ABOL_FORNECEDOR_MKT { get; set; }
        public Nullable<System.DateTime> ADAT_ATUALIZACAO { get; set; }
        public virtual ICollection<TREGISTRO_TRANSMISSAO> TREGISTRO_TRANSMISSAO { get; set; }
    }
}
