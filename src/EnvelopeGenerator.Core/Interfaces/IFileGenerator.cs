using EnvelopeGenerator.Core.Models;

namespace EnvelopeGenerator.Core.Interfaces;

/// <summary>
/// Service for generating envelope files
/// </summary>
public interface IFileGenerator
{
    /// <summary>
    /// Process a batch of vouchers and write to appropriate files
    /// </summary>
    Task ProcessVoucherBatch(IEnumerable<VoucherData> voucherBatch, EnvelopeParams parameters);
}