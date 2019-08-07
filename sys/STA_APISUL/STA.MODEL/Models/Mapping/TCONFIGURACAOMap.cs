using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace STA.MODEL.Models.Mapping
{
    public class TCONFIGURACAOMap : EntityTypeConfiguration<TCONFIGURACAO>
    {
        public TCONFIGURACAOMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ADAT_EXCLUSAO, t.ANOM_USER_ADMIN });

            // Properties
            this.Property(t => t.ADAT_EXCLUSAO)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AFTP_EXT_ARQU_USER_INTERNO)
                .HasMaxLength(100);

            this.Property(t => t.AFTP_EXT_ARQU_USER_FORNECEDOR)
                .HasMaxLength(100);

            this.Property(t => t.ANOM_USER_ADMIN)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TCONFIGURACAO");
            this.Property(t => t.ADAT_EXCLUSAO).HasColumnName("ADAT_EXCLUSAO");
            this.Property(t => t.AFTP_EXT_ARQU_USER_INTERNO).HasColumnName("AFTP_EXT_ARQU_USER_INTERNO");
            this.Property(t => t.AFTP_EXT_ARQU_USER_FORNECEDOR).HasColumnName("AFTP_EXT_ARQU_USER_FORNECEDOR");
            this.Property(t => t.ANOM_USER_ADMIN).HasColumnName("ANOM_USER_ADMIN");
        }
    }
}
