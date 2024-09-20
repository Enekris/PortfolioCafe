using Cafe.Dal.Contracts.Repositories.Order.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cafe.Dal.Infrastructure.DbMaps
{
    internal class OrderMap : IEntityTypeConfiguration<OrderDb>
    {
        public void Configure(EntityTypeBuilder<OrderDb> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Orders__3214EC07793E0129");

            builder.Property(e => e.CloseDate).HasColumnName("Close_date");
            builder.Property(e => e.CustomerId).HasColumnName("Customer_id");
            builder.Property(e => e.OrderDate).HasColumnName("Order_date");
            builder.Property(e => e.TableId).HasColumnName("Table_id");

            builder.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Orders__Customer__114A936A");

            builder.HasOne(d => d.Table).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("FK__Orders__Tables_i__123EB7A3");

            builder.ToTable("Orders");
        }
    }
}
