using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DoAnFlutterAPI.Entities;

public partial class CafeDbContext : DbContext
{
    public CafeDbContext()
    {
    }

    public CafeDbContext(DbContextOptions<CafeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<DailyRevenue> DailyRevenues { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<ItemIngredient> ItemIngredients { get; set; }

    public virtual DbSet<KitchenQueue> KitchenQueues { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PrintLog> PrintLogs { get; set; }

    public virtual DbSet<StockTransaction> StockTransactions { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<TableSession> TableSessions { get; set; }

    public virtual DbSet<TabletConfig> TabletConfigs { get; set; }

    public virtual DbSet<VwKitchenQueue> VwKitchenQueues { get; set; }

    public virtual DbSet<VwLowStockIngredient> VwLowStockIngredients { get; set; }

    public virtual DbSet<VwMenuWithStock> VwMenuWithStocks { get; set; }

    public virtual DbSet<VwRevenueThisMonth> VwRevenueThisMonths { get; set; }

    public virtual DbSet<VwTableStatus> VwTableStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE488ECC273AE");

            entity.HasIndex(e => e.Username, "UQ__Admins__536C85E40463541E").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0BB1F01EB7");

            entity.Property(e => e.IconUrl).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<DailyRevenue>(entity =>
        {
            entity.HasKey(e => e.RevenueId).HasName("PK__DailyRev__275F16DD6C9FEA8A");

            entity.ToTable("DailyRevenue");

            entity.HasIndex(e => e.RevenueDate, "IX_DailyRevenue_Date").IsDescending();

            entity.HasIndex(e => e.RevenueDate, "UQ__DailyRev__489BB39F7291C380").IsUnique();

            entity.Property(e => e.TotalAmount).HasColumnType("decimal(14, 2)");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IngredientId).HasName("PK__Ingredie__BEAEB25AC9B908E2");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CurrentStock).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MinStock).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Unit).HasMaxLength(10);
        });

