namespace RecommendationNetwork.DTOs
{
    public class CustomerRecommendationResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int FirstLevelRecommendations {  get; set; }
        public int SecondLevelRecommendations {  get; set; }
        public int ThirdLevelRecommendations {  get; set; }
    }
}
