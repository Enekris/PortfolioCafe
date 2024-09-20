using Cafe.Dal.Contracts.Repositories.Customer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cafe.Dal.Infrastructure.DbMaps
{
    internal class CustomerMap : IEntityTypeConfiguration<CustomerDb>
    {
        public void Configure(EntityTypeBuilder<CustomerDb> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Customer__3214EC079C6F0AE3");

            builder.HasIndex(e => e.Phone, "UQ__Customer__5C7E359E41EBE8F4").IsUnique();

            builder.HasIndex(e => e.Email, "UQ__Customer__A9D1053487236DEB").IsUnique();

            builder.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false);
            builder.Property(e => e.FirstName).HasMaxLength(20);
            builder.Property(e => e.LastName).HasMaxLength(20);
            builder.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.ToTable("Customers");
        }
    }
}
