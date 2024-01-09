namespace RecommendationNetwork.DTOs
{
    public class PurchaseRecommendationResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int FirstLevelRecommendations { get; set; }
        public int SecondLevelRecommendations { get; set; }
        public int ThirdLevelRecommendations { get; set; }
    }
}