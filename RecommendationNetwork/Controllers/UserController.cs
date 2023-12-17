using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private IDriver _driver;

    public UserController(IDriver driver)
    {
        _driver = driver;
    }

    [HttpPost]
    [Route("create-node")]
    public async Task<IActionResult> CreateNodeAsync()
    {
        try
        {
            // Assuming you have a pre-configured IDriver instance

            // Create a session to execute the query
            using (var session = _driver.AsyncSession())
            {
                var cypherQuery = "CREATE (n:Person {name: $name, age: $age})";
                var parameters = new { name = "Bartek", age = 21 };

                await session.WriteTransactionAsync(async transaction =>
                {
                    await transaction.RunAsync(cypherQuery, parameters);
                });
            }

            return Ok(new { Message = "Node created successfully" });
        }
        catch (Exception ex)
        {
            // Log or handle the exception appropriately
            return BadRequest(new { Message = "Error creating node", Error = ex.Message });
        }
    }
}
