using System.Text.Json.Serialization;

namespace EnvelopeGenerator.Core.Models;

/// <summary>
/// Field definition from mivnemtf table
/// </summary>
public record FieldDefinition
{
    /// <summary>
    /// Field name
    /// </summary>
    [JsonPropertyName("inname")]
    public string InName { get; init; } = string.Empty;

    /// <summary>
    /// Field data type
    /// </summary>
    [JsonPropertyName("fldtype")]
    public FieldType Type { get; init; }

    /// <summary>
    /// Field length in characters
    /// </summary>
    [JsonPropertyName("length")]
    public int Length { get; init; }

    /// <summary>
    /// Field order in the row
    /// </summary>
    [JsonPropertyName("realseder")]
    public int Order { get; init; }

    /// <summary>
    /// Source table for the field
    /// </summary>
    [JsonPropertyName("recordset")]
    public DataSource Source { get; init; }
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
/// Source table for field data
/// </summary>
public enum DataSource
{
    None = 0,
    ShovarHead = 1,
    ShovarLines = 2,
    Dynamic = 4,
    ShovarHeadDynamic = 5,
    ShovarHeadNx = 6
}