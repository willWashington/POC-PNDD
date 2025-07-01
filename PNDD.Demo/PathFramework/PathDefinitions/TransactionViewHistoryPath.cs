using NUnit.Framework;

namespace PNDD.Demo.PathFramework.PathDefinitions;

public class TransactionViewHistoryPath : IPath
{
    public string Name => "View Transaction History";
    public List<string> Statements => new()
    {
        "Given I view the transaction history",
        "Then I see recent transactions"
    };

    public List<PathNode> Nodes => new()
    {
        new PathNode
        {
            Url = "/Transaction/History",
            Action = "GET",
            AssertState = async page =>
            {
                var text = await page.InnerTextAsync("body");
                Assert.That(text, Does.Contain("Lunch").Or.Contain("Coffee"));
            }
        }
    };
}
