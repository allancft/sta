using System;
using System.Collections.Generic;

namespace STA.MODEL.Models
{
    public partial class TREGISTRO_TRANS_INTERNO
    {
        public decimal ANUM_SEQU_REGISTRO_INTERNO { get; set; }
        public string ADES_EMAIL_USER { get; set; }
        public string ADES_DESTINATARIO { get; set; }
        public System.DateTime ADAT_ENVIO { get; set; }
    }
}
