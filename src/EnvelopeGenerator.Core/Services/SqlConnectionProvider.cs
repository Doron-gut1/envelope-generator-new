namespace EnvelopeGenerator.Core.Services;

/// <summary>
/// Provides SQL connection string for the entire application
/// </summary>
public class SqlConnectionProvider
{
    /// <summary>
    /// Gets the SQL connection string
    /// </summary>
    public string ConnectionString { get; }

    /// <summary>
    /// Initializes connection string from ODBC name
    /// </summary>
    public SqlConnectionProvider(string odbcName)
    {
        var odbcConvert = new OdbcConverter.OdbcConverter();
        ConnectionString = odbcConvert.GetSqlConnectionString(odbcName, "", "");
    }
}