using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace STA.MODEL.Models.Mapping
{
    public class TLOG_ACESSO_FORNECEDORMap : EntityTypeConfiguration<TLOG_ACESSO_FORNECEDOR>
    {
        public TLOG_ACESSO_FORNECEDORMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ANUM_SEQU_LOG_ACESSO, t.ANUM_SEQU_FORNECEDOR });

            // Properties
            this.Property(t => t.ANUM_SEQU_LOG_ACESSO)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ANUM_SEQU_FORNECEDOR)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TLOG_ACESSO_FORNECEDOR");
            this.Property(t => t.ANUM_SEQU_LOG_ACESSO).HasColumnName("ANUM_SEQU_LOG_ACESSO");
            this.Property(t => t.ANUM_SEQU_FORNECEDOR).HasColumnName("ANUM_SEQU_FORNECEDOR");
            this.Property(t => t.ADAT_ACESSO).HasColumnName("ADAT_ACESSO");
        }
    }
}
