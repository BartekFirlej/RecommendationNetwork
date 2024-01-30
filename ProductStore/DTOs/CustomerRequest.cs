namespace ProductStore.DTOs
{
    public class CustomerRequest
    {
        public string Name { get; set; } = null!;

        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PIN { get; set; } = null!;

        public string Town { get; set; } = null!;

        public string ZipCode { get; set; } = null!;

        public string Street { get; set; } = null!;

        public string Country { get; set; } = null!;

        public int VoivodeshipId { get; set; }
        public int? RecommenderId { get; set; }
    }
}
