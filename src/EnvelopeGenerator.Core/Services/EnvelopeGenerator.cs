using EnvelopeGenerator.Core.Interfaces;
using EnvelopeGenerator.Core.Models;

namespace EnvelopeGenerator.Core.Services;

/// <summary>
/// Main service for envelope generation
/// </summary>
public class EnvelopeGenerator : IEnvelopeGenerator
{
    private const int BATCH_SIZE = 1000;
    
    private readonly IEnvelopeRepository _repository;
    private readonly IFileGenerator _fileGenerator;

    public EnvelopeGenerator(
        IEnvelopeRepository repository,
        IFileGenerator fileGenerator)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _fileGenerator = fileGenerator ?? throw new ArgumentNullException(nameof(fileGenerator));
    }

    /// <inheritdoc />
    public async Task<bool> GenerateEnvelopes(string odbcName, EnvelopeParams parameters)
    {
        try
        {
            
            // Get envelope structure
            var structure = await _repository.GetEnvelopeStructureAsync(parameters.EnvelopeType);
            
            // Get system parameters
            var systemParams = await _repository.GetSystemParametersAsync();

            // Get voucher data in batches
            var voucherData = await _repository.GetVoucherDataAsync(parameters);
            foreach (var batch in voucherData.Chunk(BATCH_SIZE))
            {
                await _fileGenerator.ProcessVoucherBatch(batch, parameters);
            }

            return true;
        }
        catch (Exception ex)
        {
            // TODO: Add proper logging
            Console.WriteLine($"Error generating envelopes: {ex.Message}");
            return false;
        }
    }
}