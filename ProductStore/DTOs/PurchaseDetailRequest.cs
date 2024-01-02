namespace ProductStore.DTOs
{
    public class PurchaseDetailRequest
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public float PriceForOnePiece { get; set; }

    }
}