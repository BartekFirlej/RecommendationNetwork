namespace RecommendationNetwork.DTOs
{
    public class PurchaseRequest
    {
        public int Id { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public int CustomerId { get; set; }
        public int? RecommenderId { get; set; }
    }
}