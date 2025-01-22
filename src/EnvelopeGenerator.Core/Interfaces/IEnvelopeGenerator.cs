using EnvelopeGenerator.Core.Models;

namespace EnvelopeGenerator.Core.Interfaces;

/// <summary>
/// Main interface for envelope generation
/// </summary>
public interface IEnvelopeGenerator
{
    /// <summary>
    /// Generates envelope files based on provided parameters
    /// </summary>
    /// <param name="odbcName">ODBC connection name</param>
    /// <param name="parameters">Envelope generation parameters</param>
    /// <returns>True if generation was successful</returns>
    Task<bool> GenerateEnvelopes(string odbcName, EnvelopeParams parameters);
}