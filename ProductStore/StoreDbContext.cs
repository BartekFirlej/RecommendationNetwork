using Microsoft.EntityFrameworkCore;
using ProductStore.Models;

public partial class StoreDbContext : DbContext
{
    public StoreDbContext()
    {
    }

    public StoreDbContext(DbContextOptions<StoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }

    public virtual DbSet<PurchaseProposal> PurchaseProposals { get; set; }

    public virtual DbSet<Voivodeship> Voivodeships { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=Store;User=sa;Password=zaq1@WSX;Encrypt=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Customer_PK");

            entity.ToTable("Customer");

            entity.Property(e => e.Country)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Street)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Town)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.VoivodeshipId).HasColumnName("Voivodeship_Id");
            entity.Property(e => e.RecommenderId).HasColumnName("RecommenderId");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(6)
                .IsUnicode(false);

            entity.HasOne(d => d.Voivodeship).WithMany(p => p.Customers)
                .HasForeignKey(d => d.VoivodeshipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Customer_Voivodeship_FK");

            entity.HasOne(d => d.Recommender)
                .WithMany(p => p.RecommendedCustomers)
                .HasForeignKey(d => d.RecommenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Customer_Recommender_FK");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Product_PK");

            entity.ToTable("Product");

            entity.Property(e => e.ProductName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ProductTypeId).HasColumnName("ProductType_Id");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Product_ProductType_FK");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ProductType_PK");

            entity.ToTable("ProductType");

            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Purchase_PK");

            entity.ToTable("Purchase");

            entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");
            entity.Property(e => e.RecommenderId).HasColumnName("Recommender_Id");

            entity.HasOne(d => d.Customer).WithMany(p => p.PurchaseCustomers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Purchase_Customer_FKv2");

            entity.HasOne(d => d.Recommender).WithMany(p => p.PurchaseRecommenders)
                .HasForeignKey(d => d.RecommenderId)
                .HasConstraintName("Purchase_Customer_FK");
        });

        modelBuilder.Entity<PurchaseDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PurchaseDetail_PK");

            entity.ToTable("PurchaseDetail");

            entity.Property(e => e.Number).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.ProductId).HasColumnName("Product_Id");
            entity.Property(e => e.PurchaseId).HasColumnName("Purchase_Id");

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PurchaseDetail_Product_FK");

            entity.HasOne(d => d.Purchase).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PurchaseDetail_Purchase_FK");
        });

        modelBuilder.Entity<PurchaseProposal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Purchase_Proposal_PK");

            entity.ToTable("Purchase_Proposal");

            entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");
            entity.Property(e => e.ProductId).HasColumnName("Product_Id");

            entity.HasOne(d => d.Customer).WithMany(p => p.PurchaseProposals)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Purchase_Proposal_Customer_FK");

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseProposals)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Purchase_Proposal_Product_FK");
        });

        modelBuilder.Entity<Voivodeship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Voivodeship_PK");

            entity.ToTable("Voivodeship");

            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
