namespace RecommendationNetwork.DTOs
{
    public class OrderRequest
    {
        public int Id { get; set; }
        public DateTimeOffset OrderDate {  get; set; }
        public List<OrderDetailRequest> OrderDetails { get; set; }
        public int CustomerId {  get; set; }
        public int? RecommenderId {  get; set; }
    }
}
