using Cafe.Dal.Contracts.Repositories.Dish.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cafe.Dal.Infrastructure.DbMaps
{
    internal class DishMap : IEntityTypeConfiguration<DishDb>
    {
        public void Configure(EntityTypeBuilder<DishDb> builder)
        {
            builder.HasKey(e => e.Id).HasName("Id");

            builder.HasIndex(e => e.ItemName).IsUnique().HasDatabaseName("ItemName");
            builder.Property(e => e.ItemName).HasMaxLength(50);

            builder.Property(x => x.Description).HasColumnName("Description");
            builder.Property(e => e.Description).HasMaxLength(50);

            builder.Property(e => e.Price).HasColumnName("Price");
            builder.Property(e => e.DataCreate).HasColumnName("DataCreate");

            builder.ToTable("Dishes");
        }
    }
}
