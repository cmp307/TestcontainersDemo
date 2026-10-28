using Testcontainers.MySql;

namespace Assets.Tests;

public sealed class AssetServiceTest : IAsyncLifetime
{
    private readonly MySqlContainer _mysql = new MySqlBuilder("mysql")
        .Build();

    public Task InitializeAsync()
    {
        return _mysql.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _mysql.DisposeAsync().AsTask();
    }

    [Fact]
    public void ShouldReturnTwoAssets()
    {
        // Given
        var assetService = new AssetService(new DbConnectionProvider(_mysql.GetConnectionString()));

        // When
        assetService.Create(new Asset(1, "LT10001"));
        assetService.Create(new Asset(2, "LT10002"));
        var assets = assetService.GetAssets();

        // Then
        Assert.Fail(); // Assert.Equal(2, assets.Count());
    }
}