using System.Data;
using BuildingBlocks.Application.Data;
using Microsoft.Data.SqlClient;

namespace BuildingBlocks.Infrastructure;

public class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    private IDbConnection? _connection;

    public IDbConnection CreateNewConnect()
    {
        if (this._connection is null || this._connection.State != ConnectionState.Open)
        {
            this._connection = new SqlConnection(connectionString);
            this._connection.Open();
        }

        return this._connection;
    }

    public IDbConnection GetOpenConnection()
    {
        var connection = new SqlConnection(connectionString);
        connection.Open();
        return connection;
    }

    public string GetConnectionString()
    {
        return connectionString;
    }
}