using EnvelopeGenerator.Core.Interfaces;
using EnvelopeGenerator.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace EnvelopeGenerator.Core.Services;

/// <summary>
/// Manager class for envelope generation process
/// </summary>
public class EnvelopeManager
{
    private readonly string _odbcName;
    private readonly IEnvelopeGenerator _generator;
    private readonly EnvelopeParams _params;

    /// <summary>
    /// Initializes a new instance of EnvelopeManager
    /// </summary>
    public EnvelopeManager(
        string odbcName, 
        int actionType, 
        int envelopeType, 
        int batchNumber,
        bool isYearly = false,
        long? familyCode = null,
        int? closureNumber = null,
        long? voucherGroup = null)
    {
        _odbcName = odbcName;
        
        // Configure DI
        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();
        
        // Get generator
        _generator = serviceProvider.GetRequiredService<IEnvelopeGenerator>();
        
        // Set parameters
        _params = new EnvelopeParams
        {
            ActionType = actionType,
            EnvelopeType = envelopeType,
            BatchNumber = batchNumber,
            IsYearly = isYearly,
            FamilyCode = familyCode,
            ClosureNumber = closureNumber,
            VoucherGroup = voucherGroup
        };
    }

    /// <summary>
    /// Generates envelopes based on the provided parameters
    /// </summary>
    /// <returns>Tuple containing success flag and error description if any</returns>
    public async Task<(bool Success, string ErrorDescription)> GenerateEnvelopes()
    {
        try
        {
            bool success = await _generator.GenerateEnvelopes(_odbcName, _params);
            return (success, success ? "" : "Failed to generate envelopes");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Register connection with explicit IDbConnection creation
        services.AddScoped<IDbConnection>(sp =>
        {
            var factory = sp.GetRequiredService<IConnectionFactory>();
            return factory.CreateConnection(_odbcName);
        });

        // Register all other services
        services.AddTransient<IEnvelopeGenerator, EnvelopeGenerator>();
        services.AddTransient<IFileGenerator, FileGenerator>();
        services.AddTransient<IEncodingService, HebrewEncodingService>();
        services.AddTransient<IConnectionFactory, OdbcConnectionFactory>();
        services.AddTransient<IEnvelopeRepository, EnvelopeRepository>();
    }
}