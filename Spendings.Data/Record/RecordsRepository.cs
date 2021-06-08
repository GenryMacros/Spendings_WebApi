using AutoMapper;
using Spendings.Data.DB;
using Spendings.Core.Records;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spendings.Data.Records
{
    public class RecordsRepository : IRecordRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public RecordsRepository(IMapper mapper, AppDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Core.Records.Contracts.Record> PostAsync(Core.Records.Contracts.Record record)
        {    
            var dbRecord = _mapper.Map<Record>(record);

            var addResult = await _context.Records.AddAsync(dbRecord);
            await _context.SaveChangesAsync();

            return _mapper.Map<Core.Records.Contracts.Record>(addResult.Entity);
        }
        public async Task<Core.Records.Contracts.Record> UpdateAsync(Core.Records.Contracts.Record newRecord,int recordId)
        {
            var recordToUpdate = await _context.Records.FindAsync(recordId);

            recordToUpdate.Amount = newRecord.Amount;
            recordToUpdate.CategoryId = newRecord.CategoryId;

            var updatedRecord = _context.Update(recordToUpdate).Entity;
            await _context.SaveChangesAsync();

            return _mapper.Map<Core.Records.Contracts.Record>(updatedRecord);
        }
        public async Task<Core.Records.Contracts.Record> PatchAsync(int additionalAmount, int recordId)
        {
            var recordToPatch = await _context.Records.FindAsync(recordId);
            recordToPatch.Amount = recordToPatch.Amount + additionalAmount;

            var patchedRecord = _context.Update(recordToPatch).Entity;
            await _context.SaveChangesAsync();

            return _mapper.Map<Core.Records.Contracts.Record>(patchedRecord);
        }
        public List<Core.Records.Contracts.Record> Get(DateTime startDate, DateTime endDate, int userId, int page, int pageSize)
        {
            int recordsToSkip = (page - 1) * pageSize;
            var requestedRecords = _context.Records.Where(r => r.Date >= startDate && r.Date <= endDate && r.UserId == userId)
                .Skip(recordsToSkip).Take(pageSize).ToList();

            return _mapper.Map<List<Core.Records.Contracts.Record>>(requestedRecords);
        }
        public async Task<Core.Records.Contracts.Record> GetAsync(int recordId)
        {
            var requestedRecord =  await _context.Records.FindAsync(recordId);
            return _mapper.Map<Core.Records.Contracts.Record>(requestedRecord);
        }
        public async Task DeleteAsync(int recordId)
        {
            var dbRecord = await _context.Records.FindAsync(recordId);

            _context.Records.Remove(dbRecord);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteListAsync(DateTime startDate, DateTime endDate, int userId)
        {
            List<Record> recordToDelete = _context.Records.Where(r => r.Date >= startDate && r.Date <= endDate && r.UserId == userId).ToList();

            _context.RemoveRange(recordToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
