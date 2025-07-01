using Microsoft.Playwright;
using NUnit.Framework;
using System.Diagnostics;
using System.Net.Http;

namespace PNDD.Demo.Tests;

[SetUpFixture]
public class PlaywrightSetup
{
    public static IPlaywright? PlaywrightInstance;
    public static IBrowser? BrowserInstance;

    private static Process? _appProcess;

    [OneTimeSetUp]
    public static async Task GlobalSetup()
    {
        var appProjectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "PNDD.Demo"));

        if (!Directory.Exists(appProjectDir))
            throw new Exception("❌ App project directory does not exist.");

        var csprojPath = @"C:\Users\willi\source\ExperimentalSandbox\PNDD.Demo\PNDD.Demo.csproj";

        _appProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project \"{csprojPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        _appProcess.OutputDataReceived += (_, e) => { if (!string.IsNullOrWhiteSpace(e.Data)) Console.WriteLine($"[APP OUT] {e.Data}"); };
        _appProcess.ErrorDataReceived += (_, e) => { if (!string.IsNullOrWhiteSpace(e.Data)) Console.Error.WriteLine($"[APP ERR] {e.Data}"); };

        _appProcess.Start();
        _appProcess.BeginOutputReadLine();
        _appProcess.BeginErrorReadLine();

        try
        {
            var connStr = "Host=localhost;Port=5432;Database=pndd;Username=postgres;Password=yourpassword";
            await using var conn = new Npgsql.NpgsqlConnection(connStr);
            await conn.OpenAsync();
            Console.WriteLine("✅ DB is reachable");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ DB connection failed: {ex.Message}");
        }

        // Wait until the app is responding
        using var client = new HttpClient();
        for (int i = 0; i < 10; i++)
        {
            try
            {
                var response = await client.GetAsync("https://localhost:7237/Transaction/Create");
                if (response.IsSuccessStatusCode) break;
            }
            catch { }

            await Task.Delay(1000);
        }

        // Launch Playwright
        PlaywrightInstance = await Playwright.CreateAsync();
        BrowserInstance = await PlaywrightInstance.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            SlowMo = 250,
            Args = new[] { "--ignore-certificate-errors" }
        });
    }

    [OneTimeTearDown]
    public static async Task GlobalTeardown()
    {
        await BrowserInstance?.CloseAsync()!;
        PlaywrightInstance?.Dispose();

        if (_appProcess is { HasExited: false })
            _appProcess.Kill(true);

        _appProcess?.Dispose();
    }
}
