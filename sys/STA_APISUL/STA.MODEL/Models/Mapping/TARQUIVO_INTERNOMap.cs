using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace STA.MODEL.Models.Mapping
{
    public class TARQUIVO_INTERNOMap : EntityTypeConfiguration<TARQUIVO_INTERNO>
    {
        public TARQUIVO_INTERNOMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ANUM_SEQU_ARQUIVO_INTERNO, t.ANOM_ARQUIVO });

            // Properties
            this.Property(t => t.ANUM_SEQU_ARQUIVO_INTERNO)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ANOM_ARQUIVO)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TARQUIVO_INTERNO");
            this.Property(t => t.ANUM_SEQU_ARQUIVO_INTERNO).HasColumnName("ANUM_SEQU_ARQUIVO_INTERNO");
            this.Property(t => t.ANUM_SEQU_REGISTRO_INTERNO).HasColumnName("ANUM_SEQU_REGISTRO_INTERNO");
            this.Property(t => t.ANOM_ARQUIVO).HasColumnName("ANOM_ARQUIVO");
        }
    }
}
