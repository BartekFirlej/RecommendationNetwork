namespace BlazorServerFrontend.DTOs
{
    public class CartItem
    {
        public int Quantity { get; set; }

        public ProductResponse Product { get; set; } = null!;
    }
}
