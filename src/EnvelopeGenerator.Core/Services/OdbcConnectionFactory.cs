using System.Data;
using System.Data.Odbc;
using EnvelopeGenerator.Core.Interfaces;

namespace EnvelopeGenerator.Core.Services;

/// <summary>
/// Factory for creating ODBC database connections
/// </summary>
public class OdbcConnectionFactory : IConnectionFactory
{
    public IDbConnection CreateConnection(string odbcName)
    {
        var connectionString = $"DSN={odbcName};";
        return new OdbcConnection(connectionString);
    }
}