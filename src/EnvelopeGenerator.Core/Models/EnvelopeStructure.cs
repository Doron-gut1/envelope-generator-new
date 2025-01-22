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

/// <summary>
/// Defines a single field in the envelope structure
/// </summary>
public class FieldDefinition
{
    /// <summary>
    /// Field name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Field data type
    /// </summary>
    public FieldType Type { get; set; }

    /// <summary>
    /// Field length in characters
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// Field order in the row
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Source table for the field
    /// </summary>
    public DataSource Source { get; set; }
}

/// <summary>
/// Field data types
/// </summary>
public enum FieldType
{
    Text = 1,
    Numeric = 2,
    Currency = 3
}

/// <summary>
/// Data source tables
/// </summary>
public enum DataSource
{
    ShovarHead = 1,
    ShovarLines = 2,
    Dynamic = 4,
    ShovarHeadNx = 5
}