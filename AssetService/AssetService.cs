namespace Assets;

public sealed class AssetService
{
    private readonly DbConnectionProvider _dbConnectionProvider;

    public AssetService(DbConnectionProvider dbConnectionProvider)
    {
        _dbConnectionProvider = dbConnectionProvider;
        CreateAssetsTable();
    }

    public IEnumerable<Asset> GetAssets()
    {
        IList<Asset> assets = new List<Asset>();

        using var connection = _dbConnectionProvider.GetConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT id, systemName FROM assets";
        command.Connection?.Open();

        using var dataReader = command.ExecuteReader();
        while (dataReader.Read())
        {
            var id = dataReader.GetInt64(0);
            var systemName = dataReader.GetString(1);
            assets.Add(new Asset(id, systemName));
        }

        return assets;
    }

    public void Create(Asset asset)
    {
        using var connection = _dbConnectionProvider.GetConnection();
        using var command = connection.CreateCommand();

        var id = command.CreateParameter();
        id.ParameterName = "@id";
        id.Value = asset.Id;

        var systemName = command.CreateParameter();
        systemName.ParameterName = "@systemName";
        systemName.Value = asset.SystemName;

        command.CommandText = "INSERT INTO assets (id, systemName) VALUES(@id, @systemName)";
        command.Parameters.Add(id);
        command.Parameters.Add(systemName);
        command.Connection?.Open();
        command.ExecuteNonQuery();
    }

    private void CreateAssetsTable()
    {
        using var connection = _dbConnectionProvider.GetConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS assets (id BIGINT NOT NULL, systemName VARCHAR(255) NOT NULL, PRIMARY KEY (id))";
        command.Connection?.Open();
        command.ExecuteNonQuery();
    }
}