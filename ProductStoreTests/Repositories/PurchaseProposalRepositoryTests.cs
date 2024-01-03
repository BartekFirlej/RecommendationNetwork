using Microsoft.EntityFrameworkCore;
using ProductStore.DTOs;
using ProductStore.Models;
using ProductStore.Repositories;

namespace ProductStoreTests.Repositories
{
    [TestClass]
    public class PurchaseProposalRepositoryTests
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
        public async Task GetPurchaseProposals_ReturnsProposals_WhenTheyExist()
        {
            using var dbContext = CreateDbContext();
            dbContext.PurchaseProposals.Add(new PurchaseProposal { Id = 1, CustomerId = 1, ProductId = 1, Date = DateTime.UtcNow });
            dbContext.SaveChanges();
            var repository = new PurchaseProposalRepository(dbContext);

            var result = await repository.GetPurchaseProposals();

            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task GetPurchaseProposals_ReturnsEmpty_WhenNoProposalsExist()
        {
            using var dbContext = CreateDbContext();
            var repository = new PurchaseProposalRepository(dbContext);

            var result = await repository.GetPurchaseProposals();

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task GetPurchaseProposal_ReturnsProposal_WhenItExists()
        {
            using var dbContext = CreateDbContext();
            dbContext.PurchaseProposals.Add(new PurchaseProposal { Id = 1, CustomerId = 1, ProductId = 1, Date = DateTime.UtcNow });
            dbContext.SaveChanges();
            var repository = new PurchaseProposalRepository(dbContext);

            var result = await repository.GetPurchaseProposal(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task GetPurchaseProposal_ReturnsNull_WhenProposalDoesNotExist()
        {
            using var dbContext = CreateDbContext();
            var repository = new PurchaseProposalRepository(dbContext);

            var result = await repository.GetPurchaseProposal(99);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetPurchaseProposalResponse_ReturnsProposal_WhenItExists()
        {
            using var dbContext = CreateDbContext();
            dbContext.PurchaseProposals.Add(new PurchaseProposal { Id = 1, CustomerId = 1, ProductId = 1, Date = DateTime.UtcNow });
            dbContext.SaveChanges();
            var repository = new PurchaseProposalRepository(dbContext);

            var result = await repository.GetPurchaseProposalResponse(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task GetPurchaseProposalResponse_ReturnsNull_WhenProposalDoesNotExist()
        {
            using var dbContext = CreateDbContext();
            var repository = new PurchaseProposalRepository(dbContext);

            var result = await repository.GetPurchaseProposalResponse(99);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task PostPurchaseProposal_AddsNewProposal()
        {
            using var dbContext = CreateDbContext();
            var repository = new PurchaseProposalRepository(dbContext);
            var newProposalRequest = new PurchaseProposalRequest { CustomerId = 2, ProductId = 2, Date = DateTime.UtcNow };

            var newProposal = await repository.PostPurchaseProposal(newProposalRequest);

            Assert.IsNotNull(newProposal);
            Assert.AreEqual(2, newProposal.CustomerId);
            Assert.AreEqual(2, newProposal.ProductId);
        }

        [TestMethod]
        public async Task DeletePurchaseProposal_DeletesProposal_WhenItExists()
        {
            using var dbContext = CreateDbContext();
            dbContext.PurchaseProposals.Add(new PurchaseProposal { Id = 1, CustomerId = 1, ProductId = 1, Date = DateTime.UtcNow });
            dbContext.SaveChanges();
            var repository = new PurchaseProposalRepository(dbContext);
            var proposalToDelete = await repository.GetPurchaseProposal(1);

            var deletedProposal = await repository.DeletePurchaseProposal(proposalToDelete);

            Assert.AreEqual(proposalToDelete.Id, deletedProposal.Id);
            Assert.IsFalse(dbContext.PurchaseProposals.Any(p => p.Id == proposalToDelete.Id));
        }


    }

}
