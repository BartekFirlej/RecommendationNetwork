namespace ProductStore.DTOs
{
    public class PurchaseWithDetailsResponse
    {
        public int Id { get; set; }

        public DateTime PurchaseDate { get; set; }

        public int CustomerId { get; set; }

        public int? RecommenderId { get; set; }

        public List<PurchaseDetailResponse> Products { get; set; }
    }
}