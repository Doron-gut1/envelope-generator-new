using System.Data;

namespace EnvelopeGenerator.Core.Interfaces;

/// <summary>
/// Factory for creating database connections
/// </summary>
public interface IConnectionFactory
{
    /// <summary>
    /// Creates a database connection from ODBC name
    /// </summary>
    IDbConnection CreateConnection(string odbcName);
}