// Tests/LaunchSettings.cs
using Microsoft.Playwright;
using NUnit.Framework;

namespace PNDD.Demo.Tests;

[SetUpFixture]
public class PlaywrightSetup
{
    public static IPlaywright? PlaywrightInstance;
    public static IBrowser? BrowserInstance;

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        PlaywrightInstance = await Playwright.CreateAsync();
        BrowserInstance = await PlaywrightInstance.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false, // ✅ run headed
            SlowMo = 250      // ✅ slow for visibility
        });
    }

    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        await BrowserInstance?.CloseAsync()!;
        PlaywrightInstance?.Dispose();
    }
}
