using Microsoft.EntityFrameworkCore;
using ProductStore.DTOs;
using ProductStore.Models;
using ProductStore.Repositories;

namespace ProductStoreTests.Repositories
{
    [TestClass]
    public class ProductTypeRepositoryTests
    {
        private StoreDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase(System.Guid.NewGuid().ToString())
                .Options;

            var dbContext = new StoreDbContext(options);
            return dbContext;
        }

        [TestMethod]
        public async Task GetProductTypes_ReturnsAllProductTypes()
        {
            using var dbContext = CreateDbContext();
            dbContext.ProductTypes.AddRange(
                new ProductType { Id = 1, Name = "Electronics" },
                new ProductType { Id = 2, Name = "Books" }
            );
            dbContext.SaveChanges();
            var repository = new ProductTypeRepository(dbContext);

            var result = await repository.GetProductTypes();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(pt => pt.Name == "Electronics"));
            Assert.IsTrue(result.Any(pt => pt.Name == "Books"));
        }

        [TestMethod]
        public async Task GetProductTypes_ReturnsNotFoundProductTypes()
        {
            using var dbContext = CreateDbContext();
            var repository = new ProductTypeRepository(dbContext);

            var result = await repository.GetProductTypes();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetProductTypeResponse_ReturnsProductType_WhenExists()
        {
            using var dbContext = CreateDbContext();
            dbContext.ProductTypes.AddRange(
                new ProductType { Id = 1, Name = "Electronics" },
                new ProductType { Id = 2, Name = "Books" }
            );
            dbContext.SaveChanges();
            var repository = new ProductTypeRepository(dbContext);

            var result = await repository.GetProductTypeResponse(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Electronics", result.Name);
        }

        [TestMethod]
        public async Task GetProductTypeResponse_ReturnsProductType_WhenNotExists()
        {
            using var dbContext = CreateDbContext();
            var repository = new ProductTypeRepository(dbContext);

            var result = await repository.GetProductTypeResponse(1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetProductType_ReturnsProductType_WhenExists()
        {
            using var dbContext = CreateDbContext();
            dbContext.ProductTypes.AddRange(
                new ProductType { Id = 1, Name = "Electronics" },
                new ProductType { Id = 2, Name = "Books" }
            );
            dbContext.SaveChanges();
            var repository = new ProductTypeRepository(dbContext);

            var result = await repository.GetProductType(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Electronics", result.Name);
        }

        [TestMethod]
        public async Task GetProductType_ReturnsProductType_WhenNotExists()
        {
            using var dbContext = CreateDbContext();
            var repository = new ProductTypeRepository(dbContext);

            var result = await repository.GetProductType(1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task PostProductType_AddsNewProductType()
        {
            using var dbContext = CreateDbContext();
            dbContext.ProductTypes.AddRange(
                new ProductType { Id = 1, Name = "Electronics" },
                new ProductType { Id = 2, Name = "Books" }
            );
            dbContext.SaveChanges();
            var repository = new ProductTypeRepository(dbContext);

            var newProductType = new ProductTypeRequest { Name = "Furniture" };
            var result = await repository.PostProductType(newProductType);

            Assert.AreEqual(3, dbContext.ProductTypes.Count());
            Assert.AreEqual("Furniture", result.Name);
        }

        [TestMethod]
        public async Task DeleteProductType_RemovesProductType_WhenExists()
        {
            using var dbContext = CreateDbContext();
            dbContext.ProductTypes.AddRange(
                new ProductType { Id = 1, Name = "Electronics" },
                new ProductType { Id = 2, Name = "Books" }
            );
            dbContext.SaveChanges();
            var repository = new ProductTypeRepository(dbContext);

            var productTypeToDelete = await dbContext.ProductTypes.FindAsync(1);
            var result = await repository.DeleteProductType(productTypeToDelete);

            Assert.AreEqual(1, dbContext.ProductTypes.Count());
            Assert.IsFalse(dbContext.ProductTypes.Any(pt => pt.Id == 1));
        }
    }
}
