using PNDD.Demo.PathFramework.PathDefinitions;

namespace PNDD.Demo.Tests;

public class TransactionCreatePathTest : BasePathTest
{
    [Test]
    public async Task CreateTransaction_EndToEnd()
    {
        var baseUrl = "https://localhost:7237";
        var path = new TransactionCreatePath();

        foreach (var node in path.Nodes)
        {
            await Page!.GotoAsync($"{baseUrl}{node.Url}");

            if (node.ExecuteInteraction is not null)
                await node.ExecuteInteraction(Page);

            if (node.AssertState is not null)
                await node.AssertState(Page);
        }
    }
}
