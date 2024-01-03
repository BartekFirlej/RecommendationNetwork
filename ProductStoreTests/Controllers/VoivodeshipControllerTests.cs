using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductStore.Controllers;
using ProductStore.DTOs;
using ProductStore.Services;

namespace ProductStoreTests.Controllers
{
    [TestClass]
    public class VoivodeshipControllerTests
    {
        private Mock<IVoivodeshipService> _mockVoivodeshipService;
        private VoivodeshipController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockVoivodeshipService = new Mock<IVoivodeshipService>();
            _controller = new VoivodeshipController(_mockVoivodeshipService.Object);
        }

        [TestMethod]
        public async Task GetVoivodeships_ReturnsOkWithVoivodeships()
        {
            var mockVoivodeships = new List<VoivodeshipResponse>
        {
            new VoivodeshipResponse { Id = 1, Name = "Voivodeship1" },
            new VoivodeshipResponse { Id = 2, Name = "Voivodeship2" }
        };
            _mockVoivodeshipService.Setup(service => service.GetVoivodeships()).ReturnsAsync(mockVoivodeships);

            var result = await _controller.GetVoivodeships();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(mockVoivodeships, okResult.Value);
        }

        [TestMethod]
        public async Task GetVoivodeships_ReturnsNotFound_WhenExceptionThrown()
        {
            _mockVoivodeshipService.Setup(service => service.GetVoivodeships()).ThrowsAsync(new Exception("Not Found"));

            var result = await _controller.GetVoivodeships();

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Not Found", notFoundResult.Value);
        }

        [TestMethod]
        public async Task GetVoivodeship_ReturnsOkWithVoivodeship()
        {
            int id = 1;
            var mockVoivodeship = new VoivodeshipResponse { Id = id, Name = "Voivodeship1" };
            _mockVoivodeshipService.Setup(service => service.GetVoivodeshipResponse(id)).ReturnsAsync(mockVoivodeship);

            var result = await _controller.GetVoivodeship(id);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(mockVoivodeship, okResult.Value);
        }

        [TestMethod]
        public async Task GetVoivodeship_ReturnsNotFound_WhenExceptionThrown()
        {
            int id = 1;
            _mockVoivodeshipService.Setup(service => service.GetVoivodeshipResponse(id)).ThrowsAsync(new Exception("Not Found"));

            var result = await _controller.GetVoivodeship(id);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Not Found", notFoundResult.Value);
        }

        [TestMethod]
        public async Task PostVoivodeship_ReturnsOkWithVoivodeship()
        {
            var voivodeshipToAdd = new VoivodeshipRequest { Name = "Voivodeship1" };
            var addedVoivodeship = new VoivodeshipResponse { Id = 1, Name = "Voivodeship1" };
            _mockVoivodeshipService.Setup(service => service.PostVoivodeship(voivodeshipToAdd)).ReturnsAsync(addedVoivodeship);

            var result = await _controller.PostVoivodeship(voivodeshipToAdd);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(addedVoivodeship, okResult.Value);
        }

        [TestMethod]
        public async Task PostVoivodeship_ReturnsNotFound_WhenExceptionThrown()
        {
            var voivodeshipToAdd = new VoivodeshipRequest { Name = "Voivodeship1" };
            _mockVoivodeshipService.Setup(service => service.PostVoivodeship(voivodeshipToAdd)).ThrowsAsync(new Exception("Not Found"));

            var result = await _controller.PostVoivodeship(voivodeshipToAdd);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Not Found", notFoundResult.Value);
        }

        [TestMethod]
        public async Task DeleteVoivodeship_ReturnsOkWithVoivodeship()
        {
            int id = 1;
            var deletedVoivodeship = new VoivodeshipResponse { Id = 1, Name = "Voivodeship1" };
            _mockVoivodeshipService.Setup(service => service.DeleteVoivodeship(id)).ReturnsAsync(deletedVoivodeship);

            var result = await _controller.DeleteVoivodeship(id);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(deletedVoivodeship, okResult.Value);
        }

        [TestMethod]
        public async Task DeleteVoivodeship_ReturnsNotFound_WhenExceptionThrown()
        {
            int id = 1;
            _mockVoivodeshipService.Setup(service => service.DeleteVoivodeship(id)).ThrowsAsync(new Exception("Not Found"));

            var result = await _controller.DeleteVoivodeship(id);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Not Found", notFoundResult.Value);
        }

    }

}
