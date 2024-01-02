using AutoMapper;
using ProductStore.DTOs;
using ProductStore.Models;
using ProductStore.Repositories;

namespace ProductStore.Services
{
    public interface IVoivodeshipService {
        public Task<ICollection<VoivodeshipResponse>> GetVoivodeships();
        public Task<VoivodeshipResponse> GetVoivodeshipResponse(int id);
        public Task<Voivodeship> GetVoivodeship(int id);
        public Task<VoivodeshipResponse> DeleteVoivodeship(int id);
        public Task<VoivodeshipResponse> PostVoivodeship(VoivodeshipRequest voivodeshipToAdd);
    }
    public class VoivodeshipService : IVoivodeshipService
    {
        private readonly IVoivodeshipRepository _voivodeshipRepository;
        private readonly IMapper _mapper;
        public VoivodeshipService(IVoivodeshipRepository voivodeshipRepository, IMapper mapper)
        {
            _voivodeshipRepository = voivodeshipRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<VoivodeshipResponse>> GetVoivodeships()
        {
            var voivodeships = await _voivodeshipRepository.GetVoivodeships();
            if (!voivodeships.Any())
                throw new Exception("Not found any voivodeship.");
            return voivodeships;
        }

        public async Task<Voivodeship> GetVoivodeship(int id)
        {
            var voivodeship = await _voivodeshipRepository.GetVoivodeship(id);
            if (voivodeship == null)
                throw new Exception(String.Format("Not found voivodeship with id {0}.",id));
            return voivodeship;
        }

        public async Task<VoivodeshipResponse> GetVoivodeshipResponse(int id)
        {
            var voivodeship = await _voivodeshipRepository.GetVoivodeshipResponse(id);
            if (voivodeship == null)
                throw new Exception(String.Format("Not found voivodeship with id {0}.", id));
            return voivodeship;
        }

        public async Task<VoivodeshipResponse> DeleteVoivodeship(int id)
        {
            var voivodeshipToDelete = await GetVoivodeship(id);
            await _voivodeshipRepository.DeleteVoivodeship(voivodeshipToDelete);
            return _mapper.Map<VoivodeshipResponse>(voivodeshipToDelete);
        }

        public async Task<VoivodeshipResponse> PostVoivodeship(VoivodeshipRequest voivodeshipToAdd)
        {
            var addedVoivodeship = await _voivodeshipRepository.PostVoivodeship(voivodeshipToAdd);
            return _mapper.Map<VoivodeshipResponse>(addedVoivodeship);
        }
    }
}
