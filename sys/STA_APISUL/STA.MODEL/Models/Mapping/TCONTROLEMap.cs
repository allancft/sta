using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace STA.MODEL.Models.Mapping
{
    public class TCONTROLEMap : EntityTypeConfiguration<TCONTROLE>
    {
        public TCONTROLEMap()
        {
            // Primary Key
            this.HasKey(t => t.ANUM_SEQU_CONTROLE);

            // Properties
            this.Property(t => t.ANOM_CONTROLE)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ADES_CONTROLE)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TCONTROLE");
            this.Property(t => t.ANUM_SEQU_CONTROLE).HasColumnName("ANUM_SEQU_CONTROLE");
            this.Property(t => t.ANOM_CONTROLE).HasColumnName("ANOM_CONTROLE");
            this.Property(t => t.ADES_CONTROLE).HasColumnName("ADES_CONTROLE");
            this.Property(t => t.AVAL_CONTROLE).HasColumnName("AVAL_CONTROLE");
        }
    }
}
