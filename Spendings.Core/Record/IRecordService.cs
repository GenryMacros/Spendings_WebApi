using System.Threading.Tasks;
using System;
using System.Collections.Generic;
namespace Spendings.Core.Records
{
    public interface IRecordService
    {
        Task<Contracts.Record> PostAsync(Contracts.Record record);
        List<Contracts.Record> Get(DateTime startDate, DateTime endDate, int userId, int page, int pageSize);
        Task<Contracts.Record> GetAsync(int userId);
        Task<Contracts.Record> PatchAsync(int additionalAmount, int id);
        Task<Contracts.Record> UpdateAsync(Contracts.Record newRecord, int id);
        Task DeleteAsync(int recordId);
        Task DeleteListAsync(DateTime startDate, DateTime endDate, int userId);
    }
}
