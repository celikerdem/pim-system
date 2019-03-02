using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PIMSystem.Core.Data;
using PIMSystem.Core.Domain.Entities;
using PIMSystem.Service.Data;

namespace Tests
{
    public class CategoryServiceTests
    {
        private CategoryService _categoryService;

        [SetUp]
        public void Setup()
        {
            var repository = new Mock<IRepository<Category>>();
            _categoryService = new CategoryService(repository.Object);
        }

        [Test]
        public async Task CreateCategory_ShouldNotHaveError()
        {
            var category = new Category
            {
                Name = "qazwsxedc"
            };

            var serviceResponse = await _categoryService.CreateCategoryAsync(category);

            Assert.IsTrue(!serviceResponse.HasError);
        }
    }
}