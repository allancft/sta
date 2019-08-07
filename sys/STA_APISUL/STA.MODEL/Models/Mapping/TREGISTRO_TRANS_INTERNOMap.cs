using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace STA.MODEL.Models.Mapping
{
    public class TREGISTRO_TRANS_INTERNOMap : EntityTypeConfiguration<TREGISTRO_TRANS_INTERNO>
    {
        public TREGISTRO_TRANS_INTERNOMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ANUM_SEQU_REGISTRO_INTERNO, t.ADES_EMAIL_USER, t.ADES_DESTINATARIO, t.ADAT_ENVIO });

            // Properties
            this.Property(t => t.ANUM_SEQU_REGISTRO_INTERNO)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ADES_EMAIL_USER)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ADES_DESTINATARIO)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TREGISTRO_TRANS_INTERNO");
            this.Property(t => t.ANUM_SEQU_REGISTRO_INTERNO).HasColumnName("ANUM_SEQU_REGISTRO_INTERNO");
            this.Property(t => t.ADES_EMAIL_USER).HasColumnName("ADES_EMAIL_USER");
            this.Property(t => t.ADES_DESTINATARIO).HasColumnName("ADES_DESTINATARIO");
            this.Property(t => t.ADAT_ENVIO).HasColumnName("ADAT_ENVIO");
        }
    }
}
