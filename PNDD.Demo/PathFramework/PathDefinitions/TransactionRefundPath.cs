using NUnit.Framework;

namespace PNDD.Demo.PathFramework.PathDefinitions;

public class TransactionRefundPath : IPath
{
    public string Name => "Refund Transaction";
    public List<string> Statements => new()
    {
        "Given I go to the refund page",
        "When I enter a valid transaction ID",
        "Then I see the refund confirmation"
    };

    public List<PathNode> Nodes => new()
    {
        new PathNode
        {
            Url = "/Transaction/Refund",
            Action = "GET",
            ExecuteInteraction = async page =>
            {
                await page.FillAsync("input[name='transactionId']", "1");
                await page.ClickAsync("button[type='submit']");
            }
        },
        new PathNode
        {
            Url = "/Transaction/RefundConfirm",
            Action = "GET",
            AssertState = async page =>
            {
                var text = await page.InnerTextAsync("body");
                Assert.That(text, Does.Contain("refunded"));
            }
        }
    };
}
