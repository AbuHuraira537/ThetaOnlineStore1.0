using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace thetaonlinestore.Models
{
    public partial class ThetaOnlineStoreContext : DbContext
    {
        public ThetaOnlineStoreContext()
        {
        }

        public ThetaOnlineStoreContext(DbContextOptions<ThetaOnlineStoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<SystemUser> SystemUser { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=HURAIRASPC\\SQLEXPRESS;Database=ThetaOnlineStore;User ID=sa; Password=bsef17m537;Trusted_Connection=True;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .HasColumnName("Created_By")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("Created_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.LongDescription)
                    .HasColumnName("Long_Description")
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("Modified_By")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("Modified_Date")
                    .HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Picture).HasMaxLength(50);

                entity.Property(e => e.ShortDescription)
                    .HasColumnName("Short_Description")
                    .HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CostPrice).HasColumnName("Cost_Price");

                entity.Property(e => e.CurrentStock)
                    .HasColumnName("Current_Stock")
                    .HasMaxLength(50);

                entity.Property(e => e.Images).HasMaxLength(50);

                entity.Property(e => e.LongDescription)
                    .HasColumnName("Long_Description")
                    .HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.OpeningDate)
                    .HasColumnName("Opening_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OpeningStock)
                    .HasColumnName("Opening_Stock")
                    .HasMaxLength(50);

                entity.Property(e => e.ProductCode)
                    .HasColumnName("Product_Code")
                    .HasMaxLength(50);

                entity.Property(e => e.ProductFeatures)
                    .HasColumnName("Product_Features")
                    .HasMaxLength(70);

                entity.Property(e => e.SalePrice).HasColumnName("Sale_Price");

                entity.Property(e => e.ShortDescription)
                    .HasColumnName("Short_Description")
                    .HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<SystemUser>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .HasColumnName("Created_By")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("Created_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DisplayName)
                    .HasColumnName("Display_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("Modified_By")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("Modified_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Role).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .HasColumnName("User_Name")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
