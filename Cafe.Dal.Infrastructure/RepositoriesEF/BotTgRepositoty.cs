using Cafe.Dal.Contracts.Repositories.Bot;
using Cafe.Dal.Contracts.Repositories.Bot.Models;
using Cafe.Dal.Infrastructure.DBSettingsEF;


namespace Cafe.Dal.Infrastructure.RepositoriesEF
{
    internal class BotTgRepositoty : IBotTgRepositoty
    {
        public void Update(UserDataTgDb entity, double count)
        {
            entity.Count = count;
            using BotTgContext db = new BotTgContext();
            db.UsersDataTg.Update(entity);
            db.SaveChanges();

        }

        public void Create(UserDataTgDb entity)
        {
            using BotTgContext db = new BotTgContext();
            db.UsersDataTg.Add(new UserDataTgDb()
            {
                ChatId = entity.ChatId,
                Count = 0
            });
            db.SaveChanges();
        }
        public List<UserDataTgDb> GetAll()
        {
            using BotTgContext db = new BotTgContext();
            return db.UsersDataTg.ToList();
        }

        public UserDataTgDb GetEntity(long chatId)
        {
            using BotTgContext db = new BotTgContext();
            return db.UsersDataTg.Find(chatId);
        }

        public bool CountVerification(long chatId, double count)
        {
            if ((GetEntity(chatId)?.ChatId == chatId) && (GetEntity(chatId).Count == count))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
