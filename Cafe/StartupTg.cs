using Cafe.Bll.Contracts.Servises.BotTg;
using Cafe.Bll.Contracts.Servises.BotTg.Models;
using Cafe.Bll.Contracts.Servises.Customer;
using Cafe.Bll.Contracts.Servises.Customer.Models;
using Cafe.Bll.Contracts.Servises.Dish.Models;
using Cafe.Bll.Contracts.Servises.Menu;
using Cafe.Bll.Contracts.Servises.Order;
using Cafe.Bll.Contracts.Servises.Order.Models;
using Cafe.Bll.Contracts.Servises.Table;
using Cafe.Bll.Contracts.Servises.Table.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Cafe
{
    internal class StartupTg
    {
        private int idTable { get; set; }
        private int age { get; set; }
        private string firstName { get; set; }
        private string lastName { get; set; }
        private string email { get; set; }
        private string phone { get; set; }
        private int idCustomer { get; set; }
        private string dishes { get; set; }
        private int idOrder { get; set; }
        private string itemname { get; set; }
        private string description { get; set; }
        private int price { get; set; }
        private int idDish { get; set; }
        private int seats { get; set; }
        private int tableNumber { get; set; }

        private static ITelegramBotClient _botClient;

        private static ReceiverOptions _receiverOptions;

        private readonly IBotTgService _botTgServise;
        private readonly IMenuServise _menuServise;
        private readonly ITableServise _tableServise;
        private readonly IOrderServise _orderServise;
        private readonly ICustomerServise _customerServise;

        public StartupTg(
            IBotTgService botTgServise,
            IMenuServise menuServise,
            ITableServise tableServise,
            ICustomerServise custimerServise,
            IOrderServise orderServise)
        {
            _botTgServise = botTgServise;
            _menuServise = menuServise;
            _tableServise = tableServise;
            _orderServise = orderServise;
            _customerServise = custimerServise;
        }

        public async Task Main()
        {

            _botClient = new TelegramBotClient("6523037254:AAGrj8SNpcMTlHN3pA-XWWYi-NJ2sMvhznY");
            _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]
            {
                UpdateType.Message,
                UpdateType.CallbackQuery
            },

                ThrowPendingUpdates = true,
            };

            using var cts = new CancellationTokenSource();


            _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token);

            var me = await _botClient.GetMeAsync();
            Console.WriteLine($"{me.FirstName} запущен!");

            await Task.Delay(-1);
        }


        private async Task UpdateHandler(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        {

            List<UserDataTgDto> users = _botTgServise.GetAll();
            var message = update.Message;
            var chat = message.Chat;
            try
            {

                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            var user = message.From;
                            Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");
                            switch (message.Type)
                            {
                                case MessageType.Text:
                                    {
                                        if (message.Text == "/start" || message.Text == "Вернуться в главное меню")
                                        {
                                            if (_botTgServise.GetEntity(chat.Id) == null)
                                            {
                                                _botTgServise.Create(new UserDataTgDto()
                                                {
                                                    ChatId = chat.Id
                                                });
                                            }
                                            else
                                            {
                                                _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            }
                                            var replyKeyboard = CreateKeyboard(
                                             new string[] { "Чек сервис", "Меню сервис" },
                                             new string[] { "Столик сервис", "Посетитель  сервис" });
                                            await botClient.SendTextMessageAsync(
                                                chat.Id,
                                                "Привет! Я - бот управления кафе, выберите нужное действие.",
                                                replyMarkup: replyKeyboard);

                                            return;
                                        }


                                        else if (_botTgServise.CountVerification(chat.Id, 0) && (message.Text == "Чек сервис" || message.Text == "Вернуться в Чек сервис")) //"Чек сервис"
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1);
                                            var replyKeyboard = CreateKeyboard(
                                                    new string[] { "Открыть счет", "Посмотреть все счета" },
                                                    new string[] { "Добавить блюдо(а) в счет", "Проверить состояние счета" },
                                                    new string[] { "Закрыть счет", "Выдать счет" },
                                            new string[] { "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;

                                        }

                                        //Открыть счет
                                        else if (_botTgServise.CountVerification(chat.Id, 1) && (message.Text == "Открыть счет"))
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1.1);
                                            var replyKeyboard = CreateKeyboard(
                                                   new string[] { "Добавить нового покупателя", "Добавить существующего покупателя по ID" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;
                                        }
                                        //Добавить нового покупателя
                                        else if (_botTgServise.CountVerification(chat.Id, 1.1) && message.Text == "Добавить нового покупателя")
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1.1101);
                                            await botClient.SendTextMessageAsync(chat.Id, ShowTablesInfo());
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите ID столика:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 1.1101))
                                        {
                                            idTable = Convert.ToInt32(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1.1102);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите возраст:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 1.1102))
                                        {
                                            age = Convert.ToInt32(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1.1103);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите имя:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 1.1103))
                                        {
                                            firstName = message.Text;
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1.1104);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите фамилию:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 1.1104))
                                        {
                                            lastName = message.Text;
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1.1105);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите email:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 1.1105))
                                        {
                                            email = message.Text;
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1.1106);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите телефон:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 1.1106))
                                        {
                                            phone = message.Text;
                                            var customer = new CustomerDto(age, firstName, lastName, email, phone);
                                            try
                                            {
                                                _customerServise.Create(customer);
                                                _orderServise.Create(new OrderDto(customer.Id, idTable));
                                            }
                                            catch (Exception ex)
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, $"ошибка при вставке данных в бд: \n {ex.Message}");
                                                _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1);
                                                return;
                                            }
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Чек сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;

                                        }


                                        //Добавить существующего покупателя по ID
                                        else if (_botTgServise.CountVerification(chat.Id, 1.1) && message.Text == "Добавить существующего покупателя по ID")
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1.1201);
                                            await botClient.SendTextMessageAsync(chat.Id, ShowNotReservedTables());
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите ID столика:");
                                            return;

                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 1.1201))
                                        {
                                            idTable = Convert.ToInt32(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1.1202);
                                            await botClient.SendTextMessageAsync(chat.Id, ShowCustomersInfo());
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите ID покупателя:");
                                            return;

                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 1.1202))
                                        {
                                            idCustomer = Convert.ToInt32(message.Text);
                                            try
                                            {

                                                _orderServise.Create(new OrderDto(idCustomer, idTable)); ;
                                            }
                                            catch (Exception ex)
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, $"ошибка при вставке данных в бд: \n {ex.Message}");
                                                _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1);
                                                return;
                                            }
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            var replyKeyboard = CreateKeyboard(
                                                   new string[] { "Вернуться в Чек сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;
                                        }


                                        //посмотреть все счета
                                        else if (_botTgServise.CountVerification(chat.Id, 1) && message.Text == "Посмотреть все счета")
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, ShowOrdersInfo());
                                            return;
                                        }
                                        //Добавить блюдо в счет
                                        else if (_botTgServise.CountVerification(chat.Id, 1) && message.Text == "Добавить блюдо(а) в счет")
                                        {
                                            if (ShowOpenOrdersInfo() != "Все заказы закрыты")
                                            {
                                                _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1.3);
                                                await botClient.SendTextMessageAsync(chat.Id, ShowOpenOrdersInfo());
                                                await botClient.SendTextMessageAsync(chat.Id, "Введите ID заказа:");
                                                return;
                                            }
                                            else
                                            {
                                                _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                                await botClient.SendTextMessageAsync(chat.Id, ShowOpenOrdersInfo());
                                                var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Чек сервис", "Вернуться в главное меню" });
                                                await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                                return;
                                            }

                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 1.3))
                                        {
                                            idOrder = Convert.ToInt32(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1.301);
                                            await botClient.SendTextMessageAsync(chat.Id, ShowMenuInfo());
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите ID блюд через запятую без пробелов:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 1.301))
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            dishes = Convert.ToString(message.Text);
                                            string[] idDishesString = dishes.Split(new char[] { ',' });
                                            int[] idDishes = new int[idDishesString.Length];
                                            for (int i = 0; i < idDishes.Length; i++)
                                            {
                                                idDishes[i] = Convert.ToInt32(idDishesString[i]);
                                            }
                                            _orderServise.AddToOrder(idOrder, idDishes);
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Чек сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;
                                        }

                                        //Проверить состояние счета
                                        else if (_botTgServise.CountVerification(chat.Id, 1) && message.Text == "Проверить состояние счета")
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1.4);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите ID заказа:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 1.4))
                                        {
                                            idOrder = Convert.ToInt32(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            if (_orderServise.IsOrderClosed(idOrder))
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, "Чек закрыт");
                                            }
                                            else
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, "Чек не закрыт");
                                            }
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Чек сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;
                                        }

                                        //Закрыть счет
                                        else if (_botTgServise.CountVerification(chat.Id, 1) && message.Text == "Закрыть счет")
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1.5);
                                            await botClient.SendTextMessageAsync(chat.Id, ShowOpenOrdersInfo());
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите ID заказа:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 1.5))
                                        {
                                            idOrder = Convert.ToInt32(message.Text);
                                            _orderServise.CloseOrder(idOrder);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            await botClient.SendTextMessageAsync(chat.Id, "Счет закрыт");
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Чек сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;

                                        }

                                        //Выдать счет
                                        else if (_botTgServise.CountVerification(chat.Id, 1) && message.Text == "Выдать счет")
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 1.6);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите ID заказа:");
                                            return;

                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 1.6))
                                        {
                                            idOrder = Convert.ToInt32(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            if (_orderServise.IsOrderClosed(idOrder))
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, _orderServise.Check(idOrder));
                                            }
                                            else
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, "Сначала закройте чек");
                                            }
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Чек сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;

                                        }

                                        else if (_botTgServise.CountVerification(chat.Id, 0) && (message.Text == "Меню сервис" || message.Text == "Вернуться в Меню сервис")) //"Меню сервис"
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 2);
                                            var replyKeyboard = CreateKeyboard(
                                                   new string[] { "Посмотреть меню", "Создать блюдо" },
                                                   new string[] { "Изменить цену всех блюд на процент", "Изменить цену блюда" },
                                                   new string[] { "Изменить все параметры блюда", "Удалить блюдо" },
                                            new string[] { "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;

                                        }
                                        //Посмотреть меню
                                        else if (_botTgServise.CountVerification(chat.Id, 2) && message.Text == "Посмотреть меню")
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, ShowMenuInfo());
                                            return;

                                        }

                                        //Создать блюдо
                                        else if (_botTgServise.CountVerification(chat.Id, 2) && message.Text == "Создать блюдо")
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 2.2);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите название:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 2.2))
                                        {
                                            itemname = Convert.ToString(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 2.201);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите описание:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 2.201))
                                        {
                                            description = Convert.ToString(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 2.202);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите цену:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 2.202))
                                        {
                                            price = Convert.ToInt32(message.Text);

                                            try
                                            {
                                                _menuServise.Create(new DishDto(itemname, description, price));
                                            }
                                            catch (Exception ex)
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, $"ошибка при вставке данных в бд: \n {ex.Message}");
                                                _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 2);
                                                return;
                                            }
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            await botClient.SendTextMessageAsync(chat.Id, "Блюдо создано");
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Меню сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;
                                        }

                                        //Изменить цену всех блюд на процент
                                        else if (_botTgServise.CountVerification(chat.Id, 2) && message.Text == "Изменить цену всех блюд на процент")
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 2.3);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите процент:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 2.3))
                                        {
                                            int persent = Convert.ToInt32(message.Text);
                                            _menuServise.UpdateAllPricePercent(persent);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            await botClient.SendTextMessageAsync(chat.Id, "Цена изменена");
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Меню сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;
                                        }

                                        //Изменить цену блюда
                                        else if (_botTgServise.CountVerification(chat.Id, 2) && message.Text == "Изменить цену блюда")
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, ShowMenuInfo());
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 2.4);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите ID блюда:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 2.4))
                                        {
                                            idDish = Convert.ToInt32(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 2.401);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите новую цену блюда:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 2.401))
                                        {
                                            price = Convert.ToInt32(message.Text);
                                            _menuServise.UpdatePrice(idDish, price);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            await botClient.SendTextMessageAsync(chat.Id, "Цена изменена");
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Меню сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;
                                        }

                                        //Изменить все параметры блюда
                                        else if (_botTgServise.CountVerification(chat.Id, 2) && message.Text == "Изменить все параметры блюда")
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 2.5);
                                            await botClient.SendTextMessageAsync(chat.Id, ShowMenuInfo());
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите ID блюда:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 2.5))
                                        {
                                            idDish = Convert.ToInt32(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 2.501);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите новое название:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 2.501))
                                        {
                                            itemname = Convert.ToString(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 2.502);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите новое описание:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 2.502))
                                        {
                                            description = Convert.ToString(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 2.503);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите новую цену:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 2.503))
                                        {
                                            price = Convert.ToInt32(message.Text);
                                            _menuServise.UpdateAllParams(idDish, itemname, description, price);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            await botClient.SendTextMessageAsync(chat.Id, "Параметры измненены");
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Меню сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;
                                        }

                                        //Удалить блюдо
                                        else if (_botTgServise.CountVerification(chat.Id, 2) && message.Text == "Удалить блюдо")
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 2.6);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите ID блюда:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 2.6))
                                        {
                                            idDish = Convert.ToInt32(message.Text);
                                            _menuServise.Delete(idDish);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            await botClient.SendTextMessageAsync(chat.Id, "Блюдо удалено");
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Меню сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;
                                        }

                                        else if (_botTgServise.CountVerification(chat.Id, 0) && (message.Text == "Столик сервис" || message.Text == "Вернуться в Столик сервис")) //Столик сервис
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3);
                                            var replyKeyboard = CreateKeyboard(
                                                   new string[] { "Создать столик", "Показать список столиков" },
                                                   new string[] { "Изменить все параметры столика", "Установить резерв" },
                                                   new string[] { "Удалить резерв", "Удалить столик" },
                                            new string[] { "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;

                                        }
                                        //Создать столик
                                        else if (_botTgServise.CountVerification(chat.Id, 3) && message.Text == "Создать столик")
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3.1);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите номер столика:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 3.1))
                                        {
                                            tableNumber = Convert.ToInt32(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3.101);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите колличество мест:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 3.101))
                                        {

                                            seats = Convert.ToInt32(message.Text);

                                            try
                                            {
                                                _tableServise.Create(new TableDto(tableNumber, seats));
                                            }
                                            catch (Exception ex)
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, $"ошибка при вставке данных в бд: \n {ex.Message}");
                                                _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3);
                                                return;
                                            }
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            await botClient.SendTextMessageAsync(chat.Id, "Столик создан");
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Столик сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;
                                        }

                                        //Показать список столиков
                                        else if (_botTgServise.CountVerification(chat.Id, 3) && message.Text == "Показать список столиков")
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, ShowTablesInfo());
                                            return;
                                        }

                                        //Изменить все параметры столика
                                        else if (_botTgServise.CountVerification(chat.Id, 3) && message.Text == "Изменить все параметры столика")
                                        {

                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3.3);
                                            await botClient.SendTextMessageAsync(chat.Id, ShowTablesInfo());
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите ID существующего столика:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 3.3))
                                        {
                                            idTable = Convert.ToInt32(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3.301);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите новый номер столика:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 3.301))
                                        {
                                            tableNumber = Convert.ToInt32(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3.302);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите новое колличество мест:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 3.302))
                                        {
                                            seats = Convert.ToInt32(message.Text);
                                            _tableServise.UpdateAllParams(idTable, tableNumber, seats);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            await botClient.SendTextMessageAsync(chat.Id, "Столик изменен");
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Столик сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;
                                        }

                                        //Установить резерв
                                        else if (_botTgServise.CountVerification(chat.Id, 3) && message.Text == "Установить резерв")
                                        {
                                            if (ShowNotReservedTables() != "Все столики заняты")
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, ShowNotReservedTables());
                                                _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3.4);
                                                await botClient.SendTextMessageAsync(chat.Id, "Введите ID существующего столика:");
                                                return;
                                            }
                                            else
                                            {
                                                _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                                await botClient.SendTextMessageAsync(chat.Id, ShowNotReservedTables());
                                                var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Столик сервис", "Вернуться в главное меню" });
                                                await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            }
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 3.4))
                                        {
                                            idTable = Convert.ToInt32(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3.401);
                                            var replyKeyboard = CreateKeyboard(
                                                           new string[] { "Добавить нового покупателя", "Добавить существующего покупателя по ID" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 3.401) && message.Text == "Добавить нового покупателя")
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3.402);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите возраст:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 3.402))
                                        {
                                            age = Convert.ToInt32(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3.403);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите имя:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 3.403))
                                        {
                                            firstName = message.Text;
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3.404);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите фамилию:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 3.404))
                                        {
                                            lastName = message.Text;
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3.405);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите email:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 3.405))
                                        {
                                            email = message.Text;
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3.406);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите телефон:");

                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 3.406))
                                        {
                                            phone = message.Text;
                                            CustomerDto customer = new CustomerDto(age, firstName, lastName, email, phone);
                                            try
                                            {
                                                _customerServise.Create(customer);
                                                _tableServise.SetReserved(idTable, _customerServise.GetEntity(customer.Id));
                                            }
                                            catch (Exception ex)
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, $"ошибка при вставке данных в бд: \n {ex.Message}");
                                                _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3);
                                                return;
                                            }
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            await botClient.SendTextMessageAsync(chat.Id, "Резерв установлен");
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Столик сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);

                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 3.401) && message.Text == "Добавить существующего покупателя по ID")
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3.40101);
                                            await botClient.SendTextMessageAsync(chat.Id, ShowCustomersInfo());
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите ID покупателя:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 3.40101))
                                        {
                                            idCustomer = Convert.ToInt32(message.Text);
                                            _tableServise.SetReserved(idTable, _customerServise.GetEntity(idCustomer));
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            await botClient.SendTextMessageAsync(chat.Id, "Резерв установлен");
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Столик сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;
                                        }

                                        //Удалить резерв
                                        else if (_botTgServise.CountVerification(chat.Id, 3) && message.Text == "Удалить резерв")
                                        {
                                            if (ShowNotReservedTables() != "Все столики свободны")
                                            {
                                                _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3.5);
                                                await botClient.SendTextMessageAsync(chat.Id, ShowReservedTables());
                                                await botClient.SendTextMessageAsync(chat.Id, "Введите ID столика:");
                                                return;
                                            }
                                            else
                                            {
                                                _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                                await botClient.SendTextMessageAsync(chat.Id, ShowReservedTables());
                                                var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Столик сервис", "Вернуться в главное меню" });
                                                await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            }
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 3.5))
                                        {
                                            idTable = Convert.ToInt32(message.Text);
                                            _tableServise.DeteteReserved(idTable);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            await botClient.SendTextMessageAsync(chat.Id, "Резерв удален");
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Столик сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;
                                        }

                                        //Удалить столик
                                        else if (_botTgServise.CountVerification(chat.Id, 3) && message.Text == "Удалить столик")
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 3.6);
                                            await botClient.SendTextMessageAsync(chat.Id, ShowNotReservedTables());
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите ID столика:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 3.6))
                                        {
                                            idTable = Convert.ToInt32(message.Text);
                                            _tableServise.Delete(_tableServise.GetEntity(idTable));
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            await botClient.SendTextMessageAsync(chat.Id, "Столик удален");
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Столик сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;
                                        }

                                        else if (_botTgServise.CountVerification(chat.Id, 0) && (message.Text == "Посетитель  сервис" || message.Text == "Вернуться в Посетитель сервис")) // Посетитель  сервис
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 4);

                                            var replyKeyboard = CreateKeyboard(
                                                   new string[] { "Создать посетителя", "Показать список посетителей" },
                                                   new string[] { "Изменить все параметры посетителя", "Удалить посетителя" },
                                            new string[] { "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);
                                            return;
                                        }

                                        //Создать посетителя
                                        else if (_botTgServise.CountVerification(chat.Id, 4) && message.Text == "Создать посетителя")
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 4.1);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите возраст:");
                                            return;
                                        }

                                        else if (_botTgServise.CountVerification(chat.Id, 4.1))
                                        {
                                            age = Convert.ToInt32(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 4.101);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите имя:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 4.101))
                                        {
                                            firstName = message.Text;
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 4.102);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите фамилию:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 4.102))
                                        {
                                            lastName = message.Text;
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 4.103);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите email:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 4.103))
                                        {
                                            email = message.Text;
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 4.104);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите телефон:");

                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 4.104))
                                        {
                                            phone = message.Text;
                                            var customer = new CustomerDto(age, firstName, lastName, email, phone);
                                            try
                                            {
                                                _customerServise.Create(customer);

                                            }
                                            catch (Exception ex)
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, $"ошибка при вставке данных в бд: \n {ex.Message}");
                                                _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 4);
                                                return;
                                            }

                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            await botClient.SendTextMessageAsync(chat.Id, "Посетитель добавлен");
                                            var replyKeyboard = CreateKeyboard(
                                                  new string[] { "Вернуться в Посетитель сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);

                                            return;
                                        }

                                        //Показать список посетителей
                                        else if (_botTgServise.CountVerification(chat.Id, 4) && message.Text == "Показать список посетителей")
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, ShowCustomersInfo());
                                            return;
                                        }
                                        //Изменить все параметры посетителя
                                        else if (_botTgServise.CountVerification(chat.Id, 4) && message.Text == "Изменить все параметры посетителя")
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 4.3);
                                            await botClient.SendTextMessageAsync(chat.Id, ShowCustomersInfo());
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите ID существующего посетителя:");
                                            return;

                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 4.3))
                                        {
                                            idCustomer = Convert.ToInt32(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 4.301);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите новый возраст:");
                                            return;
                                        }

                                        else if (_botTgServise.CountVerification(chat.Id, 4.301))
                                        {
                                            age = Convert.ToInt32(message.Text);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 4.302);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите новое имя:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 4.302))
                                        {
                                            firstName = message.Text;
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 4.303);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите новую фамилию:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 4.303))
                                        {
                                            lastName = message.Text;
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 4.304);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите новый email:");
                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 4.304))
                                        {
                                            email = message.Text;
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 4.305);
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите новый телефон:");

                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 4.305))
                                        {
                                            phone = message.Text;
                                            _customerServise.UpdateAllParams(idCustomer, age, firstName, lastName, email, phone);
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            await botClient.SendTextMessageAsync(chat.Id, "Посетитель изменен");
                                            var replyKeyboard = CreateKeyboard(
                                                 new string[] { "Вернуться в Посетитель сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);

                                            return;
                                        }
                                        //Удалить посетителя
                                        else if (_botTgServise.CountVerification(chat.Id, 4) && message.Text == "Удалить посетителя")
                                        {
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 4.4);
                                            await botClient.SendTextMessageAsync(chat.Id, ShowCustomersInfo());
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите ID существующего посетителя:");

                                            return;
                                        }
                                        else if (_botTgServise.CountVerification(chat.Id, 4.4))
                                        {
                                            idCustomer = Convert.ToInt32(message.Text);
                                            _customerServise.Delete(_customerServise.GetEntity(idCustomer));
                                            _botTgServise.Update(_botTgServise.GetEntity(chat.Id), 0);
                                            await botClient.SendTextMessageAsync(chat.Id, "Посетитель удален");
                                            var replyKeyboard = CreateKeyboard(
                                                 new string[] { "Вернуться в Посетитель сервис", "Вернуться в главное меню" });
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите дальнейшее действие", replyMarkup: replyKeyboard);

                                            return;
                                        }

                                        return;
                                    }

                                default:
                                    {
                                        await botClient.SendTextMessageAsync(chat.Id, "Используй только текст!");
                                        return;
                                    }
                            }

                            return;
                        }


                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(chat.Id, "Какая то важная ошибка. отправь перешли мне че он написал ниже и что ты тыкал, старт скорее всего заного придется нажать. Не надо только в инт пытаться стринг вписать и тд");
                await botClient.SendTextMessageAsync(chat.Id, ex.Message + "\n" + ex.InnerException);
                Console.WriteLine(ex.ToString());
            }
        }
        private static ReplyKeyboardMarkup CreateKeyboard(params string[][] buttonNames)
        {
            var buttons = new List<KeyboardButton[]>();

            foreach (var item in buttonNames)
            {
                var row = new KeyboardButton[item.Length];
                for (int i = 0; i < item.Length; i++)
                {
                    row[i] = new KeyboardButton(item[i]);
                }
                buttons.Add(row);
            }

            var keyboardMarkup = new ReplyKeyboardMarkup(buttons.ToArray())
            {
                ResizeKeyboard = true
            };


            return keyboardMarkup;
        }

        private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
        public string ShowTablesInfo()
        {
            string showTablesInfo = "";

            foreach (TableDto item in _tableServise.GetALLTables())
            {
                showTablesInfo += "Id: " + item.Id + "\t" + "Номер столика: " + item.Number +
                    "\t" + "Колличество мест: " + item.Seats + "\t" + "Зарезерврованно: " + item.Reserved + "\t" + "ID посетителя: " + item.ReservedCustomerId + "\n";
            }
            return showTablesInfo;
        }
        public string ShowMenuInfo()
        {

            string showMenuInfo = "";
            Console.WriteLine("Меню: EAT" + "\t" + "под номером: 1 ");
            foreach (DishDto item in _menuServise.GetAllDishes())
            {
                showMenuInfo += "Id:" + item.Id + "\t" + item.ItemName + "\t" + item.Description + "\t" + item.Price + "\t" + item.DataCreate + "\n";

            }
            return showMenuInfo;
        }
        public string ShowOrdersInfo()
        {
            string showOrdersInfo = "";
            foreach (OrderDto item in _orderServise.GetAllOrders())
            {
                showOrdersInfo += "Id:" + item.Id + "\t" + item.CustomerId + "\t" + item.TableId + "\t" + item.OrderDate + "\t" + item.CloseDate + "\t" + item.Total + "\n";

            }
            return showOrdersInfo;
        }
        public string ShowOpenOrdersInfo()
        {
            string showOpenOrdersInfo = "";
            var orders = _orderServise.GetAllOpenOrders();
            if (orders.Count != 0)
            {
                foreach (OrderDto item in orders)
                {
                    showOpenOrdersInfo += "Id:" + item.Id + "\t" + item.CustomerId + "\t" + item.TableId + "\t" + item.OrderDate + "\t" + item.CloseDate + "\t" + item.Total + "\n";

                }
                return showOpenOrdersInfo;

            }
            else
            {
                return "Все заказы закрыты";
            }


        }

        public string ShowCustomersInfo()
        {
            string showCustomersInfo = "";
            foreach (CustomerDto item in _customerServise.GetALLCustomers())
            {
                showCustomersInfo += "Id: " + item.Id + "\t" + item.Age + "\t" + item.FirstName + "\t"
                    + item.LastName + "\t" + item.Email + "\t" + item.Phone + "\t" + item.DataCreate + "\n";

            }
            return showCustomersInfo;
        }


        public string ShowReservedTables()
        {
            string showReservedTables = "";
            var tables = _tableServise.GetAllReservedTables();
            if (tables.Count != 0)
            {
                foreach (TableDto item in tables)
                {
                    showReservedTables += "Id: " + item.Id + "\t" + "Номер столика: " + item.Number +
                        "\t" + "Колличество мест: " + item.Seats + "\t" + "Зарезерврованно: " + item.Reserved + "\t" + "ID посетителя: " + item.ReservedCustomerId + "\n";

                }
                return showReservedTables;
            }
            else
            {

                return "Все столики свободны";
            }

        }
        public string ShowNotReservedTables()
        {
            string showNotReservedTables = "";
            var tables = _tableServise.GetAllNotReservedTables();
            if (tables.Count != 0)
            {
                foreach (TableDto item in tables)
                {
                    showNotReservedTables += "Id: " + item.Id + "\t" + "Номер столика: " + item.Number +
                        "\t" + "Колличество мест: " + item.Seats + "\t" + "Зарезерврованно: " + item.Reserved + "\n";

                }
                return showNotReservedTables;
            }
            else
            {

                return "Все столики заняты";
            }

        }

    }
}






