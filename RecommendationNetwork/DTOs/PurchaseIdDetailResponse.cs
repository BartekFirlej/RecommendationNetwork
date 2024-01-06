namespace RecommendationNetwork.DTOs
{
    public class PurchaseIdDetailResponse
    {
        public int ProductId { get; set; }
        public int PurchaseId { get; set; }
        public float Quantity { get; set; }
        public float PriceForOnePiece { get; set; }
    }
}
