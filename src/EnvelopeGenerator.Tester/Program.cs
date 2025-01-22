using EnvelopeGenerator.Core.Services;

namespace EnvelopeGenerator.Tester;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Get input parameters
            Console.Write("Enter ODBC Name: ");
            string odbcName = Console.ReadLine() ?? "";

            Console.Write("Enter Action Type (1-Current, 2-Debt, 3-Combined): ");
            int actionType = int.Parse(Console.ReadLine() ?? "1");

            Console.Write("Enter Envelope Type: ");
            int envelopeType = int.Parse(Console.ReadLine() ?? "1");

            Console.Write("Enter Batch Number: ");
            int batchNumber = int.Parse(Console.ReadLine() ?? "1");

            Console.Write("Is Yearly? (Y/N): ");
            bool isYearly = (Console.ReadLine()?.ToUpper() ?? "N") == "Y";

            Console.Write("Enter Family Code (optional, press Enter to skip): ");
            string? familyCodeStr = Console.ReadLine();
            long? familyCode = string.IsNullOrWhiteSpace(familyCodeStr) ? null : long.Parse(familyCodeStr);

            Console.Write("Enter Closure Number (optional, press Enter to skip): ");
            string? closureNumberStr = Console.ReadLine();
            int? closureNumber = string.IsNullOrWhiteSpace(closureNumberStr) ? null : int.Parse(closureNumberStr);

            Console.Write("Enter Voucher Group (optional, press Enter to skip): ");
            string? voucherGroupStr = Console.ReadLine();
            long? voucherGroup = string.IsNullOrWhiteSpace(voucherGroupStr) ? null : long.Parse(voucherGroupStr);

            // Create manager and generate envelopes
            var manager = new EnvelopeManager(
                odbcName, 
                actionType, 
                envelopeType, 
                batchNumber,
                isYearly,
                familyCode,
                closureNumber,
                voucherGroup);

            Console.WriteLine("\nGenerating envelopes...");
            var result = await manager.GenerateEnvelopes();

            // Show result
            if (result.Success)
            {
                Console.WriteLine("Envelopes generated successfully!");
            }
            else
            {
                Console.WriteLine($"Failed to generate envelopes: {result.ErrorDescription}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}