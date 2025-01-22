using EnvelopeGenerator.Core.Models;

namespace EnvelopeGenerator.Core.Interfaces;

/// <summary>
/// Repository for accessing envelope data
/// </summary>
public interface IEnvelopeRepository
{
    /// <summary>
    /// Get voucher data based on provided parameters
    /// </summary>
    Task<IEnumerable<VoucherData>> GetVoucherDataAsync(EnvelopeParams parameters);

    /// <summary>
    /// Get envelope structure definition
    /// </summary>
    Task<EnvelopeStructure> GetEnvelopeStructureAsync(int envelopeType);

    /// <summary>
    /// Get system parameters
    /// </summary>
    Task<SystemParameters> GetSystemParametersAsync();
}