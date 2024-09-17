using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Inventarsystem1.Models
{
    public partial class LagerContext : DbContext
    {
        public LagerContext()
        {
        }

        public LagerContext(DbContextOptions<LagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Artikel> TblArtikel { get; set; } = null!;
        public virtual DbSet<Kategorie> TblKategorie { get; set; } = null!;
        public virtual DbSet<Produkt> TblProdukt { get; set; } = null!;
        public virtual DbSet<Raum> TblRaum { get; set; } = null!;
        public virtual DbSet<Role> TblRole { get; set; } = null!;
        public virtual DbSet<Seriennummer> TblSeriennummer { get; set; } = null!;
        public virtual DbSet<User> TblUser { get; set; } = null!;
        public virtual DbSet<UserRole> TblUserRole { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artikel>(entity =>
            {
                entity.HasKey(e => e.ArtikelId)
                    .HasName("PK__TblArtik__CB7A94AD75BEA8E8");

                entity.Property(e => e.ArtikelName).HasMaxLength(50);

                entity.Property(e => e.HatSeriennummer).HasColumnName("hatSeriennummer");

                entity.HasOne(d => d.Kategorie)
                    .WithMany(p => p.TblArtikel)
                    .HasForeignKey(d => d.KategorieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TblArtike__Kateg__37A5467C");
            });

            modelBuilder.Entity<Kategorie>(entity =>
            {
                entity.HasKey(e => e.KategorieId)
                    .HasName("PK__TblKateg__32DA91C2BF95C8A8");

                entity.Property(e => e.KategorieName).HasMaxLength(50);
            });

            modelBuilder.Entity<Produkt>(entity =>
            {
                entity.HasKey(e => e.ProduktId)
                    .HasName("PK__TblProdu__F1FF30022316B497");

                entity.Property(e => e.Datum).HasColumnType("datetime");

                entity.Property(e => e.Kommentar).HasMaxLength(100);

                entity.Property(e => e.Produktname).HasMaxLength(50);

                entity.HasOne(d => d.Artikel)
                    .WithMany(p => p.TblProdukt)
                    .HasForeignKey(d => d.ArtikelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TblProduk__Artik__32E0915F");

                entity.HasOne(d => d.Raum)
                    .WithMany(p => p.TblProdukt)
                    .HasForeignKey(d => d.RaumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)

                    .HasConstraintName("FK__TblProduk__RaumI__33D4B598");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblProdukt)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TblProduk__UserI__31EC6D26");
            });

            modelBuilder.Entity<Raum>(entity =>
            {
                entity.HasKey(e => e.RaumId)
                    .HasName("PK__TblRaum__C31E2C2778D505A2");

                entity.Property(e => e.RaumName).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblRaum)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TblRaum__UserId__38996AB5");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__TblRole__8AFACE1AE875E947");

                entity.Property(e => e.RoleName).HasMaxLength(30);
            });

            modelBuilder.Entity<Seriennummer>(entity =>
            {
                entity.HasKey(e => e.SeriennummerId)
                    .HasName("PK__TblSerie__4814BEFFF99FC9E0");

                entity.Property(e => e.SeriennummerId).ValueGeneratedNever();

                entity.HasOne(d => d.SeriennummerNavigation)
                    .WithOne(p => p.TblSeriennummer)
                    .HasForeignKey<Seriennummer>(d => d.SeriennummerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TblSerien__Serie__36B12243");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__TblUser__1788CC4CCA46B6CE");

                entity.Property(e => e.Nachname).HasMaxLength(30);

                entity.Property(e => e.Passwort).HasMaxLength(64);

                entity.Property(e => e.Email).HasMaxLength(30);

                entity.Property(e => e.Salt).HasMaxLength(50)
                .HasDefaultValueSql("('')");
                });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.UserRoleId)
                    .HasName("PK__TblUserR__3D978A35CA0D2B55");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblUserRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TblUserRo__RoleI__35BCFE0A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblUserRole)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TblUserRo__UserI__34C8D9D1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
