using Cafe.Dal.Contracts.Repositories.OrderDish.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cafe.Dal.Infrastructure.DbMaps
{
    internal class OrdersDishMap : IEntityTypeConfiguration<OrdersDishDb>
    {
        public void Configure(EntityTypeBuilder<OrdersDishDb> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__OrdersDi__3214EC076E270CAB");

            builder.Property(e => e.DishId).HasColumnName("Dish_id");
            builder.Property(e => e.DishPriceOnOrdersDate).HasColumnName("DishPriceOnOrdersDate");
            builder.Property(e => e.OrderDate).HasColumnName("Order_date");
            builder.Property(e => e.OrderId).HasColumnName("Order_id");

            builder.HasOne(d => d.Dish).WithMany(p => p.OrdersDishes)
                .HasForeignKey(d => d.DishId)
                .HasConstraintName("FK__OrdersDis__Dish___69FBBC1F");

            builder.HasOne(d => d.Order).WithMany(p => p.OrdersDishes)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrdersDis__DishP__690797E6");

            builder.ToTable("OrdersDishes");
        }
    }
}
