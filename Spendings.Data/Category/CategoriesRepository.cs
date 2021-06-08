using AutoMapper;
using Spendings.Data.DB;
using System.Threading.Tasks;
using System.Linq;
using Spendings.Core.Categories;
namespace Spendings.Data.Categories
{
    public class CategoriesRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CategoriesRepository(IMapper mapper, AppDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Core.Categories.Contracts.Category> GetAsync(int categoryId)
        {
            var categorySearchResult = await _context.Categories.FindAsync(categoryId);
            return _mapper.Map<Core.Categories.Contracts.Category>(categorySearchResult);
        }
        public bool IsCategoryWithGivenNameExists(string categoryName)
        {
            return _context.Categories.Count(c => c.Name == categoryName) > 0;
        }
        public async Task<Core.Categories.Contracts.Category> PostAsync(Core.Categories.Contracts.Category category)
        {
            var mappedToDataCategory = _mapper.Map<Category>(category);

            var addResult =await _context.Categories.AddAsync(mappedToDataCategory);
            await _context.SaveChangesAsync();

            var outCategory = _mapper.Map<Core.Categories.Contracts.Category>(addResult.Entity);
            return outCategory;
        }
        public async Task DeleteAsync(int categoryId)
        {
            var categorSearchResult = await _context.Categories.FindAsync(categoryId);

            _context.Categories.Remove(categorSearchResult);
            await _context.SaveChangesAsync();
        }
    }
}
