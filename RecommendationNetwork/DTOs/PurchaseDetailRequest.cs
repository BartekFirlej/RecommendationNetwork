namespace RecommendationNetwork.DTOs
{
    public class PurchaseDetailRequest
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public float Quantity { get; set; }
        public float PriceForOnePiece { get; set; }
    }
}