namespace ProductStore.DTOs
{
    public class PurchaseProposalResponse
    {
        public int Id {  get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
    }
}
