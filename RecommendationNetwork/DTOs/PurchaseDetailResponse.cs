namespace RecommendationNetwork.DTOs
{
    public class PurchaseDetailResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public float Quantity { get; set; }
        public float PriceForOnePiece { get; set; }
    }
}