        modelBuilder.Entity<ItemIngredient>(entity =>
        {
            entity.HasKey(e => e.ItemIngredientId).HasName("PK__ItemIngr__D8B0DB398556C512");

            entity.HasIndex(e => new { e.MenuItemId, e.IngredientId }, "UQ_ItemIngredients").IsUnique();

            entity.Property(e => e.AmountPerServing).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.ItemIngredients)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemIngre__Ingre__5BE2A6F2");

            entity.HasOne(d => d.MenuItem).WithMany(p => p.ItemIngredients)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemIngre__MenuI__5AEE82B9");
        });

        modelBuilder.Entity<KitchenQueue>(entity =>
        {
            entity.HasKey(e => e.QueueId).HasName("PK__KitchenQ__8324E715921ED33F");

            entity.ToTable("KitchenQueue");

            entity.HasIndex(e => new { e.Status, e.OrderedAt }, "IX_KitchenQueue_Status_OrderedAt");

            entity.Property(e => e.ItemName).HasMaxLength(200);
            entity.Property(e => e.Note).HasMaxLength(300);
            entity.Property(e => e.OrderedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("waiting");
            entity.Property(e => e.TableCode).HasMaxLength(20);

            entity.HasOne(d => d.Order).WithMany(p => p.KitchenQueues)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KitchenQu__Order__01142BA1");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.KitchenQueues)
                .HasForeignKey(d => d.OrderItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KitchenQu__Order__02084FDA");
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.MenuItemId).HasName("PK__MenuItem__8943F7227980E2CE");

            entity.HasIndex(e => new { e.CategoryId, e.IsAvailable }, "IX_MenuItems_CategoryId_Available");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.MenuItems)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MenuItems__Categ__4CA06362");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF40B7BA8C");

            entity.HasIndex(e => new { e.TableId, e.Status }, "IX_Orders_TableId_Status");

            entity.HasIndex(e => e.ZaloPayOrderId, "IX_Orders_ZaloPayOrderId");

            entity.HasIndex(e => e.ZaloPayOrderId, "UQ__Orders__351C6EA0988B4C0F").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("preparing");
            entity.Property(e => e.TableCode).HasMaxLength(20);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.ZaloPayOrderId).HasMaxLength(200);

            entity.HasOne(d => d.Session).WithMany(p => p.Orders)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__SessionI__6C190EBB");

            entity.HasOne(d => d.Table).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__TableId__6D0D32F4");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__57ED068104D3A78D");

            entity.HasIndex(e => e.OrderId, "IX_OrderItems_OrderId");

            entity.Property(e => e.ItemName).HasMaxLength(200);
            entity.Property(e => e.ItemStatus)
                .HasMaxLength(20)
                .HasDefaultValue("pending");
            entity.Property(e => e.Note).HasMaxLength(300);
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.MenuItem).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__MenuI__73BA3083");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Order__72C60C4A");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A38A3347EFF");

            entity.HasIndex(e => e.ZaloPayOrderId, "IX_Payments_ZaloPayOrderId");

            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Method)
                .HasMaxLength(20)
                .HasDefaultValue("zalopay");
            entity.Property(e => e.PaidAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("success");
            entity.Property(e => e.ZaloPayOrderId).HasMaxLength(200);
            entity.Property(e => e.ZaloTransId).HasMaxLength(200);

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__OrderI__7A672E12");
        });

        modelBuilder.Entity<PrintLog>(entity =>
        {
            entity.HasKey(e => e.PrintLogId).HasName("PK__PrintLog__A3E8BE84B81D7606");

            entity.ToTable("PrintLog");

            entity.Property(e => e.ErrorMsg).HasMaxLength(500);
            entity.Property(e => e.PrintedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PrinterIp).HasMaxLength(50);
            entity.Property(e => e.Success).HasDefaultValue(true);

            entity.HasOne(d => d.Order).WithMany(p => p.PrintLogs)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PrintLog__OrderI__0D7A0286");
        });

        modelBuilder.Entity<StockTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__StockTra__55433A6B1E3AA9C3");

            entity.HasIndex(e => new { e.IngredientId, e.CreatedAt }, "IX_StockTransactions_IngredientId").IsDescending(false, true);

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Reason).HasMaxLength(300);
            entity.Property(e => e.TransType).HasMaxLength(10);

            entity.HasOne(d => d.Ingredient).WithMany(p => p.StockTransactions)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockTran__Ingre__5FB337D6");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("PK__Tables__7D5F01EE2602948D");

            entity.HasIndex(e => e.TableCode, "UQ__Tables__896A4323C03CF7BB").IsUnique();

            entity.Property(e => e.Capacity).HasDefaultValue(4);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Floor).HasDefaultValue(1);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.TableCode).HasMaxLength(20);
            entity.Property(e => e.TableName).HasMaxLength(100);
        });

        modelBuilder.Entity<TableSession>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__TableSes__C9F492909CF9650F");

            entity.Property(e => e.OpenedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("open");

            entity.HasOne(d => d.Table).WithMany(p => p.TableSessions)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TableSess__Table__656C112C");
        });

        modelBuilder.Entity<TabletConfig>(entity =>
        {
            entity.HasKey(e => e.ConfigId).HasName("PK__TabletCo__C3BC335CF929AC41");

            entity.ToTable("TabletConfig");

            entity.HasIndex(e => e.DeviceId, "UQ__TabletCo__49E1231099B47CAC").IsUnique();

            entity.Property(e => e.DeviceId).HasMaxLength(200);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.SetupAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Table).WithMany(p => p.TabletConfigs)
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("FK__TabletCon__Table__44FF419A");
        });

        modelBuilder.Entity<VwKitchenQueue>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_KitchenQueue");

            entity.Property(e => e.ItemName).HasMaxLength(200);
            entity.Property(e => e.Note).HasMaxLength(300);
            entity.Property(e => e.QueueId).ValueGeneratedOnAdd();
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TableCode).HasMaxLength(20);
        });

        modelBuilder.Entity<VwLowStockIngredient>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_LowStockIngredients");

            entity.Property(e => e.CurrentStock).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Deficit).HasColumnType("decimal(11, 2)");
            entity.Property(e => e.IngredientId).ValueGeneratedOnAdd();
            entity.Property(e => e.MinStock).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.StockStatus)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Unit).HasMaxLength(10);
        });

        modelBuilder.Entity<VwMenuWithStock>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_MenuWithStock");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.MaxServableByIngredients).HasColumnType("decimal(23, 0)");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<VwRevenueThisMonth>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_RevenueThisMonth");

            entity.Property(e => e.CumulativeAmount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(14, 2)");
        });

        modelBuilder.Entity<VwTableStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_TableStatus");

            entity.Property(e => e.Status)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.TableCode).HasMaxLength(20);
            entity.Property(e => e.TableName).HasMaxLength(100);
            entity.Property(e => e.TotalSpent).HasColumnType("decimal(38, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
