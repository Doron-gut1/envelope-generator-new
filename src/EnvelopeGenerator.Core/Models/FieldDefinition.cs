namespace EnvelopeGenerator.Core.Models;

/// <summary>
/// Field definition from mivnemtf table
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

    /// <summary>
    /// Whether this is an active field (not rek/simanenu)
    /// </summary>
    public bool IsActive { get; set; }
}

/// <summary>
/// Source table for field data
/// </summary>
public enum DataSource
{
    ShovarHead = 1,
    ShovarLines = 2,
    None = 4,
    ShovarHeadDynamic = 5,
    ShovarHeadNx = 6
}