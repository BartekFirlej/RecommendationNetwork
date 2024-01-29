namespace ProductStore.DTOs
{
    public class PurchaseProposalResponse
    {
        public DateTimeOffset Date { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductName { get; set; }
        public string ProductTypeName { get; set; }
    }
}