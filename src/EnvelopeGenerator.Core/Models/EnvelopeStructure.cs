namespace EnvelopeGenerator.Core.Models;

/// <summary>
/// Defines the structure of an envelope
/// </summary>
public class EnvelopeStructure
{
    /// <summary>
    /// List of fields in the envelope
    /// </summary>
    public List<FieldDefinition> Fields { get; set; } = new();

    /// <summary>
    /// Number of digits after decimal point for currency fields
    /// </summary>
    public int NumOfDigits { get; set; }

    /// <summary>
    /// Position of yearly rows (1: below periodic, 2: after periodic)
    /// </summary>
    public int PositionOfShnati { get; set; }

    /// <summary>
    /// Whether to use DOS Hebrew encoding
    /// </summary>
    public bool DosHebrewEncoding { get; set; }

    /// <summary>
    /// Number of detail lines per envelope
    /// </summary>
    public int NumOfPerutLines { get; set; }

    /// <summary>
    /// Number of fields per detail line
    /// </summary>
    public int NumOfPerutFields { get; set; }
}