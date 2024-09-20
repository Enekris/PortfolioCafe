using Cafe.Bll.Contracts.Servises.BotTg.Models;

namespace Cafe.Bll.Contracts.Servises.BotTg
{
    public interface IBotTgService
    {
        public void Update(UserDataTgDto entity, double count);


        public void Create(UserDataTgDto entity);

        public List<UserDataTgDto> GetAll();


        public UserDataTgDto GetEntity(long chatId);

        public bool CountVerification(long chatId, double count);

    }
}
