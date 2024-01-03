using Microsoft.EntityFrameworkCore;
using ProductStore.DTOs;
using ProductStore.Models;
using ProductStore.Repositories;

namespace ProductStoreTests.Repositories
{
    [TestClass]
    public class VoivodeshipRepositoryTests
    {
        private StoreDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{System.Guid.NewGuid()}")
                .Options;

            var dbContext = new StoreDbContext(options);
            return dbContext;
        }

        [TestMethod]
        public async Task GetVoivodeships_ReturnsAllVoivodeships()
        {
            using var dbContext = CreateDbContext(); 
            dbContext.Voivodeships.Add(new Voivodeship { Id = 1, Name = "Voivodeship1" });
            dbContext.Voivodeships.Add(new Voivodeship { Id = 2, Name = "Voivodeship2" });
            dbContext.SaveChanges();
            var repository = new VoivodeshipRepository(dbContext);

            var result = await repository.GetVoivodeships();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(v => v.Name == "Voivodeship1"));
            Assert.IsTrue(result.Any(v => v.Name == "Voivodeship2"));
        }

        [TestMethod]
        public async Task GetVoivodeships_ReturnsAllVoivodeships_NotFound()
        {
            using var dbContext = CreateDbContext();
            dbContext.SaveChanges();
            var repository = new VoivodeshipRepository(dbContext);

            var result = await repository.GetVoivodeships();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetVoivodeship_ReturnsVoivodeship_WhenExists()
        {
            using var dbContext = CreateDbContext();
            dbContext.Voivodeships.Add(new Voivodeship { Id = 1, Name = "Voivodeship1" });
            dbContext.Voivodeships.Add(new Voivodeship { Id = 2, Name = "Voivodeship2" });
            dbContext.SaveChanges();
            var repository = new VoivodeshipRepository(dbContext);

            var result = await repository.GetVoivodeship(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Voivodeship1", result.Name);
        }

        [TestMethod]
        public async Task GetVoivodeship_ReturnsVoivodeship_WhenNotExists()
        {
            using var dbContext = CreateDbContext();
            dbContext.Voivodeships.Add(new Voivodeship { Id = 1, Name = "Voivodeship1" });
            dbContext.Voivodeships.Add(new Voivodeship { Id = 2, Name = "Voivodeship2" });
            dbContext.SaveChanges();
            var repository = new VoivodeshipRepository(dbContext);

            var result = await repository.GetVoivodeship(3);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetVoivodeshipResponse_ReturnsVoivodeshipResponse_WhenExists()
        {
            using var dbContext = CreateDbContext();
            dbContext.Voivodeships.Add(new Voivodeship { Id = 1, Name = "Voivodeship1" });
            dbContext.Voivodeships.Add(new Voivodeship { Id = 2, Name = "Voivodeship2" });
            dbContext.SaveChanges();
            var repository = new VoivodeshipRepository(dbContext);

            var result = await repository.GetVoivodeshipResponse(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Voivodeship1", result.Name);
        }

        [TestMethod]
        public async Task GetVoivodeshipResponse_ReturnsVoivodeshipResponse_WhenNotExists()
        {
            using var dbContext = CreateDbContext();
            dbContext.SaveChanges();
            var repository = new VoivodeshipRepository(dbContext);

            var result = await repository.GetVoivodeshipResponse(1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DeleteVoivodeship_RemovesVoivodeship_WhenExists()
        {
            using var dbContext = CreateDbContext();
            dbContext.Voivodeships.Add(new Voivodeship { Id = 1, Name = "Voivodeship1" });
            dbContext.Voivodeships.Add(new Voivodeship { Id = 2, Name = "Voivodeship2" });
            dbContext.SaveChanges();
            var repository = new VoivodeshipRepository(dbContext);

            var voivodeshipToDelete = await dbContext.Voivodeships.FindAsync(1);
            var result = await repository.DeleteVoivodeship(voivodeshipToDelete);

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(1, dbContext.Voivodeships.Count());
        }

        [TestMethod]
        public async Task PostVoivodeship_AddsNewVoivodeship()
        {
            using var dbContext = CreateDbContext();
            dbContext.Voivodeships.Add(new Voivodeship { Id = 1, Name = "Voivodeship1" });
            dbContext.Voivodeships.Add(new Voivodeship { Id = 2, Name = "Voivodeship2" });
            dbContext.SaveChanges();
            var repository = new VoivodeshipRepository(dbContext);

            var newVoivodeship = new VoivodeshipRequest { Name = "Voivodeship3" };
            var result = await repository.PostVoivodeship(newVoivodeship);

            Assert.AreEqual(3, dbContext.Voivodeships.Count());
            Assert.AreEqual("Voivodeship3", result.Name);
        }
    }
}
