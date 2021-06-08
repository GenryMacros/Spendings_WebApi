using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Spendings.Core.Users;
using Spendings.Core.Users.Contracts;
using Spendings.Orchrestrators.Users.Contracts;
using AutoMapper;

namespace onion_spendings.Users
{
    [ApiController]
    [Route("Spendings/v1")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        public UsersController(IMapper mapper, IUserService service)
        {
            _service = service;
            _mapper = mapper;
        }
        [HttpGet("Users/{userId}")]
        public IActionResult Get(int userId)
        {
            var recievedUser = _service.Get(userId);
            var outUser = _mapper.Map<OutUser>(recievedUser);
            return Ok(outUser);
        }
        [HttpGet("Users")]
        public IActionResult Get([FromQuery] InUser user)
        {
            var coreUser = _mapper.Map<User>(user);
            var addResult = _service.Get(coreUser);
            var outUser = _mapper.Map<OutUser>(addResult);
            return Ok(outUser);
        }
        [HttpPost("Users")]
        public async Task<IActionResult> PostAsync(InUser user)
        {
            var coreUser = _mapper.Map<User>(user);
            var addResult = await _service.PostAsync(coreUser);
            var outUser = _mapper.Map<OutUser>(addResult);
            return Ok(outUser);
        }
        [HttpPatch("Users/{userId}")]
        public async Task<IActionResult> PatchAsync(int userId,string newLogin)
        {
            var addResult = await _service.PatchAsync(userId, newLogin);
            var outUser = _mapper.Map<OutUser>(addResult);
            return Ok(outUser);
        }
        [HttpDelete("Users/{userId}")]
        public async Task<IActionResult> DeleteAsync(int userId)
        {
            await _service.DeleteAsync(userId);
            return Ok();
        }
    }
}
