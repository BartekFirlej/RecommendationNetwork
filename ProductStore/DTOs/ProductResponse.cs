namespace ProductStore.DTOs
{
    public class ProductResponse
    {
        public int Id { get; set; } 

        public string Name { get; set; }

        public float Price { get; set; }

        public int ProductTypeId { get; set; }

        public string ProductTypeName {  get; set; }
    }
}
