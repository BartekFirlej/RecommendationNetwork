namespace ProductStore.DTOs
{
    public class ProductPostResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public int ProductTypeId { get; set; }
    }
}
