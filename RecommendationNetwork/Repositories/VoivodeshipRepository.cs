using Neo4j.Driver;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;

namespace RecommendationNetwork.Repositories
{
    public interface IVoivodeshipRepository
    {
        public Task<VoivodeshipResponse> AddVoivodeship(VoivodeshipRequest voivodeshipToAdd);
        public Task<List<VoivodeshipResponse>> GetVoivodeships();
        public Task<VoivodeshipResponse> GetVoivodeship(int id);
    }
    public class VoivodeshipRepository : IVoivodeshipRepository
    {
        private readonly IDriver _driver;
        public VoivodeshipRepository(IDriver driver)
        {
            _driver = driver;
        }

        private static VoivodeshipResponse MapToVoivodeshipResponse(IRecord record)
        {
            var properties = record["v"].As<INode>().Properties;

            var voivodeshipResponse = new VoivodeshipResponse
            {
                Id = properties["Id"].As<int>(),
                Name = properties["Name"].As<string>()
            };
            return voivodeshipResponse;
        }

        public async Task<VoivodeshipResponse> AddVoivodeship(VoivodeshipRequest voivodeshipToAdd)
        {
            using (var session = _driver.AsyncSession())
            {
                var addVoivodeshipQuery = "CREATE (v:Voivodeship {Id: $Id, Name: $Name}) RETURN v";
                var parameters = voivodeshipToAdd;

                var result = await session.WriteTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(addVoivodeshipQuery, parameters);
                    return await queryResult.SingleAsync();
                });

                var voivodeshipResponse = MapToVoivodeshipResponse(result);
                
                return voivodeshipResponse;
            }
        }

        public async Task<List<VoivodeshipResponse>> GetVoivodeships()
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveNodesCypher = "MATCH (v:Voivodeship) RETURN v";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    try
                    {
                        var queryResult = await transaction.RunAsync(retrieveNodesCypher);
                        return await queryResult.ToListAsync();
                    }
                    catch
                    {
                        throw new NotFoundVoivodeshipException();
                    }
                });

                var voivodeshipResponse = result.Select(record => MapToVoivodeshipResponse(record)).ToList();

                return voivodeshipResponse;
            }
        }

        public async Task<VoivodeshipResponse> GetVoivodeship(int id)
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveNodesCypher = "MATCH (v:Voivodeship) WHERE v.Id=$id RETURN v";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    try
                    {
                        var queryResult = await transaction.RunAsync(retrieveNodesCypher, new { id });
                        return await queryResult.SingleAsync();
                    }
                    catch
                    {
                        throw new NotFoundVoivodeshipException(id);
                    }
                });

                var voivodeshipResponse = MapToVoivodeshipResponse(result);

                return voivodeshipResponse;
            }
        }
    }
}
