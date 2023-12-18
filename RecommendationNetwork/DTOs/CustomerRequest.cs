namespace RecommendationNetwork.DTOs
{
    public class CustomerRequest
    {
        public string PESEL { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public int VoivodeshipId { get; set; }
    }
}
