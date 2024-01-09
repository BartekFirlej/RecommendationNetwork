namespace RecommendationNetwork.DTOs
{
    public class PurchaseWithDetailsRequest
    {
        public int Id { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public List<PurchaseDetailRequest> PurchaseDetails { get; set; }
        public int CustomerId { get; set; }
        public int? RecommenderId { get; set; }
    }
}