namespace ProductStore.DTOs
{
    public class PurchaseIdDetailRequest
    {
        public int ProductId { get; set; }
        public int PurchaseId { get; set; }
        public int Quantity { get; set; }
        public float PriceForOnePiece {  get; set; }
    }
}
