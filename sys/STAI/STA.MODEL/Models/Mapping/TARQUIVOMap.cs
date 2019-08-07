using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace STA.MODEL.Models.Mapping
{
    public class TARQUIVOMap : EntityTypeConfiguration<TARQUIVO>
    {
        public TARQUIVOMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ANUM_SEQU_FORNECEDOR, t.ANUM_SEQU_REGISTRO, t.ANUM_SEQU_ARQUIVO, t.ANOM_ARQUIVO });

            // Properties
            this.Property(t => t.ANUM_SEQU_FORNECEDOR)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ANUM_SEQU_REGISTRO)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ANUM_SEQU_ARQUIVO)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ANOM_ARQUIVO)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TARQUIVO");
            this.Property(t => t.ANUM_SEQU_FORNECEDOR).HasColumnName("ANUM_SEQU_FORNECEDOR");
            this.Property(t => t.ANUM_SEQU_REGISTRO).HasColumnName("ANUM_SEQU_REGISTRO");
            this.Property(t => t.ANUM_SEQU_ARQUIVO).HasColumnName("ANUM_SEQU_ARQUIVO");
            this.Property(t => t.ANOM_ARQUIVO).HasColumnName("ANOM_ARQUIVO");
        }
    }
}
