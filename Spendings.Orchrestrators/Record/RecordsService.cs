using System.Threading.Tasks;
using Spendings.Core.Records;
using System;
using System.Collections.Generic;
using Spendings.Core.Exeptions;

namespace Spendings.Orchrestrators.Records
{
    public class RecordsService : IRecordService
    {
        private readonly IRecordRepository _repo;
        public RecordsService(IRecordRepository repo)
        {
            _repo = repo;
        }
        public List<Core.Records.Contracts.Record> Get(DateTime from, DateTime to, int userId, int page, int pageSize)
        {
            return _repo.Get(from,to,userId,page,pageSize);
        }
        public async Task<Core.Records.Contracts.Record> GetAsync(int recordId)
        {
            bool isExists = await isRecordExists(recordId);
            if (!isExists)
                throw new NotFoundException("No record with given id");

            return await _repo.GetAsync(recordId);
        }
        public async Task<Core.Records.Contracts.Record> UpdateAsync(Core.Records.Contracts.Record newRecord, int recordId)
        {
            bool isExists = await isRecordExists(recordId);
            if (!isExists)
                throw new NotFoundException("No record with given id");

            return await _repo.UpdateAsync(newRecord, recordId);
        }
        public async Task<Core.Records.Contracts.Record> PostAsync(Core.Records.Contracts.Record record)
        {
            return await _repo.PostAsync(record);
        }
        public async Task DeleteAsync(int recordId)
        {
            bool isExists = await isRecordExists(recordId);
            if (!isExists)
                throw new DeletionFailedException("No record with given id");

            await _repo.DeleteAsync(recordId);
        }
        public async Task DeleteListAsync(DateTime startDate, DateTime endDate, int userId)
        {
            await _repo.DeleteListAsync(startDate, endDate, userId);
        }

        private async Task<bool> isRecordExists(int recordId)
        {
            var requestedRecord = await _repo.GetAsync(recordId);

            if (requestedRecord == null)
                return false;
            return true;
        }

        public async Task<Core.Records.Contracts.Record> PatchAsync(int additionalAmount, int recordId)
        {
            await checkIfRecordsExistsAndOverflowed(recordId, additionalAmount);

            return await _repo.PatchAsync(additionalAmount, recordId);
        }
        private async Task checkIfRecordsExistsAndOverflowed(int recordId, int additionalAmount)
        {
            var requestedRecord = await _repo.GetAsync(recordId);

            if (requestedRecord == null)
                throw new NotFoundException("No record with given id");

            checkForOverflow(requestedRecord.Amount, additionalAmount);
        }

        private void checkForOverflow(int currentAmount, int additionalAmount)
        {
            long newAmount = (long)currentAmount + additionalAmount;

            if (newAmount > int.MaxValue)
                throw new OverflowException();
        }

    }
}
