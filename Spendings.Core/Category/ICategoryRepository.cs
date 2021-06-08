using System.Threading.Tasks;


namespace Spendings.Core.Categories
{
    public interface ICategoryRepository
    {
        Task<Contracts.Category> GetAsync(int categoryId);
        Task<Contracts.Category> PostAsync(Contracts.Category category);
        Task DeleteAsync(int categoryId);
        bool IsCategoryWithGivenNameExists(string categoryName);
    }
}
