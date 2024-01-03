using AutoMapper;
using Moq;
using ProductStore.DTOs;
using ProductStore.Models;
using ProductStore.Repositories;
using ProductStore.Services;

namespace ProductStoreTests.Services
{
    [TestClass]
    public class VoivodeshipServiceTests
    {
        private Mock<IVoivodeshipRepository> _mockVoivodeshipRepository;
        private Mock<IMapper> _mockMapper;
        private VoivodeshipService _service;

        [TestInitialize]
        public void SetUp()
        {
            _mockVoivodeshipRepository = new Mock<IVoivodeshipRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new VoivodeshipService(_mockVoivodeshipRepository.Object, _mockMapper.Object);
        }

        [TestMethod]
        public async Task GetVoivodeships_ReturnsVoivodeships_WhenFound()
        {
            var mockVoivodeships = new List<VoivodeshipResponse>
            {
                new VoivodeshipResponse { Id = 1, Name = "Voivodeship1" },
                new VoivodeshipResponse { Id = 2, Name = "Voivodeship2" }
            };
            _mockVoivodeshipRepository.Setup(repo => repo.GetVoivodeships()).ReturnsAsync(mockVoivodeships);

            var result = await _service.GetVoivodeships();

            Assert.IsNotNull(result);
            Assert.AreEqual(mockVoivodeships.Count, result.Count);
            Assert.AreEqual(mockVoivodeships.First().Name, result.First().Name);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Not found any voivodeship.")]
        public async Task GetVoivodeships_ThrowsException_WhenNotFound()
        {
            _mockVoivodeshipRepository.Setup(repo => repo.GetVoivodeships()).ReturnsAsync(new List<VoivodeshipResponse>());

            await _service.GetVoivodeships();
        }

        [TestMethod]
        public async Task GetVoivodeship_ReturnsVoivodeship_WhenFound()
        {
            int id = 1;
            var mockVoivodeship = new Voivodeship { Id = id, Name = "Voivodeship1" };
            _mockVoivodeshipRepository.Setup(repo => repo.GetVoivodeship(id)).ReturnsAsync(mockVoivodeship);

            var result = await _service.GetVoivodeship(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(mockVoivodeship.Name, result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetVoivodeship_ThrowsException_WhenNotFound()
        {
            int id = 1;
            _mockVoivodeshipRepository.Setup(repo => repo.GetVoivodeship(id)).ReturnsAsync((Voivodeship)null);

            await _service.GetVoivodeship(id);
        }

        [TestMethod]
        public async Task GetVoivodeshipResponse_ReturnsVoivodeshipResponse_WhenFound()
        {
            int id = 1;
            var mockVoivodeshipResponse = new VoivodeshipResponse { Id = id, Name = "Voivodeship1" };
            _mockVoivodeshipRepository.Setup(repo => repo.GetVoivodeshipResponse(id)).ReturnsAsync(mockVoivodeshipResponse);

            var result = await _service.GetVoivodeshipResponse(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(mockVoivodeshipResponse.Name, result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetVoivodeshipResponse_ThrowsException_WhenNotFound()
        {
            int id = 1;
            _mockVoivodeshipRepository.Setup(repo => repo.GetVoivodeshipResponse(id)).ReturnsAsync((VoivodeshipResponse)null);

            await _service.GetVoivodeshipResponse(id);
        }

        [TestMethod]
        public async Task PostVoivodeship_ReturnsVoivodeshipResponse_WhenSuccessful()
        {
            var voivodeshipRequest = new VoivodeshipRequest { Name = "Voivodeship1" };
            var addedVoivodeship = new Voivodeship { Id = 1, Name = "Voivodeship1" };
            var voivodeshipResponse = new VoivodeshipResponse { Id = 1, Name = "Voivodeship1" };

            _mockVoivodeshipRepository.Setup(repo => repo.PostVoivodeship(voivodeshipRequest)).ReturnsAsync(addedVoivodeship);
            _mockMapper.Setup(mapper => mapper.Map<VoivodeshipResponse>(addedVoivodeship)).Returns(voivodeshipResponse);

            var result = await _service.PostVoivodeship(voivodeshipRequest);

            Assert.IsNotNull(result);
            Assert.AreEqual(voivodeshipResponse.Name, result.Name);
        }

        [TestMethod]
        public async Task DeleteVoivodeship_ReturnsVoivodeshipResponse_WhenSuccessful()
        {
            int id = 1;
            var voivodeshipToDelete = new Voivodeship { Id = id, Name = "Voivodeship1" };
            var voivodeshipResponse = new VoivodeshipResponse { Id = id, Name = "Voivodeship1" };

            _mockVoivodeshipRepository.Setup(repo => repo.GetVoivodeship(id)).ReturnsAsync(voivodeshipToDelete);
            _mockVoivodeshipRepository.Setup(repo => repo.DeleteVoivodeship(voivodeshipToDelete)).ReturnsAsync(voivodeshipToDelete);
            _mockMapper.Setup(mapper => mapper.Map<VoivodeshipResponse>(voivodeshipToDelete)).Returns(voivodeshipResponse);

            var result = await _service.DeleteVoivodeship(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(voivodeshipToDelete.Name, result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task DeleteVoivodeship_ThrowsException_WhenVoivodeshipNotFound()
        {
            int id = 1;
            _mockVoivodeshipRepository.Setup(repo => repo.GetVoivodeship(id)).ReturnsAsync((Voivodeship)null);

            await _service.DeleteVoivodeship(id);
        }

    }
}
