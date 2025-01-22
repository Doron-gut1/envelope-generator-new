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
        OdbcConverter.OdbcConverter odbcConvert = new OdbcConverter.OdbcConverter();
        string connectionString = odbcConvert.GetSqlConnectionString(odbcName, "", "");
        return new OdbcConnection(connectionString);
    }
}