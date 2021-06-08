using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Spendings.Core.Records
{
    public interface IRecordRepository
    {
        Task<Contracts.Record> PostAsync(Contracts.Record record);
        List<Contracts.Record> Get(DateTime startDate, DateTime endDate, int userId, int page, int pageSize);
        Task<Contracts.Record> GetAsync(int recordId);
        Task<Contracts.Record> UpdateAsync(Contracts.Record newRecord, int id);
        Task<Contracts.Record> PatchAsync(int additionalAmount, int id);
        Task DeleteAsync(int recordId);
        Task DeleteListAsync(DateTime startDate, DateTime endDate, int userId);
    }
}
