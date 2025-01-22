using EnvelopeGenerator.Core.Interfaces;
using EnvelopeGenerator.Core.Models;
using System.Text;

namespace EnvelopeGenerator.Core.Services;

/// <summary>
/// Service for generating envelope files
/// </summary>
public class FileGenerator : IFileGenerator
{
    private readonly IEncodingService _encodingService;

    public FileGenerator(IEncodingService encodingService)
    {
        _encodingService = encodingService;
    }

    /// <inheritdoc />
    public async Task ProcessVoucherBatch(IEnumerable<VoucherData> voucherBatch, EnvelopeParams parameters)
    {
        var files = new Dictionary<string, StreamWriter>();
        try
        {
            InitializeFiles(files, parameters);
            foreach (var voucher in voucherBatch)
            {
                var line = FormatLine(voucher, parameters);

                // Write to appropriate file based on action type and voucher type
                if (parameters.ActionType == 3 && voucher.UniqNum == -100)
                {
                    await files["SvrMeshulavShotefHov"].WriteLineAsync(line);
                }
                else if (voucher.ManaHovNum == 0)
                {
                    await files["SvrShotef"].WriteLineAsync(line);
                }
                else
                {
                    await files["SvrHov"].WriteLineAsync(line);
                }
            }
        }
        finally
        {
            foreach (var file in files.Values)
            {
                await file.DisposeAsync();
            }
        }
    }

    private void InitializeFiles(Dictionary<string, StreamWriter> files, EnvelopeParams parameters)
    {
        var mntName = parameters.BatchNumber.ToString().Replace("/", "");
        var baseDir = parameters.OutputDirectory;

        if (parameters.ActionType == 1 || parameters.ActionType == 2)
        {
            var fileName = parameters.ActionType == 1 ? "SvrShotef" : "SvrHov";
            files[fileName] = new StreamWriter(
                Path.Combine(baseDir, $"{fileName}_{mntName}.txt"),
                false,
                Encoding.GetEncoding(1255));
        }
        else
        {
            files["SvrShotef"] = new StreamWriter(
                Path.Combine(baseDir, $"SvrShotef_{mntName}.txt"),
                false,
                Encoding.GetEncoding(1255));

            files["SvrHov"] = new StreamWriter(
                Path.Combine(baseDir, $"SvrHov_{mntName}.txt"),
                false,
                Encoding.GetEncoding(1255));

            files["SvrMeshulavShotefHov"] = new StreamWriter(
                Path.Combine(baseDir, $"SvrMeshulavShotefHov_{mntName}.txt"),
                false,
                Encoding.GetEncoding(1255));
        }
    }

    private string FormatLine(VoucherData voucher, EnvelopeParams parameters)
    {
        var sb = new StringBuilder();
        var allFields = voucher.GetAllFields();  // שימוש במתודה במקום בגישה ישירה לשדה

        foreach (var field in allFields.OrderBy(f => f.Key))
        {
            string value = field.Value?.ToString() ?? string.Empty;

            // Handle encoding if needed
            if (field.Key.StartsWith("txt", StringComparison.OrdinalIgnoreCase) && parameters.IsYearly)
            {
                value = _encodingService.ConvertToDosHebrew(value);
            }

            // TODO: Add more field-specific formatting based on field type and requirements
            sb.Append(value);
        }

        return sb.ToString();
    }
}