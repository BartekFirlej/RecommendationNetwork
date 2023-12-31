using RecommendationNetwork.DTOs;
using RecommendationNetwork.Repositories;

namespace RecommendationNetwork.Services
{
    public interface IVoivodeshipService
    {
        public Task<VoivodeshipResponse> AddVoivodeship(VoivodeshipRequest voivodeshipToAdd);
        public Task<List<VoivodeshipResponse>> GetVoivodeships();
        public Task<VoivodeshipResponse> GetVoivodeship(int id);
    }
    public class VoivodeshipService : IVoivodeshipService
    {
        private readonly IVoivodeshipRepository _voivodeshipRepository;
        private readonly RabbitMqVoivodeshipConsumer _rabbitMqConsumer;
        public VoivodeshipService(IVoivodeshipRepository voivodeshipRepository, RabbitMqVoivodeshipConsumer rabbitMqConsumer)
        {
            _voivodeshipRepository = voivodeshipRepository;
            _rabbitMqConsumer = rabbitMqConsumer;


            Console.WriteLine("ZASUBOWAŁEM VOIVODESHIP");
            _rabbitMqConsumer.VoivodeshipAdded += OnVoivodeshipAdded;
            _rabbitMqConsumer.StartConsuming("voivodeshipQueue");
        }

        private async void OnVoivodeshipAdded(object sender, VoivodeshipRequest voivodeshipToAdd)
        {
            Console.WriteLine("CONSUMING MESSAGE VOIVODESHIP");
            var createdNode = await AddVoivodeship(voivodeshipToAdd);
        }

        public async Task<VoivodeshipResponse> AddVoivodeship(VoivodeshipRequest voivodeshipToAdd)
        {
            return await _voivodeshipRepository.AddVoivodeship(voivodeshipToAdd);
        }

        public async Task<List<VoivodeshipResponse>> GetVoivodeships()
        {
            return await _voivodeshipRepository.GetVoivodeships();
        }

        public async Task<VoivodeshipResponse> GetVoivodeship(int id)
        {
            return await _voivodeshipRepository.GetVoivodeship(id);
        }
    }
}
