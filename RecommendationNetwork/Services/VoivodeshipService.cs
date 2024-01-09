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
        public VoivodeshipService(IVoivodeshipRepository voivodeshipRepository)
        {
            _voivodeshipRepository = voivodeshipRepository;
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