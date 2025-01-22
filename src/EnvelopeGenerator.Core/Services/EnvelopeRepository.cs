using Dapper;
using System.Data.SqlClient;
using EnvelopeGenerator.Core.Interfaces;
using EnvelopeGenerator.Core.Models;
using System.Text;

namespace EnvelopeGenerator.Core.Services;

/// <summary>
/// Repository implementation for accessing envelope data
/// </summary>
public class EnvelopeRepository : IEnvelopeRepository
{
    private readonly SqlConnectionProvider _connectionProvider;

    public EnvelopeRepository(SqlConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<VoucherData>> GetVoucherDataAsync(EnvelopeParams parameters)
    {
        // Get envelope structure for building the query
        var structure = await GetEnvelopeStructureAsync(parameters.EnvelopeType);
        var sql = BuildSqlQuery(structure.Fields);

        using var connection = new SqlConnection(_connectionProvider.ConnectionString);
        var result = await connection.QueryAsync<dynamic>(sql,
            new
            {
                parameters.ActionType,
                parameters.EnvelopeType,
                parameters.BatchNumber,
                parameters.FamilyCode,
                parameters.ClosureNumber,
                parameters.VoucherGroup
            },
            commandType: System.Data.CommandType.StoredProcedure);

        // Convert dynamic results to VoucherData
        return result.Select(row =>
        {
            var voucherData = new VoucherData
            {
                Mspkod = row.mspkod,
                ManaHovNum = row.manahovnum,
                MtfNum = row.mtfnum,
                ShovarMsp = row.shovarmsp,
                Miun = row.miun,
                UniqNum = row.uniqnum,
                Shnati = row.shnati
            };

            // Add dynamic fields based on structure definition
            foreach (var field in structure.Fields)
            {
                if (!field.Name.Contains("simanenu") && !field.Name.Contains("rek"))
                {
                    var value = ((IDictionary<string, object>)row)[field.Name];
                    if (value != null)
                    {
                        voucherData.SetField(field.Name, value);
                    }
                }
            }

            return voucherData;
        });
    }

    private string BuildSqlQuery(IEnumerable<FieldDefinition> fields)
    {
        var queryBuilder = new StringBuilder();
        queryBuilder.AppendLine("SELECT");
        queryBuilder.AppendLine("    sh.mspkod,");
        queryBuilder.AppendLine("    sh.manahovnum,");
        queryBuilder.AppendLine("    sl.mtfnum,");
        queryBuilder.AppendLine("    sh.shovarmsp,");
        queryBuilder.AppendLine("    sh.shnati,");
        queryBuilder.AppendLine("    IIF(ISNULL(sh.shovarmsp, 0) = 0, sh.hskod, '0') AS miun,");
        queryBuilder.AppendLine("    sh.uniqnum");

        foreach (var field in fields)
        {
            if (!field.Name.Contains("simanenu") && !field.Name.Contains("rek"))
            {
                var tablePrefix = field.Source switch
                {
                    DataSource.ShovarHead => "sh.",
                    DataSource.ShovarLines => "sl.",
                    DataSource.None => "",
                    DataSource.ShovarHeadDynamic => "shd.",
                    DataSource.ShovarHeadNx => "shn.",
                    _ => throw new ArgumentException($"Unknown data source: {field.Source}")
                };

                queryBuilder.AppendLine($"    ,{tablePrefix}{field.Name}");
            }
        }

        queryBuilder.AppendLine("FROM shovarhead sh");
        queryBuilder.AppendLine("INNER JOIN shovarlines sl ON sh.shovar = sl.shovar");
        queryBuilder.AppendLine("LEFT JOIN shovarheadnx shn ON sh.shovar = shn.shovar");
        queryBuilder.AppendLine("LEFT JOIN shovarheadDynamic shd ON sh.shovar = shd.shovar");
        queryBuilder.AppendLine("WHERE sh.mnt = @BatchNumber");
        queryBuilder.AppendLine("    AND (sndto < CASE WHEN ISNULL((SELECT PrintEmailMtf FROM param3), 0) = 0 THEN 3 ELSE 4 END OR shnati <> 0)");
        queryBuilder.AppendLine("    AND (@FamilyCode IS NULL OR sh.mspkod = @FamilyCode)");
        queryBuilder.AppendLine("    AND (@ClosureNumber IS NULL OR sh.sgrnum = @ClosureNumber)");
        queryBuilder.AppendLine("    AND (@VoucherGroup IS NULL OR sh.kvuzashovar = @VoucherGroup)");
        queryBuilder.AppendLine("ORDER BY");
        queryBuilder.AppendLine("    sh.nameinsvr,");
        queryBuilder.AppendLine("    sh.mspkod,");
        queryBuilder.AppendLine("    IIF(ISNULL(sh.shovarmsp, 0) = 0, sh.hskod, '0'),");
        queryBuilder.AppendLine("    sl.mtfnum,");
        queryBuilder.AppendLine("    sh.manahovnum");

        return queryBuilder.ToString();
    }

    /// <inheritdoc />
    public async Task<EnvelopeStructure> GetEnvelopeStructureAsync(int envelopeType)
    {
        using var connection = new SqlConnection(_connectionProvider.ConnectionString);
        using var multi = await connection.QueryMultipleAsync(
            "GetEnvelopeStructure",
            new { EnvelopeType = envelopeType },
            commandType: System.Data.CommandType.StoredProcedure);

        var fields = await multi.ReadAsync<FieldDefinition>();
        var parameters = await multi.ReadFirstAsync<EnvelopeStructure>();

        return new EnvelopeStructure
        {
            Fields = fields.ToList(),
            NumOfDigits = parameters.NumOfDigits,
            PositionOfShnati = parameters.PositionOfShnati,
            DosHebrewEncoding = parameters.DosHebrewEncoding,
            NumOfPerutLines = parameters.NumOfPerutLines,
            NumOfPerutFields = fields.Count(f => f.Name.StartsWith("sm"))
        };
    }

    /// <inheritdoc />
    public async Task<SystemParameters> GetSystemParametersAsync()
    {
        using var connection = new SqlConnection(_connectionProvider.ConnectionString);
        return await connection.QueryFirstAsync<SystemParameters>(
            "GetSystemParams",
            commandType: System.Data.CommandType.StoredProcedure);
    }
}