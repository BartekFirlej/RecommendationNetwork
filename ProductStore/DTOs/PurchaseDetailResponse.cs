namespace ProductStore.DTOs
{
    public class PurchaseDetailResponse
    {
        public int Id {  get; set; }
        public int ProductId { get; set; }
        public int PurchaseId { get; set; }
        public int Quantity { get; set; }
        public float PriceForOnePiece { get; set; }
    }
}
