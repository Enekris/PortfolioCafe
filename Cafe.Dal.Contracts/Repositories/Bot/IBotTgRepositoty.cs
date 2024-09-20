using Cafe.Dal.Contracts.Repositories.Bot.Models;

namespace Cafe.Dal.Contracts.Repositories.Bot
{
    public interface IBotTgRepositoty
    {
        public void Update(UserDataTgDb entity, double count);


        public void Create(UserDataTgDb entity);

        public List<UserDataTgDb> GetAll();


        public UserDataTgDb GetEntity(long chatId);


        public bool CountVerification(long chatId, double count);
    }
}
