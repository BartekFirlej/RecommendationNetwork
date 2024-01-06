namespace RecommendationNetwork.DTOs
{
    public class PurchaseIdDetailRequest
    {
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float PriceForOnePiece { get; set; }
    }
}
