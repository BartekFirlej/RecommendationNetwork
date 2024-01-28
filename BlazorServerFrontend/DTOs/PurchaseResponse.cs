namespace BlazorServerFrontend.DTOs
{
    public class PurchaseResponse
    {
        public int Id { get; set; }

        public DateTime PurchaseDate { get; set; }

        public float Amount { get; set; }

        public int CustomerId { get; set; }

        public int? RecommenderId { get; set; }
    }
}
