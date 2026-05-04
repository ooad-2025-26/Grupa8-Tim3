using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Models;

namespace StudyBuddyApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Uloga> Uloge { get; set; }
        public DbSet<Predmet> Predmeti { get; set; }
        public DbSet<Lokacija> Lokacije { get; set; }
        public DbSet<SesijaUcenja> SesijeUcenja { get; set; }
        public DbSet<PrijavaNaSesiju> PrijaveNaSesije { get; set; }
        public DbSet<Prisustvo> Prisustva { get; set; }
        public DbSet<Obavjestenje> Obavjestenja { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Korisnik>().ToTable("Korisnik");
            modelBuilder.Entity<Uloga>().ToTable("Uloga");
            modelBuilder.Entity<Predmet>().ToTable("Predmet");
            modelBuilder.Entity<Lokacija>().ToTable("Lokacija");
            modelBuilder.Entity<SesijaUcenja>().ToTable("SesijaUcenja");
            modelBuilder.Entity<PrijavaNaSesiju>().ToTable("PrijavaNaSesiju");
            modelBuilder.Entity<Prisustvo>().ToTable("Prisustvo");
            modelBuilder.Entity<Obavjestenje>().ToTable("Obavjestenje");

            modelBuilder.Entity<PrijavaNaSesiju>()
                .HasOne(p => p.Korisnik)
                .WithMany()
                .HasForeignKey(p => p.KorisnikId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PrijavaNaSesiju>()
                .HasOne(p => p.SesijaUcenja)
                .WithMany()
                .HasForeignKey(p => p.SesijaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prisustvo>()
                .HasOne(p => p.Korisnik)
                .WithMany()
                .HasForeignKey(p => p.KorisnikId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prisustvo>()
                .HasOne(p => p.SesijaUcenja)
                .WithMany()
                .HasForeignKey(p => p.SesijaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Obavjestenje>()
                .HasOne(o => o.Korisnik)
                .WithMany()
                .HasForeignKey(o => o.KorisnikId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SesijaUcenja>()
                .HasOne(s => s.Kreator)
                .WithMany()
                .HasForeignKey(s => s.KreatorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}