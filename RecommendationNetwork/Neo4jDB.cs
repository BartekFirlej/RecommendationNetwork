using Neo4j.Driver;


public class Neo4jService
{
    private readonly IDriver _driver;

    public Neo4jService(string uri, string user, string password)
    {
        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
    }

    public IAsyncSession GetSession()
    {
        return _driver.AsyncSession();
    }

    public void Dispose()
    {
        _driver?.Dispose();

    }
}

