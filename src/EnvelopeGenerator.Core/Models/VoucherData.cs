namespace EnvelopeGenerator.Core.Models;

/// <summary>
/// Represents voucher data from the database
/// </summary>
public class VoucherData
{
    /// <summary>
    /// Family code
    /// </summary>
    public long Mspkod { get; set; }

    /// <summary>
    /// Debt batch number
    /// </summary>
    public long? ManaHovNum { get; set; }

    /// <summary>
    /// Envelope number
    /// </summary>
    public int MtfNum { get; set; }

    /// <summary>
    /// Is family level voucher
    /// </summary>
    public bool ShovarMsp { get; set; }

    /// <summary>
    /// Sorting field
    /// </summary>
    public string? Miun { get; set; }

    /// <summary>
    /// Unique number for combined processing
    /// </summary>
    public int? UniqNum { get; set; }

    /// <summary>
    /// Is yearly voucher
    /// </summary>
    public bool Shnati { get; set; }

    /// <summary>
    /// Dynamic fields storage
    /// </summary>
    private readonly Dictionary<string, object> _dynamicFields = new();

    /// <summary>
    /// Set dynamic field value
    /// </summary>
    public void SetField(string name, object value)
    {
        _dynamicFields[name] = value;
    }

    /// <summary>
    /// Get dynamic field value
    /// </summary>
    public object GetField(string name)
    {
        return _dynamicFields.TryGetValue(name, out var value) ? value : null;
    }

    /// <summary>
    /// Get all dynamic fields
    /// </summary>
    public IReadOnlyDictionary<string, object> GetAllFields()
    {
        return _dynamicFields;
    }
}