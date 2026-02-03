using System.Data;

namespace BuildingBlocks.Application.Data;

public interface ISqlConnectionFactory
{
    IDbConnection CreateNewConnect();
    IDbConnection GetOpenConnection();
    string        GetConnectionString();
}