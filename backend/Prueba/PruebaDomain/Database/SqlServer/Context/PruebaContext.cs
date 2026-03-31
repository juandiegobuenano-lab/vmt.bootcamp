using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PruebaDomain.Database.SqlServer.Entities;

namespace PruebaDomain.Database.SqlServer.Context;

public partial class PruebaContext : DbContext
{
    public PruebaContext()
    {
    }

    public PruebaContext(DbContextOptions<PruebaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Collection> Collections { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<UserStatusType> UserStatusTypes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;User=sa;Password=Admin1234@;Database=Prueba;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(e => e.CollectionId).HasName("PK__Collecti__7DE6BC242DB0853D");

            entity.Property(e => e.CollectionId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("CollectionID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasDefaultValue("THIS IS MY COLLECTION!");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasMany(d => d.Items).WithMany(p => p.Collections)
                .UsingEntity<Dictionary<string, object>>(
                    "CollectionsItem",
                    r => r.HasOne<Item>().WithMany()
                        .HasForeignKey("ItemId")
                        .HasConstraintName("FK_CollectionsItems_Item"),
                    l => l.HasOne<Collection>().WithMany()
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CollectionsItems_Collection"),
                    j =>
                    {
                        j.HasKey("CollectionId", "ItemId");
                        j.ToTable("CollectionsItems");
                        j.IndexerProperty<Guid>("CollectionId").HasColumnName("CollectionID");
                        j.IndexerProperty<int>("ItemId").HasColumnName("ItemID");
                    });
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Items__727E83EB5BD6043E");

            entity.HasIndex(e => e.Name, "IX_Items_Name");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A924E9014");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ShowName).HasMaxLength(100);
        });

        modelBuilder.Entity<UserStatusType>(entity =>
        {
            entity.HasKey(e => e.UserStatusTypeId).HasName("PK__UserStat__13CE8A579FDFF216");

            entity.ToTable("UserStatusType");

            entity.Property(e => e.UserStatusTypeId).HasColumnName("UserStatusTypeID");
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ShowName).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Usuario__1788CC4C006B72FF");

            entity.ToTable("Usuario");

            entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .HasColumnName("AvatarURL");
            entity.Property(e => e.BannerUrl)
                .HasMaxLength(255)
                .HasColumnName("BannerURL");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.StatusContent)
                .HasMaxLength(50)
                .HasDefaultValue("HEY!");
            entity.Property(e => e.StatusType).HasDefaultValue(1);
            entity.Property(e => e.Username).HasMaxLength(32);

            entity.HasOne(d => d.StatusTypeNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.StatusType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_StatusType");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UsersRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UsersRoles_Role"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UsersRoles_User"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("UsersRoles");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
