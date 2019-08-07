using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace STA.MODEL.Models.Mapping
{
    public class TFORNECEDORMap : EntityTypeConfiguration<TFORNECEDOR>
    {
        public TFORNECEDORMap()
        {
            // Primary Key
            this.HasKey(t => t.ANUM_SEQU_FORNECEDOR);

            // Properties
            this.Property(t => t.ANUM_SEQU_FORNECEDOR)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ACOD_FORNECEDOR_BAAN)
                .IsRequired()
                .HasMaxLength(6);

            this.Property(t => t.ANOM_FORNECEDOR)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ADES_EMAIL_FORNECEDOR)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ANOM_RESPONSAVEL_FORNECEDOR)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ADES_DIRETORIO_FORNECEDOR)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ADES_SENHA_FORNECEDOR)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AFTP_EXT_ARQU)
                .HasMaxLength(50);

            this.Property(t => t.ADES_EMAIL_RESPONSAVEL)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TFORNECEDOR");
            this.Property(t => t.ANUM_SEQU_FORNECEDOR).HasColumnName("ANUM_SEQU_FORNECEDOR");
            this.Property(t => t.ACOD_FORNECEDOR_BAAN).HasColumnName("ACOD_FORNECEDOR_BAAN");
            this.Property(t => t.ANOM_FORNECEDOR).HasColumnName("ANOM_FORNECEDOR");
            this.Property(t => t.ANUM_CNPJ_CPF).HasColumnName("ANUM_CNPJ_CPF");
            this.Property(t => t.ANUM_TEL_FORNECEDOR).HasColumnName("ANUM_TEL_FORNECEDOR");
            this.Property(t => t.ADES_EMAIL_FORNECEDOR).HasColumnName("ADES_EMAIL_FORNECEDOR");
            this.Property(t => t.ANOM_RESPONSAVEL_FORNECEDOR).HasColumnName("ANOM_RESPONSAVEL_FORNECEDOR");
            this.Property(t => t.ADES_DIRETORIO_FORNECEDOR).HasColumnName("ADES_DIRETORIO_FORNECEDOR");
            this.Property(t => t.ADES_SENHA_FORNECEDOR).HasColumnName("ADES_SENHA_FORNECEDOR");
            this.Property(t => t.AFTP_EXT_ARQU).HasColumnName("AFTP_EXT_ARQU");
            this.Property(t => t.ADES_EMAIL_RESPONSAVEL).HasColumnName("ADES_EMAIL_RESPONSAVEL");
            this.Property(t => t.ABOL_FORNECEDOR_MKT).HasColumnName("ABOL_FORNECEDOR_MKT");
            this.Property(t => t.ADAT_ATUALIZACAO).HasColumnName("ADAT_ATUALIZACAO");
        }
    }
}
