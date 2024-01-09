using Neo4j.Driver;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;

namespace RecommendationNetwork.Repositories
{
    public interface IPurchaseDetailRepository
    {
        public Task<PurchaseIdDetailResponse> AddPurchaseDetail(PurchaseIdDetailRequest purchaseDetailToAdd);
        public Task<List<PurchaseIdDetailResponse>> GetPurchaseDetails();
        public Task<PurchaseIdDetailResponse> GetPurchaseDetail(int id);
    }
    public class PurchaseDetailRepository : IPurchaseDetailRepository
    {
        private readonly IDriver _driver;
        public PurchaseDetailRepository(IDriver driver)
        {
            _driver = driver;
        }

        private static PurchaseIdDetailResponse MapToPurchaseIdDetailResponse(IRecord record)
        {
            var propertiesPurchase = record["p"].As<INode>().Properties;
            var propertiesConsistOf = record["a"].As<IRelationship>().Properties;
            var propertiesProduct = record["pt"].As<INode>().Properties;

            var orderResponse = new PurchaseIdDetailResponse
            {
                Id = propertiesConsistOf["Id"].As<int>(),
                ProductId = propertiesProduct["Id"].As<int>(),
                PurchaseId = propertiesPurchase["Id"].As<int>(),
                Quantity = propertiesConsistOf["Quantity"].As<int>(),
                PriceForOnePiece = propertiesConsistOf["PriceForOnePiece"].As<float>()
            };

            return orderResponse;
        }

        public async Task<PurchaseIdDetailResponse> AddPurchaseDetail(PurchaseIdDetailRequest purchaseDetailToAdd)
        {
            using (var session = _driver.AsyncSession())
            {
                var addPurchaseDetail = "MATCH (p:Purchase {Id: $PurchaseId}), (pt:Product{Id:$ProductId}) MERGE (p)-[a:CONISTS_OF{Id: $Id, Quantity: $Quantity, PriceForOnePiece: $PriceForOnePiece}]->(pt) RETURN p,a,pt";
                var parameters = purchaseDetailToAdd;

                var addedPurchaseDetail = await session.WriteTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(addPurchaseDetail, parameters);
                    return await queryResult.SingleAsync();

                });

                var purchaseDetailResponse = MapToPurchaseIdDetailResponse(addedPurchaseDetail);
                return purchaseDetailResponse;
            }
        }

        public async Task<PurchaseIdDetailResponse> GetPurchaseDetail(int id)
        {
            using (var session = _driver.AsyncSession())
            {
                var retrievePurchaseDetails = "MATCH (p:Purchase)-[a:CONISTS_OF]->(pt:Product) WHERE a.Id=$id RETURN p, a, pt";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    try
                    {
                        var queryResult = await transaction.RunAsync(retrievePurchaseDetails);
                        return await queryResult.SingleAsync();
                    }
                    catch
                    {
                        throw new NotFoundPurchaseDetailException(id);
                    }
                });

                var ordersResponse = MapToPurchaseIdDetailResponse(result);

                return ordersResponse;
            }
        }

        public async Task<List<PurchaseIdDetailResponse>> GetPurchaseDetails()
        {
            using (var session = _driver.AsyncSession())
            {
                var retrievePurchaseDetails = "MATCH (p:Purchase)-[a:CONISTS_OF]->(pt:Product) RETURN p, a, pt";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(retrievePurchaseDetails);
                    return await queryResult.ToListAsync();
                });

                if (!result.Any())
                    throw new NotFoundPurchaseDetailException();

                var ordersResponse = result.Select(record => MapToPurchaseIdDetailResponse(record)).ToList();

                return ordersResponse;
            }
        }
    }
}   