using Cart;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

[Route("cart")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly IDatabase _redisDB;

    public CartController(IConnectionMultiplexer redisMultiplexer)
    {
        _redisDB = redisMultiplexer.GetDatabase();
    }

    [HttpPost("{key}")]
    public IActionResult Create(string key, [FromBody] List<CartItem> newItems)
    {
        List<CartItem> existingItems = new List<CartItem>();

        // Check if the key exists
        var serializedValue = _redisDB.StringGet(key);
        if (!serializedValue.IsNullOrEmpty)
        {
            // Deserialize the existing list
            existingItems = JsonSerializer.Deserialize<List<CartItem>>(serializedValue);
        }

        foreach (var newItem in newItems)
        {
            var existingItem = existingItems.FirstOrDefault(i => i.ItemId == newItem.ItemId);
            if (existingItem != null)
            {
                // Sum the quantities if item already exists
                existingItem.ItemQuantity += newItem.ItemQuantity;
            }
            else
            {
                // Add new item if it doesn't exist
                existingItems.Add(newItem);
            }
        }

        // Serialize and save the updated list
        serializedValue = JsonSerializer.Serialize(existingItems);
        _redisDB.StringSet(key, serializedValue);

        return Ok($"Items updated for key {key}.");
    }


    [HttpGet("{key}")]
    public IActionResult Read(string key)
    {
        var value = _redisDB.StringGet(key);
        if (value.IsNullOrEmpty) return NotFound($"Key {key} not found.");

        return Ok(value.ToString());
    }

    [HttpPut("{key}")]
    public IActionResult Update(string key, [FromBody] List<CartItem> newValue)
    {
        if (!_redisDB.KeyExists(key)) return NotFound($"Key {key} not found.");

        var serializedValue = JsonSerializer.Serialize(newValue);
        _redisDB.StringSet(key, serializedValue);
        return Ok($"Key {key} updated with new value.");
    }

    [HttpPut("{key}/{itemId}/{newQuantity}")]
    public IActionResult UpdateQuantity(string key, int itemId, int newQuantity)
    {
        var serializedValue = _redisDB.StringGet(key);
        if (serializedValue.IsNullOrEmpty) return NotFound($"Key {key} not found.");

        var items = JsonSerializer.Deserialize<List<CartItem>>(serializedValue);
        var itemToUpdate = items.FirstOrDefault(item => item.ItemId == itemId);

        if (itemToUpdate == null) return NotFound($"Item with ID {itemId} not found.");

        itemToUpdate.ItemQuantity = newQuantity;

        serializedValue = JsonSerializer.Serialize(items);
        _redisDB.StringSet(key, serializedValue);
        return Ok($"Quantity of item with ID {itemId} updated to {newQuantity}.");
    }

    [HttpDelete("{key}")]
    public IActionResult Delete(string key)
    {
        if (!_redisDB.KeyExists(key)) return NotFound($"Key {key} not found.");

        _redisDB.KeyDelete(key);
        return Ok($"Key {key} deleted.");
    }

    [HttpDelete("{key}/{itemId}")]
    public IActionResult DeleteItem(string key, int itemId)
    {
        var serializedValue = _redisDB.StringGet(key);
        if (serializedValue.IsNullOrEmpty) return NotFound($"Key {key} not found.");

        var items = JsonSerializer.Deserialize<List<CartItem>>(serializedValue);
        var itemToRemove = items.FirstOrDefault(item => item.ItemId == itemId);

        if (itemToRemove == null) return NotFound($"Item with ID {itemId} not found.");

        items.Remove(itemToRemove);

        serializedValue = JsonSerializer.Serialize(items);
        _redisDB.StringSet(key, serializedValue);
        return Ok($"Item with ID {itemId} removed from key {key}.");
    }
}