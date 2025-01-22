namespace EnvelopeGenerator.Core.Interfaces;

/// <summary>
/// Service for handling text encoding conversions
/// </summary>
public interface IEncodingService
{
    /// <summary>
    /// Convert text to DOS Hebrew encoding
    /// </summary>
    string ConvertToDosHebrew(string text);

    /// <summary>
    /// Convert text to Windows Hebrew encoding
    /// </summary>
    string ConvertToWindowsHebrew(string text);

    /// <summary>
    /// Reverse Hebrew text
    /// </summary>
    string ReverseHebrew(string text);
}