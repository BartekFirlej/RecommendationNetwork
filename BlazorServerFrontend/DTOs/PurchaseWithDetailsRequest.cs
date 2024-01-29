namespace BlazorServerFrontend.DTOs
{
    public class PurchaseWithDetailsRequest
    {
        public DateTime PurchaseDate { get; set; }

        public int CustomerId { get; set; }

        public int? RecommenderId { get; set; }
        public List<PurchaseDetailRequest> Products { get; set; }
    }
}
