using EmailTemplates;
using System.Diagnostics;

namespace EmailSenderService;

public class Worker(EmailRenderer emailRenderer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Press ENTER to start.");
        Console.ReadLine();

        Console.WriteLine("Rendering emails...");

        for (int i = 0; i < 15000; i++)
        {
            var parameters = new Dictionary<string, object?>
            {
                { nameof(TestEmailTemplate.Title), "Test" },
                { nameof(TestEmailTemplate.PreviewText), $"Reference number: {Guid.NewGuid()}" },
            };

            var body = await emailRenderer.RenderTemplateAsync<TestEmailTemplate>(parameters, stoppingToken);
            TextWriter.Null.Write(body);
        }

        Console.WriteLine("Rendering emails finished.");

        for (; ; )
        {
            GC.Collect(2, GCCollectionMode.Forced, blocking: true, compacting: true);

            PrintMemoryStats();

            await Task.Delay(5000, stoppingToken);
        }
    }

    private static void PrintMemoryStats()
    {
        var currentProcess = Process.GetCurrentProcess();

        currentProcess.Refresh();

        var workingSetMB = currentProcess.WorkingSet64 / 1024.0 / 1024.0;
        var privateMemoryMB = currentProcess.PrivateMemorySize64 / 1024.0 / 1024.0;
        var managedMemoryMB = GC.GetTotalMemory(false) / 1024.0 / 1024.0;

        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}]");
        Console.WriteLine($"- Working Set (WorkingSet64): {workingSetMB:F2} MB");
        Console.WriteLine($"- Private Bytes (PrivateMemorySize64): {privateMemoryMB:F2} MB");
        Console.WriteLine($"- Managed Heap (GC.GetTotalMemory): {managedMemoryMB:F2} MB");
    }
}
