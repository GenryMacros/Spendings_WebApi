using System.Threading.Tasks;
using Moq;
using Spendings.Core.Categories;
using Spendings.Orchrestrators.Categories;
using Spendings.Core.Exeptions;
using Xunit;

namespace Onion.Spendings.Api.Unit.Tests.Category
{
    public class CategoryServiceTest
    {
        private readonly CategoriesService _service;
        private readonly Mock<ICategoryRepository> _mockedRepository = new Mock<ICategoryRepository>();
        public CategoryServiceTest()
        {
            _service = new CategoriesService(_mockedRepository.Object);
        }

        [Fact]
        public async Task GetAsync_IfIdIsUndefined_ThrowNotFoundException()
        {
            //Arrange
            int categoryId = 10;
            global::Spendings.Core.Categories.Contracts.Category category = null;

            _mockedRepository.Setup(r => r.GetAsync(categoryId)).ReturnsAsync(category);
            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () => await _service.GetAsync(categoryId));

            //Assert
            Assert.NotNull(exception);
        }
        [Fact]
        public async Task PostAsync_IfTNameIsNotOriginal_ThrowFailedInsertionException()
        {
            //Arrange
            global::Spendings.Core.Categories.Contracts.Category category = new global::Spendings.Core.Categories.Contracts.Category
            {
                Id = 1,
                Name = "asdasd"
            };

            _mockedRepository.Setup(r => r.IsCategoryWithGivenNameExists(category.Name)).Returns(true);
            //Act
            var exception = await Assert.ThrowsAsync<FailedInsertionException>(async () => await _service.PostAsync(category));

            //Assert
            Assert.NotNull(exception);
        }
        [Fact]
        public async Task DeleteAsync_IfIdIsUndefined_ThrowNotFoundException()
        {
            //Arrange
            int categoryId = 1;
            global::Spendings.Core.Categories.Contracts.Category category = null;
            _mockedRepository.Setup(r => r.GetAsync(categoryId)).ReturnsAsync(category);
            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () => await _service.DeleteAsync(categoryId));

            //Assert
            Assert.NotNull(exception);
        }
    }
}
