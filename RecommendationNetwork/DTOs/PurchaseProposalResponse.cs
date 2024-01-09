namespace RecommendationNetwork.DTOs
{
    public class PurchaseProposalResponse
    {
        public DateTimeOffset Date { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
    }
}