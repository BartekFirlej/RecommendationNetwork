using Microsoft.EntityFrameworkCore;
using ProductStore.DTOs;
using ProductStore.Models;
using ProductStore.Repositories;

namespace ProductStoreTests.Repositories
{
    [TestClass]
    public class ProductRepositoryTests
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
        public async Task GetProducts_ReturnsAllProducts()
        {
            using var dbContext = CreateDbContext();
            dbContext.ProductTypes.Add(new ProductType { Id = 1, Name = "Electronics" });
            dbContext.Products.AddRange(
                new Product { Id = 1, ProductName = "Laptop", Price = 1200.00f, ProductTypeId = 1 },
                new Product { Id = 2, ProductName = "Smartphone", Price = 800.00f, ProductTypeId = 1 }
            );
            dbContext.SaveChanges();
            var repository = new ProductRepository(dbContext);

            var result = await repository.GetProducts();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(p => p.ProductName == "Laptop"));
            Assert.IsTrue(result.Any(p => p.ProductName == "Smartphone"));
        }

        [TestMethod]
        public async Task GetProducts_ReturnsNull()
        {
            using var dbContext = CreateDbContext();
            var repository = new ProductRepository(dbContext);

            var result = await repository.GetProducts();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetProductResponse_ReturnsProduct_WhenExists()
        {
            using var dbContext = CreateDbContext();
            dbContext.ProductTypes.Add(new ProductType { Id = 1, Name = "Electronics" });
            dbContext.Products.AddRange(
                new Product { Id = 1, ProductName = "Laptop", Price = 1200.00f, ProductTypeId = 1 },
                new Product { Id = 2, ProductName = "Smartphone", Price = 800.00f, ProductTypeId = 1 }
            );
            dbContext.SaveChanges();
            var repository = new ProductRepository(dbContext);

            var result = await repository.GetProductResponse(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Laptop", result.ProductName);
            Assert.AreEqual(1200.00f, result.Price);
        }

        [TestMethod]
        public async Task GetProductResponse_ReturnsProduct_WhenNotExists()
        {
            using var dbContext = CreateDbContext();
            dbContext.ProductTypes.Add(new ProductType { Id = 1, Name = "Electronics" });
            dbContext.Products.AddRange(
                new Product { Id = 1, ProductName = "Laptop", Price = 1200.00f, ProductTypeId = 1 },
                new Product { Id = 2, ProductName = "Smartphone", Price = 800.00f, ProductTypeId = 1 }
            );
            dbContext.SaveChanges();
            var repository = new ProductRepository(dbContext);

            var result = await repository.GetProductResponse(3);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetProduct_ReturnsProduct_WhenFound()
        {
            using var dbContext = CreateDbContext();
            dbContext.ProductTypes.Add(new ProductType { Id = 1, Name = "Electronics" });
            dbContext.Products.AddRange(
                new Product { Id = 1, ProductName = "Laptop", Price = 1200.00f, ProductTypeId = 1 },
                new Product { Id = 2, ProductName = "Smartphone", Price = 800.00f, ProductTypeId = 1 }
            );
            dbContext.SaveChanges();
            var repository = new ProductRepository(dbContext);

            var result = await repository.GetProduct(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Laptop", result.ProductName);
        }

        [TestMethod]
        public async Task GetProduct_ReturnsProduct_WhenNotFound()
        {
            using var dbContext = CreateDbContext();
            dbContext.ProductTypes.Add(new ProductType { Id = 1, Name = "Electronics" });
            dbContext.Products.AddRange(
                new Product { Id = 1, ProductName = "Laptop", Price = 1200.00f, ProductTypeId = 1 },
                new Product { Id = 2, ProductName = "Smartphone", Price = 800.00f, ProductTypeId = 1 }
            );
            dbContext.SaveChanges();
            var repository = new ProductRepository(dbContext);

            var result = await repository.GetProduct(3);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task PostProduct_AddsNewProduct()
        {
            using var dbContext = CreateDbContext();
            dbContext.ProductTypes.Add(new ProductType { Id = 1, Name = "Electronics" });
            dbContext.Products.AddRange(
                new Product { Id = 1, ProductName = "Laptop", Price = 1200.00f, ProductTypeId = 1 },
                new Product { Id = 2, ProductName = "Smartphone", Price = 800.00f, ProductTypeId = 1 }
            );
            dbContext.SaveChanges();
            var repository = new ProductRepository(dbContext);
            var newProduct = new ProductRequest { ProductName = "Tablet", Price = 500.00f, ProductTypeId = 1 };

            var result = await repository.PostProduct(newProduct);

            Assert.AreEqual(3, dbContext.Products.Count());
            Assert.AreEqual("Tablet", result.ProductName);
        }

        [TestMethod]
        public async Task DeleteProduct_RemovesProduct_WhenExists()
        {
            using var dbContext = CreateDbContext();
            var repository = new ProductRepository(dbContext);
            dbContext.ProductTypes.Add(new ProductType { Id = 1, Name = "Electronics" });
            dbContext.Products.AddRange(
                new Product { Id = 1, ProductName = "Laptop", Price = 1200.00f, ProductTypeId = 1 },
                new Product { Id = 2, ProductName = "Smartphone", Price = 800.00f, ProductTypeId = 1 }
            );
            dbContext.SaveChanges();
            var productToDelete = await dbContext.Products.FindAsync(1);

            var result = await repository.DeleteProduct(productToDelete);

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(1, dbContext.Products.Count());
            Assert.IsFalse(dbContext.Products.Any(p => p.Id == 1));
        }
    }
}
