using Dapper;
using EnvelopeGenerator.Core.Interfaces;
using EnvelopeGenerator.Core.Models;
using System.Data;

namespace EnvelopeGenerator.Core.Services;

/// <summary>
/// Repository implementation for accessing envelope data
/// </summary>
public class EnvelopeRepository : IEnvelopeRepository
{
    private readonly IDbConnection _connection;

    public EnvelopeRepository(IDbConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<VoucherData>> GetVoucherDataAsync(EnvelopeParams parameters)
    {
        var result = await _connection.QueryAsync<VoucherData>(
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
            commandType: CommandType.StoredProcedure);

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
        // Get structure fields
        var fields = await _connection.QueryAsync<FieldDefinition>(
            "GetEnvelopeStructure",
            new { EnvelopeType = envelopeType },
            commandType: CommandType.StoredProcedure);

        // Get header parameters (from second result set)
        using var multi = await _connection.QueryMultipleAsync(
            "GetEnvelopeStructure",
            new { EnvelopeType = envelopeType },
            commandType: CommandType.StoredProcedure);

        await multi.ReadAsync<FieldDefinition>(); // Skip first result set
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
        return await _connection.QueryFirstAsync<SystemParameters>(
            "GetSystemParams",
            commandType: CommandType.StoredProcedure);
    }
}