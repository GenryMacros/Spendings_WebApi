using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Spendings.Core.Categories;
using Spendings.Core.Categories.Contracts;
using Spendings.Orchrestrators.Categories.Contracts;
using AutoMapper;

namespace onion_spendings.Categorie
{
    [ApiController]
    [Route("Spendings/v1")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly IMapper _mapper;
        public CategoriesController(IMapper mapper, ICategoryService service)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("Categories/{categoryId}")]
        public async Task<IActionResult> GetAsync(int categoryId)
        {
            var recievedCategory = await _service.GetAsync(categoryId);
            var outCategory = _mapper.Map<OutCategory>(recievedCategory);
            return Ok(outCategory);
        }

        [HttpPost("Categories")]
        public async Task<IActionResult> PostAsync([FromBody]InCategory category)
        {
            var mappedCoreCategory = _mapper.Map<Category>(category);
            var addResult = await _service.PostAsync(mappedCoreCategory);
            var outCategory = _mapper.Map<OutCategory>(addResult);
            return Ok(outCategory);
        }

        [HttpDelete("Categories/{categoryId}")]
        public async Task<IActionResult> DeleteAsync(int categoryId)
        {
            await _service.DeleteAsync(categoryId);
            return Ok();
        }
    }
}