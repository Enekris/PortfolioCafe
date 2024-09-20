using Cafe.Dal.Contracts.Repositories.Dish.Models;
using Cafe.Dal.Contracts.Repositories.Order;
using Cafe.Dal.Contracts.Repositories.Order.Models;
using Cafe.Dal.Contracts.Repositories.OrderDish.model;
using Cafe.Dal.Infrastructure.DBSettingsEF;

namespace Cafe.Dal.Infrastructure.RepositoriesEF
{
    public class OrderRepository : IOrderRepository
    {


        public bool Create(OrderDb entity)
        {
            using (CafeContext db = new CafeContext())
            {
                var order = new OrderDb(entity.CustomerId, entity.TableId);
                db.Orders.Add(order);
                db.SaveChanges();
                entity.Id = order.Id;

            }
            using (CafeContext db = new CafeContext())
            {
                var table = db.Tables.Find(entity.TableId);
                table.Reserved = DateTime.Now;
                table.ReservedCustomerId = entity.CustomerId;
                db.Tables.Update(table);
                db.SaveChanges();
                return true;
            }

        }
        public List<DishDb> GetAllDishes()
        {
            using CafeContext db = new CafeContext();
            return db.Dishes.ToList();
        }

        public List<DishDb> AddToOrder(int idOrder, int[] idDishes)
        {
            var order = GetEntity(idOrder);
            List<DishDb> dishesFromGetAll = GetAllDishes();
            int i = 0;
            while (i < idDishes.Length)
            {
                order.OrderDishes.Add(dishesFromGetAll.Find(p => p.Id == idDishes[i]));
                i++;
            }
            i = 0;
            while (i < order.OrderDishes.Count)
            {
                using (CafeContext db = new CafeContext())
                {
                    var ordersdishes = new OrdersDishDb
                    {
                        OrderId = order.Id,
                        DishId = order.OrderDishes[i].Id,
                        OrderDate = order.OrderDate,
                        DishPriceOnOrdersDate = order.OrderDishes[i].Price
                    };

                    db.OrdersDishes.Add(ordersdishes);
                    db.SaveChanges();

                }
                i++;
            }
            UpdateTotal(order);
            return order.OrderDishes;
        }

        private void UpdateTotal(OrderDb entity)  // тащит из OrderDishes стоимость блюд. Обновляет стоимость в базе
        {
            if (entity.Total == null)
            {
                entity.Total = 0;
            }

            foreach (var item in entity.OrderDishes)
            {
                entity.Total += item.Price;
            }
            using CafeContext db = new CafeContext();
            db.Orders.Update(entity);
            db.SaveChanges();
        }

        public void CloseOrder(int idOrder) //закрывает счет, снимает бронь со стола
        {
            var order = GetEntity(idOrder);
            using (CafeContext db = new CafeContext())
            {

                order.CloseDate = DateTime.Now;
                db.Orders.Update(order);
                db.SaveChanges();
            }

            using (CafeContext db = new CafeContext())
            {
                var table = db.Tables.Find(order.TableId);
                table.Reserved = null;
                table.ReservedCustomerId = null;
                db.Tables.Update(table);
                db.SaveChanges();

            }
        }

        public List<OrderDb> GetAllOpen()
        {
            List<OrderDb> orders = new List<OrderDb>();

            using (CafeContext db = new CafeContext())
            {
                orders.AddRange(db.Orders.Where(p => p.CloseDate == null).ToList());

            }
            return orders;
        }

        public List<OrderDb> GetAll()  //список всех счетов   
        {

            using CafeContext db = new CafeContext();
            var orders = db.Orders.ToList();
            return orders;
        }

        public string Check(int id)
        {
            string check = "";
            using (CafeContext db = new CafeContext())
            {
                var query = from order in db.Orders
                            join customer in db.Customers on order.Id equals id
                            where order.CustomerId == customer.Id
                            select new { firstName = customer.FirstName, order_date = order.OrderDate, close_date = order.CloseDate };
                foreach (var item in query)
                {
                    check += item.firstName + "\t" + item.order_date + "\t" + item.close_date + "\n";
                }
                check += "-------------------------\n";
            }

            using (CafeContext db = new CafeContext())
            {
                var query = from dish in db.Dishes
                            join ordersDishes in db.OrdersDishes on dish.Id equals ordersDishes.DishId
                            where ordersDishes.OrderId == id
                            group dish by new { dish.ItemName, ordersDishes.DishPriceOnOrdersDate } into g
                            select new
                            {
                                itemName = g.Key.ItemName,
                                price = g.Key.DishPriceOnOrdersDate,
                                kolvo = g.Count()
                            };
                foreach (var item in query)
                {
                    check += item.itemName + "\t" + item.price + "\t" + item.kolvo + "\n";
                }
                check += "-------------------------\n";
            }
            using (CafeContext db = new CafeContext())
            {
                var query = from orders in db.Orders
                            where orders.Id == id
                            select new
                            {
                                total = orders.Total
                            };
                foreach (var item in query)
                {
                    check += "Итого:" + item.total + "\n";
                }

            }
            return check;
        }


        public OrderDb GetEntity(int id)
        {
            using CafeContext db = new CafeContext();
            return db.Orders.Find(id);
        }


    }
}
