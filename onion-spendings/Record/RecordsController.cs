using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Spendings.Core.Records;
using Spendings.Core.Records.Contracts;
using Spendings.Orchrestrators.Records.Contracts;
using AutoMapper;
using System.Collections.Generic;

namespace onion_spendings.Records
{
    [ApiController]
    [Route("Spendings/v1")]
    public class RecordsController : Controller
    {
        private readonly IRecordService _service;
        private readonly IMapper _mapper;
        public RecordsController(IMapper mapper, IRecordService service)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost("Users/{userId}/Records")]
        public async Task<IActionResult> PostAsync([FromBody] InRecord record, int userId)
        {
            var coreRecord = _mapper.Map<Record>(record);
            coreRecord.UserId = userId;

            var addResult = await _service.PostAsync(coreRecord);
            var outRecord = _mapper.Map<OutRecord>(addResult);
            return Ok(outRecord);
        }

        [HttpGet("Users/{userId}/Records/from/{from}/till/{till}")]
        public IActionResult GetAsync(int userId,  DateTime from,  DateTime till,[FromQuery] PaginationParameters param)
        {
            var requestedList =  _service.Get(from, till, userId, param.PageNumber, param.PageSize);
            var outList = _mapper.Map<List<OutRecord>>(requestedList);
            return Ok(outList);
        }

        [HttpGet("Records/{recordId}")]
        public async Task<IActionResult> GetAsync(int recordId)
        {
            var recievedRecord = await _service.GetAsync(recordId);
            var outRecord = _mapper.Map<OutRecord>(recievedRecord);
            return Ok(outRecord);
        }

        [HttpPut("Records/{recordId}")]
        public async Task<IActionResult> UpdateAsync([FromBody] InRecord newRecord, int recordId)
        {
            var coreRecord = _mapper.Map<Record>(newRecord);
            var updatedRecord = await _service.UpdateAsync(coreRecord, recordId);
            var outRecord = _mapper.Map<OutRecord>(updatedRecord);
            return Ok(outRecord);
        }
       
        [HttpPatch("Records/{recordId}")]
        public async Task<IActionResult> PatchAsync(int additionalAmount, int recordId)
        {
            var patchedUser = await _service.PatchAsync(additionalAmount, recordId);
            var outRecord = _mapper.Map<OutRecord>(patchedUser);
            return Ok(outRecord);
        }
        
        [HttpDelete("Records/{recordId}")]
        public async Task<IActionResult> DeleteAsync(int recordId)
        {
            await _service.DeleteAsync(recordId);
            return Ok();
        }

        [HttpDelete("Users/{userId}/Records/from/{from}/till/{till}")]
        public async Task<IActionResult> DeleteListAsync(int userId, DateTime from, DateTime till)
        {
            await _service.DeleteListAsync(from, till, userId);
            return Ok();
        }
    }
}
