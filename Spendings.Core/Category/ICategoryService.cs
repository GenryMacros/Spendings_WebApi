using System.Threading.Tasks;

namespace Spendings.Core.Categories
{
    public interface ICategoryService
    {
        Task<Contracts.Category> GetAsync(int categoryId);
        Task<Contracts.Category> PostAsync(Contracts.Category category);
        Task DeleteAsync(int categoryId);
    }
}
