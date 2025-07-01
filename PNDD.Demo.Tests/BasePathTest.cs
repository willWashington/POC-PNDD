// Tests/BasePathTest.cs
using Microsoft.Playwright;
using NUnit.Framework;

namespace PNDD.Demo.Tests;

public abstract class BasePathTest
{
    protected IBrowser Browser => PlaywrightSetup.BrowserInstance!;
    protected IPlaywright Playwright => PlaywrightSetup.PlaywrightInstance!;
    protected IBrowserContext? Context;
    protected IPage? Page;

    [SetUp]
    public async Task Setup()
    {
        Context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = null
        });
        Page = await Context.NewPageAsync();
    }

    [TearDown]
    public async Task Teardown()
    {
        await Context?.CloseAsync()!;
    }
}
