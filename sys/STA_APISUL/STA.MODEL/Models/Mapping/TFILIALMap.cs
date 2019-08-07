using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace STA.MODEL.Models.Mapping
{
    public class TFILIALMap : EntityTypeConfiguration<TFILIAL>
    {
        public TFILIALMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ANUM_FILIAL_CCPR, t.ANUM_FILIAL_ITAMBE });

            // Properties
            this.Property(t => t.ANUM_FILIAL_CCPR)
                .IsRequired()
                .HasMaxLength(4);

            this.Property(t => t.ANUM_FILIAL_ITAMBE)
                .IsRequired()
                .HasMaxLength(4);

            // Table & Column Mappings
            this.ToTable("TFILIAL");
            this.Property(t => t.ANUM_FILIAL_CCPR).HasColumnName("ANUM_FILIAL_CCPR");
            this.Property(t => t.ANUM_FILIAL_ITAMBE).HasColumnName("ANUM_FILIAL_ITAMBE");
        }
    }
}
