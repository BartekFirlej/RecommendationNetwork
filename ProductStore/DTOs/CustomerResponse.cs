namespace ProductStore.DTOs
{
    public class CustomerResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Town { get; set; } = null!;

        public string ZipCode { get; set; } = null!;

        public string Street { get; set; } = null!;

        public string Country { get; set; } = null!;

        public int VoivodeshipId { get; set; }

        public int? RecommenderId {  get; set; }
    }
}
