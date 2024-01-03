namespace ProductStore.DTOs
{
    public class PurchaseProposalRequest
    {
        public DateTime Date {  get; set; }
        public int CustomerId { get; set; }
        public int ProductId {  get; set; }
    }
}
