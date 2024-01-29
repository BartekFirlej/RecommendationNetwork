namespace BlazorServerFrontend.DTOs
{
    public class CartProduct
    {
        public int ItemQuantity { get; set; }

        public int ItemId { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public int ProductTypeId { get; set; }

        public string ProductTypeName { get; set; }
    }
}
