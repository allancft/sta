using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using STA.MODEL.Models.Mapping;

namespace STA.MODEL.Models
{
    public partial class DB_STAContext : DbContext
    {
        static DB_STAContext()
        {
            Database.SetInitializer<DB_STAContext>(null);
        }

        public DB_STAContext()
            : base("Name=DB_STAContext")
        {
        }

        public DbSet<dtproperty> dtproperties { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<TAPLICATIVO> TAPLICATIVOes { get; set; }
        public DbSet<TARQUIVO> TARQUIVOes { get; set; }
        public DbSet<TARQUIVO_INTERNO> TARQUIVO_INTERNO { get; set; }
        public DbSet<TCOMPANHIA> TCOMPANHIAs { get; set; }
        public DbSet<TCONFIGURACAO> TCONFIGURACAOs { get; set; }
        public DbSet<TCONTROLE> TCONTROLEs { get; set; }
        public DbSet<TDEPARA> TDEPARAs { get; set; }
        public DbSet<TFILIAL> TFILIALs { get; set; }
        public DbSet<TFORNECEDOR> TFORNECEDORs { get; set; }
        public DbSet<TLOG_ACESSO_FORNECEDOR> TLOG_ACESSO_FORNECEDOR { get; set; }
        public DbSet<TREGISTRO_TRANS_INTERNO> TREGISTRO_TRANS_INTERNO { get; set; }
        public DbSet<TREGISTRO_TRANSMISSAO> TREGISTRO_TRANSMISSAO { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new dtpropertyMap());
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new TAPLICATIVOMap());
            modelBuilder.Configurations.Add(new TARQUIVOMap());
            modelBuilder.Configurations.Add(new TARQUIVO_INTERNOMap());
            modelBuilder.Configurations.Add(new TCOMPANHIAMap());
            modelBuilder.Configurations.Add(new TCONFIGURACAOMap());
            modelBuilder.Configurations.Add(new TCONTROLEMap());
            modelBuilder.Configurations.Add(new TDEPARAMap());
            modelBuilder.Configurations.Add(new TFILIALMap());
            modelBuilder.Configurations.Add(new TFORNECEDORMap());
            modelBuilder.Configurations.Add(new TLOG_ACESSO_FORNECEDORMap());
            modelBuilder.Configurations.Add(new TREGISTRO_TRANS_INTERNOMap());
            modelBuilder.Configurations.Add(new TREGISTRO_TRANSMISSAOMap());
        }
    }
}
