using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RuokaAppiBackend.Models
{
    public partial class kauppalistadbContext : DbContext
    {
        public kauppalistadbContext()
        {
        }

        public kauppalistadbContext(DbContextOptions<kauppalistadbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Kaupassakavijat> Kaupassakavijats { get; set; } = null!;
        public virtual DbSet<KauppaOstokset> KauppaOstoksets { get; set; } = null!;
        public virtual DbSet<Timesheet> Timesheets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-SU2GC2OF\\SQLEXPRESS;Database=kauppalistadb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kaupassakavijat>(entity =>
            {
                entity.HasKey(e => e.IdKavija)
                    .HasName("PK_Pyytajat");

                entity.ToTable("Kaupassakavijat");

                entity.Property(e => e.IdKavija).HasColumnName("Id_kavija");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Nimi).HasMaxLength(50);

                entity.Property(e => e.Puhelin).HasMaxLength(50);
            });

            modelBuilder.Entity<KauppaOstokset>(entity =>
            {
                entity.HasKey(e => e.IdKauppaOstos);

                entity.ToTable("KauppaOstokset");

                entity.Property(e => e.IdKauppaOstos).HasColumnName("Id_kauppaOstos");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.LastModifiedAt).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            modelBuilder.Entity<Timesheet>(entity =>
            {
                entity.HasKey(e => e.IdTimesheet);

                entity.ToTable("Timesheet");

                entity.Property(e => e.IdTimesheet).HasColumnName("Id_Timesheet");

                entity.Property(e => e.Comments).HasMaxLength(1000);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.IdKauppaOstos).HasColumnName("Id_KauppaOstos");

                entity.Property(e => e.IdKavija).HasColumnName("Id_kavija");

                entity.Property(e => e.LastModifiedAt).HasColumnType("datetime");

                entity.Property(e => e.StartLatitude).HasMaxLength(50);

                entity.Property(e => e.StartLongitude).HasMaxLength(50);

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.StopLatitude).HasMaxLength(50);

                entity.Property(e => e.StopLongitude).HasMaxLength(50);

                entity.Property(e => e.StopTime).HasColumnType("datetime");

                entity.HasOne(d => d.IdKauppaOstosNavigation)
                    .WithMany(p => p.Timesheets)
                    .HasForeignKey(d => d.IdKauppaOstos)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Timesheet_KauppaOstokset");

                entity.HasOne(d => d.IdKavijaNavigation)
                    .WithMany(p => p.Timesheets)
                    .HasForeignKey(d => d.IdKavija)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Timesheet_Kaupassakavijat");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
