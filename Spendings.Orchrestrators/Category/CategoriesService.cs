using System.Threading.Tasks;
using Spendings.Core.Categories;
using Spendings.Core.Exeptions;

namespace Spendings.Orchrestrators.Categories
{
   public class CategoriesService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        public CategoriesService(ICategoryRepository repo)
        {
            _repo = repo;
        }
        public async Task<Core.Categories.Contracts.Category> GetAsync(int categoryId)
        {
            var recievedCategory = await _repo.GetAsync(categoryId);

            if (recievedCategory == null)
                throw new NotFoundException("No category with given ID");

            return recievedCategory;
        }
        public async Task<Core.Categories.Contracts.Category> PostAsync(Core.Categories.Contracts.Category category)
        {
            if (IsCategoryWithNameExists(category.Name))
                throw new FailedInsertionException();

            return await _repo.PostAsync(category);
        }
        public async Task DeleteAsync(int categoryId)
        {
            bool isExists = await IsCategoryExists(categoryId);
            if (isExists)
                throw new NotFoundException();

            await _repo.DeleteAsync(categoryId);
        }
        private async Task<bool> IsCategoryExists(int categoryId)
        {
            return await _repo.GetAsync(categoryId) == null;
        }
        private bool IsCategoryWithNameExists(string categoryName)
        {
            return _repo.IsCategoryWithGivenNameExists(categoryName);
        }
    }
}
