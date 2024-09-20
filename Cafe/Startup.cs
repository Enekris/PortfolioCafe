using Cafe.Bll.Contracts.Servises.Customer;
using Cafe.Bll.Contracts.Servises.Customer.Models;
using Cafe.Bll.Contracts.Servises.Dish.Models;
using Cafe.Bll.Contracts.Servises.Menu;
using Cafe.Bll.Contracts.Servises.Order;
using Cafe.Bll.Contracts.Servises.Order.Models;
using Cafe.Bll.Contracts.Servises.Table;
using Cafe.Bll.Contracts.Servises.Table.Models;

namespace Cafe
{
    public class Startup
    {
        private readonly IMenuServise _menuServise;
        private readonly ITableServise _tableServise;
        private readonly IOrderServise _orderServise;
        private readonly ICustomerServise _customerServise;

        public Startup(
            IMenuServise menuServise,
            ITableServise tableServise,
            ICustomerServise custimerServise,
            IOrderServise orderServise)
        {
            _menuServise = menuServise;
            _tableServise = tableServise;
            _orderServise = orderServise;
            _customerServise = custimerServise;
        }
        public void Main()
        {

            Console.WriteLine("1 - Чек сервис");
            Console.WriteLine("2 - Меню сервис");
            Console.WriteLine("3 - Столик сервис");
            Console.WriteLine("4 - Посетитель  сервис");
            int value = Convert.ToInt32(Console.ReadLine());
            switch (value)
            {
                case 1:
                    Console.WriteLine("1 - Открыть счет");
                    Console.WriteLine("2 - Посмотреть все счета");
                    Console.WriteLine("3 - Добавить блюдо(а) в счет");
                    Console.WriteLine("4 - Проверить состояние счета");
                    Console.WriteLine("5 - Закрыть счет");
                    Console.WriteLine("6 - Выдать счет");

                    value = Convert.ToInt32(Console.ReadLine());
                    switch (value)
                    {
                        case 1:
                            Console.WriteLine("1 - Добавить нового покупателя");
                            Console.WriteLine("2 - Добавить существующего покупателя по ID");
                            value = Convert.ToInt32(Console.ReadLine());
                            ShowTablesInfo();
                            switch (value)
                            {
                                case 1:
                                    Console.WriteLine("Введите ID столика: ");
                                    int tableNumber = Convert.ToInt32(Console.ReadLine());
                                    Console.WriteLine("Введите возраст:");
                                    int age = Convert.ToInt32(Console.ReadLine());
                                    Console.WriteLine("Введите имя:");
                                    string firstName = Convert.ToString(Console.ReadLine());
                                    Console.WriteLine("Введите фамилию:");
                                    string lastName = Convert.ToString(Console.ReadLine());
                                    Console.WriteLine("Введите email:");
                                    string email = Convert.ToString(Console.ReadLine());
                                    Console.WriteLine("Введите телефон:");
                                    string phone = Convert.ToString(Console.ReadLine());
                                    var customer = new CustomerDto(age, firstName, lastName, email, phone);
                                    _customerServise.Create(customer);
                                    _orderServise.Create(new OrderDto(customer.Id, tableNumber));
                                    break;

                                case 2:
                                    Console.WriteLine("Введите ID столика: ");
                                    tableNumber = Convert.ToInt32(Console.ReadLine());
                                    ShowCustomersInfo();
                                    Console.WriteLine("Введите ID покупателя: ");
                                    int idCustomer = Convert.ToInt32(Console.ReadLine());
                                    _orderServise.Create(new OrderDto(idCustomer, tableNumber));
                                    break;
                            }
                            break;

                        case 2:
                            ShowOrdersInfo();
                            break;

                        case 3:

                            int idOrder;
                            if (ShowOpenOrdersInfo() == true)
                            {
                                Console.WriteLine("Введите ID заказа: ");
                                idOrder = Convert.ToInt32(Console.ReadLine());
                                ShowMenuInfo();
                                Console.WriteLine("Введите ID блюд через запятую без пробелов: ");
                                string dishes = Convert.ToString(Console.ReadLine());
                                string[] idDishesString = dishes.Split(new char[] { ',' });
                                int[] idDishes = new int[idDishesString.Length];
                                for (int i = 0; i < idDishes.Length; i++)
                                {
                                    idDishes[i] = Convert.ToInt32(idDishesString[i]);
                                }
                                _orderServise.AddToOrder(idOrder, idDishes);

                            }
                            break;


                        case 4:


                            ShowOrdersInfo();
                            Console.WriteLine("Введите ID заказа: ");
                            idOrder = Convert.ToInt32(Console.ReadLine());
                            if (_orderServise.IsOrderClosed(idOrder))
                            {
                                Console.WriteLine("Чек закрыт");
                            }
                            else
                            {
                                Console.WriteLine("Чек не закрыт");
                            }

                            break;


                        case 5:
                            if (ShowOpenOrdersInfo() == true)
                            {
                                Console.WriteLine("Введите ID заказа: ");
                                idOrder = Convert.ToInt32(Console.ReadLine());
                                _orderServise.CloseOrder(idOrder);
                                break;
                            }
                            else
                            {
                                break;
                            }

                        case 6:
                            ShowOrdersInfo();
                            Console.WriteLine("Введите ID заказа: ");
                            idOrder = Convert.ToInt32(Console.ReadLine());
                            if (_orderServise.IsOrderClosed(idOrder))
                            {
                                Console.WriteLine(_orderServise.Check(idOrder));
                            }
                            else
                            {
                                Console.WriteLine("Сначала закройте чек");
                            }
                            break;
                    }

                    break;
                case 2:
                    Console.WriteLine("1 - Посмотреть меню");
                    Console.WriteLine("2 - Создать блюдо");
                    Console.WriteLine("3 - Изменить цену всех блюд на процент");
                    Console.WriteLine("4 - Изменить цену блюда");
                    Console.WriteLine("5 - Изменить все параметры блюда");
                    Console.WriteLine("6 - Удалить блюдо");
                    value = Convert.ToInt32(Console.ReadLine());
                    switch (value)
                    {
                        case 1:
                            ShowMenuInfo();
                            break;

                        case 2:
                            Console.WriteLine("Введите название:");
                            string itemname = Convert.ToString(Console.ReadLine());
                            Console.WriteLine("Введите описание:");
                            string description = Convert.ToString(Console.ReadLine());
                            Console.WriteLine("Введите цену:");
                            int price = Convert.ToInt32(Console.ReadLine());
                            _menuServise.Create(new DishDto(itemname, description, price));
                            break;

                        case 3:
                            Console.WriteLine("Введите процент:");
                            int persent = Convert.ToInt32(Console.ReadLine());
                            _menuServise.UpdateAllPricePercent(persent);
                            break;

                        case 4:
                            ShowMenuInfo();
                            Console.WriteLine("Введите ID блюда:");
                            int id = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Введите новую цену блюда:");
                            price = Convert.ToInt32(Console.ReadLine());
                            _menuServise.UpdatePrice(_menuServise.GetEntity(id).Id, price);
                            Console.WriteLine("Цена изменена");
                            break;

                        case 5:
                            ShowMenuInfo();
                            Console.WriteLine("Введите ID блюда:");
                            id = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Введите новое название:");
                            itemname = Convert.ToString(Console.ReadLine());
                            Console.WriteLine("Введите новое описание:");
                            description = Convert.ToString(Console.ReadLine());
                            Console.WriteLine("Введите новую цену:");
                            price = Convert.ToInt32(Console.ReadLine());
                            _menuServise.UpdateAllParams(id, itemname, description, price);
                            Console.WriteLine("Параметры измненены");
                            break;

                        case 6:
                            ShowMenuInfo();
                            Console.WriteLine("Введите ID блюда:");
                            id = Convert.ToInt32(Console.ReadLine());
                            _menuServise.Delete(id);
                            break;
                    }
                    break;

                case 3:
                    Console.WriteLine("1 - Создать столик");
                    Console.WriteLine("2 - Показать список столиков");
                    Console.WriteLine("3 - Изменить все параметры столика");
                    Console.WriteLine("4 - Установить резерв");
                    Console.WriteLine("5 - Удалить резерв");
                    Console.WriteLine("6 - Удалить столик");

                    value = Convert.ToInt32(Console.ReadLine());
                    switch (value)
                    {
                        case 1:

                            Console.WriteLine("Введите номер столика:");
                            int tableNumber = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Введите колличество мест:");
                            int seats = Convert.ToInt32(Console.ReadLine());
                            _tableServise.Create(new TableDto(tableNumber, seats));
                            break;

                        case 2:
                            ShowTablesInfo();
                            break;

                        case 3:
                            ShowTablesInfo();
                            Console.WriteLine("Введите ID существующего столика:");
                            int id = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Введите новый номер столика:");
                            tableNumber = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Введите новое колличество мест:");
                            seats = Convert.ToInt32(Console.ReadLine());
                            _tableServise.UpdateAllParams(id, tableNumber, seats);
                            break;

                        case 4:
                            ShowNotReservedTables();
                            Console.WriteLine("Введите ID существующего столика:");
                            id = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("1 - Добавить нового покупателя");
                            Console.WriteLine("2 - Добавить существующего покупателя по ID");
                            value = Convert.ToInt32(Console.ReadLine());

                            switch (value)
                            {
                                case 1:

                                    Console.WriteLine("Введите возраст:");
                                    int age = Convert.ToInt32(Console.ReadLine());
                                    Console.WriteLine("Введите имя:");
                                    string firstName = Convert.ToString(Console.ReadLine());
                                    Console.WriteLine("Введите фамилию:");
                                    string lastName = Convert.ToString(Console.ReadLine());
                                    Console.WriteLine("Введите email:");
                                    string email = Convert.ToString(Console.ReadLine());
                                    Console.WriteLine("Введите телефон:");
                                    string phone = Convert.ToString(Console.ReadLine());
                                    CustomerDto customer = new CustomerDto(age, firstName, lastName, email, phone);
                                    _customerServise.Create(customer);
                                    _tableServise.SetReserved(id, _customerServise.GetEntity(customer.Id));
                                    break;

                                case 2:
                                    ShowCustomersInfo();
                                    Console.WriteLine("Введите ID покупателя: ");
                                    int idCustomer = Convert.ToInt32(Console.ReadLine());
                                    _tableServise.SetReserved(id, _customerServise.GetEntity(idCustomer));
                                    break;
                            }

                            break;


                        case 5:
                            ShowReservedTables();
                            Console.WriteLine("Введите ID существующего столика:");
                            id = Convert.ToInt32(Console.ReadLine());
                            _tableServise.DeteteReserved(id);
                            break;

                        case 6:
                            ShowTablesInfo();
                            Console.WriteLine("Введите ID существующего столика:");
                            id = Convert.ToInt32(Console.ReadLine());
                            _tableServise.Delete(_tableServise.GetEntity(id));
                            break;
                    }
                    break;

                case 4:
                    Console.WriteLine("1 - Создать посетителя");
                    Console.WriteLine("2 - Показать список посетителей");
                    Console.WriteLine("3 - Изменить все параметры посетителя");
                    Console.WriteLine("4 - Удалить посетителя");

                    value = Convert.ToInt32(Console.ReadLine());
                    switch (value)
                    {
                        case 1:
                            Console.WriteLine("Введите возраст:");
                            int age = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Введите имя:");
                            string firstName = Convert.ToString(Console.ReadLine());
                            Console.WriteLine("Введите фамилию:");
                            string lastName = Convert.ToString(Console.ReadLine());
                            Console.WriteLine("Введите email:");
                            string email = Convert.ToString(Console.ReadLine());
                            Console.WriteLine("Введите телефон:");
                            string phone = Convert.ToString(Console.ReadLine());
                            _customerServise.Create(new CustomerDto(age, firstName, lastName, email, phone));
                            break;

                        case 2:
                            ShowCustomersInfo();
                            break;

                        case 3:
                            ShowCustomersInfo();
                            Console.WriteLine("Введите ID существующего посетителя:");
                            int id = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Введите новый возраст:");
                            age = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Введите новое имя:");
                            firstName = Convert.ToString(Console.ReadLine());
                            Console.WriteLine("Введите новую фамилию:");
                            lastName = Convert.ToString(Console.ReadLine());
                            Console.WriteLine("Введите новый email:");
                            email = Convert.ToString(Console.ReadLine());
                            Console.WriteLine("Введите новый телефон:");
                            phone = Convert.ToString(Console.ReadLine());
                            _customerServise.UpdateAllParams(id, age, firstName, lastName, email, phone);
                            break;

                        case 4:
                            ShowCustomersInfo();
                            Console.WriteLine("Введите ID существующего посетителя:");
                            id = Convert.ToInt32(Console.ReadLine());
                            _customerServise.Delete(_customerServise.GetEntity(id));
                            break;

                    }
                    break;

            }

        }

