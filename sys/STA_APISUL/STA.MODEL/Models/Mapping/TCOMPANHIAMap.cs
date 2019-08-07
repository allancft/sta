using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace STA.MODEL.Models.Mapping
{
    public class TCOMPANHIAMap : EntityTypeConfiguration<TCOMPANHIA>
    {
        public TCOMPANHIAMap()
        {
            // Primary Key
            this.HasKey(t => t.ANUM_SEQU_COMP);

            // Properties
            this.Property(t => t.ANUM_SEQU_COMP)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ANUM_COMP)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(4);

            this.Property(t => t.ANOM_COMP)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("TCOMPANHIA");
            this.Property(t => t.ANUM_SEQU_COMP).HasColumnName("ANUM_SEQU_COMP");
            this.Property(t => t.ANUM_COMP).HasColumnName("ANUM_COMP");
            this.Property(t => t.ANOM_COMP).HasColumnName("ANOM_COMP");
        }
    }
}
