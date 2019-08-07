using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace STA.MODEL.Models.Mapping
{
    public class TDEPARAMap : EntityTypeConfiguration<TDEPARA>
    {
        public TDEPARAMap()
        {
            // Primary Key
            this.HasKey(t => t.ANUM_DEPARA);

            // Properties
            this.Property(t => t.ANOM_ORIGEM)
                .IsRequired();

            this.Property(t => t.ANOM_DESTINO)
                .IsRequired();

            this.Property(t => t.ATXT_USUARIO_ORIGEM)
                .HasMaxLength(150);

            this.Property(t => t.ATXT_SENHA_ORIGEM)
                .HasMaxLength(150);

            this.Property(t => t.ATXT_USUARIO_DESTINO)
                .HasMaxLength(150);

            this.Property(t => t.ATXT_SENHA_DESTINO)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("TDEPARA");
            this.Property(t => t.ANUM_DEPARA).HasColumnName("ANUM_DEPARA");
            this.Property(t => t.ANOM_ORIGEM).HasColumnName("ANOM_ORIGEM");
            this.Property(t => t.ANOM_DESTINO).HasColumnName("ANOM_DESTINO");
            this.Property(t => t.ATXT_SERVIDOR_ORIGEM).HasColumnName("ATXT_SERVIDOR_ORIGEM");
            this.Property(t => t.ATXT_USUARIO_ORIGEM).HasColumnName("ATXT_USUARIO_ORIGEM");
            this.Property(t => t.ATXT_SENHA_ORIGEM).HasColumnName("ATXT_SENHA_ORIGEM");
            this.Property(t => t.ABOL_USARSSL_ORIGEM).HasColumnName("ABOL_USARSSL_ORIGEM");
            this.Property(t => t.ATXT_SERVIDOR_DESTINO).HasColumnName("ATXT_SERVIDOR_DESTINO");
            this.Property(t => t.ATXT_USUARIO_DESTINO).HasColumnName("ATXT_USUARIO_DESTINO");
            this.Property(t => t.ATXT_SENHA_DESTINO).HasColumnName("ATXT_SENHA_DESTINO");
            this.Property(t => t.ABOL_USARSSL_DESTINO).HasColumnName("ABOL_USARSSL_DESTINO");
            this.Property(t => t.ABOL_ATIVO).HasColumnName("ABOL_ATIVO");
        }
    }
}