        public void ShowMenuInfo()
        {


            Console.WriteLine("Меню: EAT" + "\t" + "под номером: 1 ");
            foreach (DishDto item in _menuServise.GetAllDishes())
            {
                Console.WriteLine("Id:" + item.Id + "\t" + item.ItemName + "\t" + item.Description + "\t" + item.Price + "\t" + item.DataCreate);
                Console.WriteLine();
            }
        }
        public void ShowOrdersInfo()
        {

            foreach (OrderDto item in _orderServise.GetAllOrders())
            {
                Console.WriteLine("Id:" + item.Id + "\t" + item.CustomerId + "\t" + item.TableId + "\t" + item.OrderDate + "\t" + item.CloseDate + "\t" + item.Total);
                Console.WriteLine();
            }
        }
        public bool ShowOpenOrdersInfo()
        {
            var orders = _orderServise.GetAllOpenOrders();
            if (orders.Count != 0)
            {
                foreach (OrderDto item in orders)
                {
                    Console.WriteLine("Id:" + item.Id + "\t" + item.CustomerId + "\t" + item.TableId + "\t" + item.OrderDate + "\t" + item.CloseDate + "\t" + item.Total);
                    Console.WriteLine();
                }
                return true;

            }
            else
            {
                Console.WriteLine("Все заказы закрыты");
                return false;
            }


        }
        public void ShowCustomersInfo()
        {

            foreach (CustomerDto item in _customerServise.GetALLCustomers())
            {
                Console.WriteLine("Id:" + item.Id + "\t" + item.Age + "\t" + item.FirstName + "\t"
                    + item.LastName + "\t" + item.Email + "\t" + item.Phone + "\t" + item.DataCreate);
                Console.WriteLine();
            }
        }
        public void ShowTablesInfo()
        {


            foreach (TableDto item in _tableServise.GetALLTables())
            {
                Console.WriteLine("Id:" + item.Id + "\t" + "Номер столика:" + item.Number +
                    "\t" + "Колличество мест:" + item.Seats + "\t" + "Зарезерврованно:" + item.Reserved + "\t" + "ID посетителя: " + item.ReservedCustomerId);
                Console.WriteLine();
            }

        }

        public bool ShowReservedTables()
        {
            var tables = _tableServise.GetAllReservedTables();
            if (tables.Count != 0)
            {
                foreach (TableDto item in tables)
                {
                    Console.WriteLine("Id:" + item.Id + "\t" + "Номер столика:" + item.Number +
                        "\t" + "Колличество мест:" + item.Seats + "\t" + "Зарезерврованно:" + item.Reserved + "\t" + "ID посетителя: " + item.ReservedCustomerId);
                    Console.WriteLine();
                }
                return true;
            }
            else
            {
                Console.WriteLine("Все столики свободны");
                return false;
            }

        }
        public bool ShowNotReservedTables()
        {
            var tables = _tableServise.GetAllNotReservedTables();
            if (tables.Count != 0)
            {
                foreach (TableDto item in tables)
                {
                    Console.WriteLine("Id:" + item.Id + "\t" + "Номер столика:" + item.Number +
                        "\t" + "Колличество мест:" + item.Seats + "\t" + "Зарезерврованно:" + item.Reserved);
                    Console.WriteLine();
                }
                return true;
            }
            else
            {
                Console.WriteLine("Все столики заняты");
                return false;
            }

        }
    }
}
