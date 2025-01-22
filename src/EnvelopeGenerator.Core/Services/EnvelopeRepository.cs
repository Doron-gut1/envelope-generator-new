using Dapper;
using System.Data.SqlClient;
using EnvelopeGenerator.Core.Interfaces;
using EnvelopeGenerator.Core.Models;

namespace EnvelopeGenerator.Core.Services;

/// <summary>
/// Repository implementation for accessing envelope data
/// </summary>
public class EnvelopeRepository : IEnvelopeRepository
{
    private readonly string _connectionString;

    public EnvelopeRepository(SqlConnectionProvider connectionProvider)
    {
        _connectionString = connectionProvider.ConnectionString;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<VoucherData>> GetVoucherDataAsync(EnvelopeParams parameters)
    {
        using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<VoucherData>(
            "GetVoucherData",
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

        return result.Select(row =>
        {
            var voucherData = new VoucherData
            {
                Mspkod = row.Mspkod,
                ManaHovNum = row.ManaHovNum,
                MtfNum = row.MtfNum,
                ShovarMsp = row.ShovarMsp,
                Miun = row.Miun,
                UniqNum = row.UniqNum,
                Shnati = row.Shnati
            };

            // Add dynamic fields
            var dynamicObject = (IDictionary<string, object>)row;
            foreach (var field in dynamicObject)
            {
                voucherData.DynamicFields[field.Key] = field.Value;
            }

            return voucherData;
        });
    }

    /// <inheritdoc />
    public async Task<EnvelopeStructure> GetEnvelopeStructureAsync(int envelopeType)
    {
        using var connection = new SqlConnection(_connectionString);
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
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstAsync<SystemParameters>(
            "GetSystemParams",
            commandType: System.Data.CommandType.StoredProcedure);
    }
}