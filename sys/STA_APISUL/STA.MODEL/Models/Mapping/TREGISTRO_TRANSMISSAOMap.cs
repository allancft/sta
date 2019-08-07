using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace STA.MODEL.Models.Mapping
{
    public class TREGISTRO_TRANSMISSAOMap : EntityTypeConfiguration<TREGISTRO_TRANSMISSAO>
    {
        public TREGISTRO_TRANSMISSAOMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ANUM_SEQU_REGISTRO, t.ATIP_PROTOCOLO, t.ABOL_VALIDACAO_PROTOCOLO, t.ADAT_PROTOCOLO, t.ABOL_EXCLUSAO, t.ANUM_SEQU_FORNECEDOR });

            // Properties
            this.Property(t => t.ANUM_SEQU_REGISTRO)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ATIP_PROTOCOLO)
                .IsRequired()
                .HasMaxLength(12);

            this.Property(t => t.ABOL_VALIDACAO_PROTOCOLO)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ABOL_EXCLUSAO)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ANUM_SEQU_FORNECEDOR)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TREGISTRO_TRANSMISSAO");
            this.Property(t => t.ANUM_SEQU_REGISTRO).HasColumnName("ANUM_SEQU_REGISTRO");
            this.Property(t => t.ATIP_PROTOCOLO).HasColumnName("ATIP_PROTOCOLO");
            this.Property(t => t.ABOL_VALIDACAO_PROTOCOLO).HasColumnName("ABOL_VALIDACAO_PROTOCOLO");
            this.Property(t => t.ADAT_PROTOCOLO).HasColumnName("ADAT_PROTOCOLO");
            this.Property(t => t.ABOL_EXCLUSAO).HasColumnName("ABOL_EXCLUSAO");
            this.Property(t => t.ANUM_SEQU_FORNECEDOR).HasColumnName("ANUM_SEQU_FORNECEDOR");

            // Relationships
            this.HasRequired(t => t.TFORNECEDOR)
                .WithMany(t => t.TREGISTRO_TRANSMISSAO)
                .HasForeignKey(d => d.ANUM_SEQU_FORNECEDOR);

        }
    }
}
