using Cafe.Dal.Contracts.Repositories.Dish;
using Cafe.Dal.Contracts.Repositories.Dish.Models;
using Cafe.Dal.Infrastructure.DBSettingsEF;

namespace Cafe.Dal.Infrastructure.RepositoriesEF
{
    public class DishRepository : IDishRepository
    {

        public bool Create(DishDb entity)
        {
            using CafeContext db = new CafeContext();
            var dish = new DishDb(entity.ItemName, entity.Description, entity.Price);
            db.Dishes.Add(dish);
            db.SaveChanges();
            entity.Id = dish.Id;
            return true;

        }

        public bool Delete(int idDish)
        {
            using CafeContext db = new CafeContext();
            db.Dishes.Remove(GetEntity(idDish));
            db.SaveChanges();
            return true;
        }


        public List<DishDb> GetAll()
        {
            using CafeContext db = new CafeContext();
            return db.Dishes.ToList();
        }

        public DishDb GetEntity(int id)
        {
            using CafeContext db = new CafeContext();
            return db.Dishes.Find(id);
        }

        public bool Update(DishDb entity)
        {
            using CafeContext db = new CafeContext();
            db.Dishes.Update(entity);
            db.SaveChanges();
            return true;
        }
    }




}

