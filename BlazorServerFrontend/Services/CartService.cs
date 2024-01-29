using BlazorServerFrontend.DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorServerFrontend.Services
{
    public class CartService
    {
        private readonly HttpClient _httpClient;

        public CartService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CartItem>> GetCart(string key)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<CartItem>>($"http://localhost:8082/cart/{key}");
            }
            catch (HttpRequestException e)
            {
                return new List<CartItem>();
            }
        }

        public async Task AddItemsToCart(string key, List<CartItem> newItems)
        {
            await _httpClient.PostAsJsonAsync($"http://localhost:8082/cart/{key}", newItems);
        }

        public async Task AddItemToCart(string key, CartItem newItem)
        {
            List<CartItem> newItems = new List<CartItem> { newItem };
            await _httpClient.PostAsJsonAsync($"http://localhost:8082/cart/{key}", newItems);
        }

        public async Task EmptyCart(string key)
        {
            await _httpClient.PutAsJsonAsync($"http://localhost:8082/cart/{key}", new List<CartItem>());
        }

        public async Task ModifyItemQuantity(string key, int itemId, int newQuantity)
        {

            await _httpClient.PutAsync($"http://localhost:8082/cart/{key}/{itemId}/{newQuantity}", null);

        }

        public async Task DeleteCart(string key)
        {
            await _httpClient.DeleteAsync($"http://localhost:8082/cart/{key}");
        }

        public async Task DeleteCartItem(string key, int itemId)
        {
            await _httpClient.DeleteAsync($"http://localhost:8082/cart/{key}/{itemId}");
        }

        public List<CartProduct> JoinLists(List<CartItem> listOfProductsInCarts, List<ProductResponse> listOfProductsDetails)
        {
            return   listOfProductsInCarts.Join(
                listOfProductsDetails,
                p => p.ItemId,
                pd => pd.Id,
                (p, pd) => new CartProduct
                {
                    ItemId = pd.Id,
                    Name = pd.Name,
                    ProductTypeId = pd.ProductTypeId,
                    ProductTypeName = pd.ProductTypeName,
                    ItemQuantity = p.ItemQuantity,
                    Price = pd.Price,
                }).ToList();
        }
    }
}
