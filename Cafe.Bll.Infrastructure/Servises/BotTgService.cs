using AutoMapper;
using Cafe.Bll.Contracts.Servises.BotTg;
using Cafe.Bll.Contracts.Servises.BotTg.Models;
using Cafe.Dal.Contracts.Repositories.Bot;
using Cafe.Dal.Contracts.Repositories.Bot.Models;

namespace Cafe.Bll.Infrastructure.Servises
{
    internal class BotTgServise : IBotTgService
    {
        private readonly IBotTgRepositoty _botRepository;
        private readonly IMapper _mapper;



        public BotTgServise(IMapper mapper, IBotTgRepositoty botRepository)
        {
            _botRepository = botRepository;
            _mapper = mapper;
        }
        public void Create(UserDataTgDto entity)
        {
            var entityDb = new UserDataTgDb()
            {
                ChatId = entity.ChatId,
                Count = 0
            };
            _botRepository.Create(entityDb);
        }
        public List<UserDataTgDto> GetAll()
        {
            List<UserDataTgDb> userDataDb = _botRepository.GetAll();
            var userDataDto = _mapper.Map<List<UserDataTgDto>>(userDataDb);
            return userDataDto;
        }

        public UserDataTgDto GetEntity(long chatId)
        {
            var userDataDb = _botRepository.GetEntity(chatId);
            UserDataTgDto userDataDto = _mapper.Map<UserDataTgDto>(userDataDb);
            return userDataDto;

        }

        public bool CountVerification(long chatId, double count)
        {
            return _botRepository.CountVerification(chatId, count);
        }

        public void Update(UserDataTgDto entity, double count)
        {
            var userDataDb = _botRepository.GetEntity(entity.ChatId);
            _botRepository.Update(userDataDb, count);
        }
    }



}

