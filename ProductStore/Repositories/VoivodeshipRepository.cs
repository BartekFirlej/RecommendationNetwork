using Microsoft.EntityFrameworkCore;
using ProductStore.DTOs;
using ProductStore.Models;

namespace ProductStore.Repositories
{
    public interface IVoivodeshipRepository
    {
        public Task<ICollection<VoivodeshipResponse>> GetVoivodeships();
        public Task<VoivodeshipResponse> GetVoivodeshipResponse(int id);
        public Task<Voivodeship> GetVoivodeship(int id);
        public Task<Voivodeship> DeleteVoivodeship(Voivodeship voivodeshipToDelete);
        public Task<Voivodeship> PostVoivodeship(VoivodeshipRequest voivodeshipToAdd);
    }
    public class VoivodeshipRepository : IVoivodeshipRepository
    {
        private readonly StoreDbContext _dbContext;

        public VoivodeshipRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<VoivodeshipResponse>> GetVoivodeships()
        {
            return await _dbContext.Voivodeships
                .Select(v => new VoivodeshipResponse
                {
                    Id = v.Id,
                    Name = v.Name
                }).ToListAsync();
        }

        public async Task<Voivodeship> GetVoivodeship(int id)
        {
            return await _dbContext.Voivodeships
                .Where(v => v.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<VoivodeshipResponse> GetVoivodeshipResponse(int id)
        {
            return await _dbContext.Voivodeships
                .Select(v => new VoivodeshipResponse
                {
                    Id = v.Id,
                    Name = v.Name
                })
                .Where(v => v.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Voivodeship> DeleteVoivodeship(Voivodeship voivodeshipToDelete)
        {
            _dbContext.Voivodeships.Remove(voivodeshipToDelete);
            await _dbContext.SaveChangesAsync();
            return voivodeshipToDelete;
        }

        public async Task<Voivodeship> PostVoivodeship(VoivodeshipRequest voivodeshipToAdd)
        {
            var newVoivodeship = new Voivodeship
            {
                Name = voivodeshipToAdd.Name
            };
            await _dbContext.Voivodeships.AddAsync(newVoivodeship);
            await _dbContext.SaveChangesAsync();
            return newVoivodeship;
        }
    }
}