namespace BlazorServerFrontend.DTOs
{
    public class PurchaseDetailResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public int Quantity { get; set; }
        public float PriceForOnePiece { get; set; }
    }
}
