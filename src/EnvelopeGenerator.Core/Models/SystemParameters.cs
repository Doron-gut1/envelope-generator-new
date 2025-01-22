namespace EnvelopeGenerator.Core.Models;

/// <summary>
/// System-wide parameters
/// </summary>
public class SystemParameters
{
    /// <summary>
    /// Whether to reverse Hebrew text
    /// </summary>
    public bool RevHeb { get; set; }

    /// <summary>
    /// Whether to encode Hebrew text as DOS Hebrew
    /// </summary>
    public bool DosHebrew { get; set; }

    /// <summary>
    /// Print email MTF parameter
    /// </summary>
    public bool PrintEmailMtf { get; set; }
}