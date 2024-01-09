namespace RecommendationNetwork.DTOs
{
    public class CustomerRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int VoivodeshipId { get; set; }
        public int? RecommenderId { get; set; }
    }
}