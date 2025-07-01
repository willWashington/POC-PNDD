using NUnit.Framework;

public interface IPath
{
    string Name { get; }
    List<string> Statements { get; }
    List<PathNode> Nodes { get; }
}

public class CreateTransactionPath : IPath
{
    public string Name => "Create Transaction End-to-End";
    public List<string> Statements => new()
    {
        "Given I am on the create transaction screen",
        "When I submit a valid transaction",
        "Then I see the confirmation page",
        "And I see my transaction in history"
    };

    public List<PathNode> Nodes => new()
    {
        new PathNode
        {
            Url = "/Transaction/Create",
            Action = "GET",
            ExecuteInteraction = async page =>
            {
                await page.FillAsync("input[name='Description']", "Lunch");
                await page.FillAsync("input[name='Amount']", "15.00");
                await page.ClickAsync("button[type='submit']");
            }
        },
        new PathNode
        {
            Url = "/Transaction/Confirmation",
            Action = "GET",
            AssertState = async page =>
            {
                var content = await page.ContentAsync();
                Assert.That(content, Does.Contain("Lunch"));
            }
        },
        new PathNode
        {
            Url = "/Transaction/History",
            Action = "GET",
            AssertState = async page =>
            {
                var history = await page.Locator("table").InnerTextAsync();
                Assert.That(history, Does.Contain("Lunch"));
            }
        }
    };
}
