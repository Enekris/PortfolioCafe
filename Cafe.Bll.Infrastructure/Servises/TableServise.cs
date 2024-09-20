using AutoMapper;
using Cafe.Bll.Contracts.Servises.Customer.Models;
using Cafe.Bll.Contracts.Servises.Table;
using Cafe.Bll.Contracts.Servises.Table.Models;
using Cafe.Dal.Contracts.Repositories.Table;
using Cafe.Dal.Contracts.Repositories.Table.Models;

namespace Cafe.Bll.Infrastructure.Servises
{
    internal class TableServise : ITableServise
    {
        private readonly ITableRepository _tableRepository;
        private readonly IMapper _mapper;
        public TableServise(IMapper mapper, ITableRepository tableRepository)
        {
            _tableRepository = tableRepository;
            _mapper = mapper;
        }

        public List<TableDto> GetALLTables()
        {
            var tablesDb = _tableRepository.GetAll();
            var tablesDto = _mapper.Map<List<TableDto>>(tablesDb);
            return tablesDto;
        }
        public List<TableDto> GetAllReservedTables()
        {
            var tablesDb = _tableRepository.GetAllReserved();
            var tablesDto = _mapper.Map<List<TableDto>>(tablesDb);
            return tablesDto;
        }

        public List<TableDto> GetAllNotReservedTables()
        {
            var tablesDb = _tableRepository.GetAllNotReserved();
            var tablesDto = _mapper.Map<List<TableDto>>(tablesDb);
            return tablesDto;
        }

        public void Delete(TableDto table)
        {
            _tableRepository.Delete(table.Id);
        }
        //public void DeleteAll()
        //{
        //    _tableRepository.DeleteALL();
        //}
        public void Create(TableDto table)
        {
            var tableDb = new TableDb(table.Number, table.Seats);
            _tableRepository.Create(tableDb);
            table.Id = tableDb.Id;
        }
        public bool UpdateAllParams(int id, int number, int seats)
        {
            var tableDb = _tableRepository.GetEntity(id);
            tableDb.Number = number;
            tableDb.Seats = seats;
            return _tableRepository.Update(tableDb);
        }
        public bool DeteteReserved(int id)
        {
            TableDb table = _tableRepository.GetEntity(id);
            table.Reserved = null;
            table.ReservedCustomerId = null;
            _tableRepository.Update(table);
            return true;
        }

        public bool SetReserved(int idTable, CustomerDto customer)
        {
            TableDb table = _tableRepository.GetEntity(idTable);
            table.Reserved = DateTime.Now;
            table.ReservedCustomerId = customer.Id;
            _tableRepository.Update(table);
            return true;
        }

        public TableDto GetEntity(int id)
        {
            var tablesDb = _tableRepository.GetEntity(id);
            var tablesDto = _mapper.Map<TableDto>(tablesDb);
            return tablesDto;
        }


    }
}
