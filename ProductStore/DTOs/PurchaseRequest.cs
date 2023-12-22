namespace ProductStore.DTOs
{
    public class PurchaseRequest
    {
        public DateTime PurchaseDate { get; set; }

        public int CustomerId { get; set; }

        public int? RecommenderId { get; set; }
    }
}
