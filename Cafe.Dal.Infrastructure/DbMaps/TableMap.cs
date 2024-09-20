using Cafe.Dal.Contracts.Repositories.Table.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cafe.Dal.Infrastructure.DbMaps
{
    internal class TableMap : IEntityTypeConfiguration<TableDb>
    {
        public void Configure(EntityTypeBuilder<TableDb> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Tables__3214EC073EB625F0");

            builder.HasIndex(e => e.Number, "UQ__Tables__78A1A19D5DB70977").IsUnique();

            builder.Property(e => e.ReservedCustomerId).HasColumnName("Reserved_Customer_Id");

            builder.ToTable("Tables");
        }
    }
}
