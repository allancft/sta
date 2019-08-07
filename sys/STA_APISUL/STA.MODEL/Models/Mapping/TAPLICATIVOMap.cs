using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace STA.MODEL.Models.Mapping
{
    public class TAPLICATIVOMap : EntityTypeConfiguration<TAPLICATIVO>
    {
        public TAPLICATIVOMap()
        {
            // Primary Key
            this.HasKey(t => t.ANUM_SEQU_APLIC);

            // Properties
            this.Property(t => t.ANUM_SEQU_APLIC)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ANOM_APLIC)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(8);

            this.Property(t => t.ANOM_DIR_RAIZ)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("TAPLICATIVO");
            this.Property(t => t.ANUM_SEQU_APLIC).HasColumnName("ANUM_SEQU_APLIC");
            this.Property(t => t.ANOM_APLIC).HasColumnName("ANOM_APLIC");
            this.Property(t => t.ANOM_DIR_RAIZ).HasColumnName("ANOM_DIR_RAIZ");
        }
    }
}
