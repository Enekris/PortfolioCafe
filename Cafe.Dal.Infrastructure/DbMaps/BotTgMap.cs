using Cafe.Dal.Contracts.Repositories.Bot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cafe.Dal.Infrastructure.DbMaps
{

    internal class BotTgMap : IEntityTypeConfiguration<UserDataTgDb>
    {
        public void Configure(EntityTypeBuilder<UserDataTgDb> builder)
        {
            builder.HasKey(e => e.ChatId);

            builder.ToTable("UsersDataTg");

            builder.Property(e => e.ChatId)
                .ValueGeneratedNever()
                .HasColumnName("Chat_Id");

            builder.ToTable("UsersDataTg");
        }
    }
}
