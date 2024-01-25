using BlazorServerFrontend.DTOs;

namespace BlazorServerFrontend.Services
{
    public class CartService
    {
        public List<CartItem> CartItems { get; private set; } = new List<CartItem>();

        public void AddToCart(CartItem product)
        {
            CartItems.Add(product);
        }

        public void EmptyCart()
        {
            CartItems = new List<CartItem>();
        }

        public void RemoveCartItem(int id)
        {
            foreach(var item in CartItems)
            {
                if(item.Product.Id == id)
                {
                    CartItems.Remove(item);
                }
                break;
            }
        }
    }

}
