using Cafe.Dal.Contracts.Repositories.Table;
using Cafe.Dal.Contracts.Repositories.Table.Models;
using Cafe.Dal.Infrastructure.DBSettingsEF;

namespace Cafe.Dal.Infrastructure.RepositoriesEF
{
    internal class TableRepository : ITableRepository
    {

        public bool Create(TableDb entity)
        {
            using CafeContext db = new CafeContext();
            var table = new TableDb(entity.Number, entity.Seats);
            db.Tables.Add(table);
            db.SaveChanges();
            entity.Id = table.Id;
            return true;
        }


        public bool Delete(int id)
        {
            using CafeContext db = new CafeContext();
            db.Tables.Remove(GetEntity(id));
            db.SaveChanges();
            return true;
        }

        //public void DeleteALL()
        //{
        //    using (SqlConnection connection = new SqlConnection(SqlOperations.ConnStr))
        //    {
        //        connection.Open();
        //        string sql = $"DELETE FROM Tables;";
        //        SqlCommand cmd1 = new SqlCommand(sql, connection);
        //        cmd1.ExecuteNonQuery();
        //        connection.Close();
        //        connection.Dispose();
        //    }
        //}

        public List<TableDb> GetAll()
        {
            List<TableDb> tables = new List<TableDb>();
            using (CafeContext db = new CafeContext())
            {
                tables.AddRange(db.Tables.ToList());
            }
            return tables;
        }

        public List<TableDb> GetAllNotReserved()
        {
            List<TableDb> tables = new List<TableDb>();
            using (CafeContext db = new CafeContext())
            {
                tables.AddRange(db.Tables.Where(p => p.Reserved == null).ToList());
            }
            return tables;
        }

        public List<TableDb> GetAllReserved()
        {
            List<TableDb> tables = new List<TableDb>();
            using (CafeContext db = new CafeContext())
            {
                tables.AddRange(db.Tables.Where(p => p.Reserved != null).ToList());
            }
            return tables;
        }


        public TableDb GetEntity(int id)
        {
            using CafeContext db = new CafeContext();
            var table = db.Tables.Find(id);
            return table;
        }

        public bool Update(TableDb entity)
        {
            using CafeContext db = new CafeContext();
            db.Tables.Update(entity);
            db.SaveChanges();
            return true;

        }

    }
}
