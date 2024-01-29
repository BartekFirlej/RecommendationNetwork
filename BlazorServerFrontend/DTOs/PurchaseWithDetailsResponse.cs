namespace BlazorServerFrontend.DTOs
{
    public class PurchaseWithDetailsResponse
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int CustomerId { get; set; }
        public int? RecommenderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerLastName { get; set; }
        public string? RecommenderName { get; set; }
        public string? RecommenderLastName { get; set; }
        public List<PurchaseDetailResponse> PurchaseDetails { get; set; }
    }
}
