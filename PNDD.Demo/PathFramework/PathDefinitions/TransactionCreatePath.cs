using Microsoft.Playwright;
using NUnit.Framework;
using PNDD.Demo.PathFramework;

namespace PNDD.Demo.PathFramework.PathDefinitions;

public class TransactionCreatePath : IPath
{
    public string Name => "Create Transaction";

    public List<string> Statements => new()
    {
        "Given I go to the transaction creation page",
        "When I submit a valid transaction",
        "Then I see a confirmation message"
    };

    public List<PathNode> Nodes => new()
    {
        new PathNode
        {
            Url = "/Transaction/Create",
            Action = "GET",
            ExecuteInteraction = async page =>
            {
                var description = page.Locator("#Description");
                var amount = page.Locator("#Amount");
                var submit = page.Locator("button[type='submit']");

                await description.FillAsync("Lunch");
                await amount.FillAsync("12.50");
                await submit.ClickAsync();
            }
        },
        new PathNode
        {
            Url = "/Transaction/Confirmation",
            Action = "GET",
            AssertState = async page =>
            {
                var body = await page.TextContentAsync("body");
                Assert.That(body, Does.Contain("Lunch"));
            }
        }
    };
}
