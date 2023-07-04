using eAchizitii.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace eAchizitii.Data
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {

        
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {

            modelbuilder.Entity<Produs_comanda>().HasKey(pc => new
            {
                
                pc.ProdusId,
                pc.ComandaId

            });

            modelbuilder.Entity<Produs_comanda>().HasOne(p => p.produs).
                WithMany(pc => pc.produse_comenzi).HasForeignKey(pc => pc.ProdusId);

            modelbuilder.Entity<Produs_comanda>().HasOne(c => c.comanda).
                WithMany(pc => pc.produse_comenzi).HasForeignKey(pc => pc.ComandaId);


            modelbuilder.Entity<Angajat_comanda>().HasKey(ac => new
            {

                ac.angajatId,
                ac.ComandaId,
                ac.RolComandaId

            });

            modelbuilder.Entity<Angajat_comanda>().HasOne(a => a.angajat).WithMany(ac => ac.angajati_comenzi).
                HasForeignKey(ac => ac.angajatId);
            modelbuilder.Entity<Angajat_comanda>().HasOne(c => c.comanda).WithMany(ac => ac.angajati_comenzi).
                HasForeignKey(ac => ac.ComandaId);
            modelbuilder.Entity<Angajat_comanda>().HasOne(r => r.rolComanda).WithMany(ac => ac.angajati_comenzi).
                HasForeignKey(ac => ac.RolComandaId);

            
            base.OnModelCreating(modelbuilder);


        }

        public DbSet<Produs> Produse { get; set; }
        public DbSet<Comanda> Comenzi { get; set; }
        public DbSet<AdresaLivrare> AdreseLivrare { get; set; }
        public DbSet<Sucursala> Sucursale { get; set; }
        public DbSet<Produs_comanda> Produse_comenzi { get; set; }
        public DbSet<Angajat_comanda> Angajati_comenzi{ get; set; }
        public DbSet<RolComanda> RoluriComenzi { get; set; }
        public DbSet<StatusComanda> StatusuriComanda { get; set; }
        public DbSet<InfoComanda> InfoComenzi { get; set; }
        public DbSet<MesajComanda> MesajeComanda { get; set; }
        public DbSet<Fisier> Fisiere { get; set; }

    }
}
