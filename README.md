# Path Node Driven Design (PNDD)

This project demonstrates **Path Node Driven Design (PNDD)** — a strategy that maps full application features to structured paths composed of navigable nodes.


---

## Install Requirements

- Docker
- Postgres SQL
- C# / .NET / EF
- // What am I missing?

## Execution:
- Run `dotnet test` from the root (iirc - test explorer is failing atm - forcing me to try to run the server separately)

---

## Concept

A `PathNode` represents a step in a user-visible flow.

---

## Paths

Multiple `PathNode`s are grouped into a class that defines a feature flow.

---

## End-to-End Test Execution

Test classes execute all the `PathNode`s in a path sequentially, driving the browser using Playwright and verifying behavior with assertions.

```cs
public class PathNode
{
    public string Url { get; set; }
    public string? Action { get; set; }
    public string? DependsOn { get; set; }
    public Func<IPage, Task>? ExecuteInteraction { get; set; }
    public Func<IPage, Task>? AssertState { get; set; }
}

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
```

### Test:

```cs
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
```

---

## Benefits

- Declarative, self-describing paths  
- Reusable test logic  
- Full coverage mapped to real user workflows  
- Easily composed feature flows

---

## Structure
```
PNDD.Demo/
├── Controllers/
├── Models/
├── Views/
├── PathFramework/
│ ├── PathNode.cs
│ ├── IPath.cs
│ └── PathDefinitions/
│ └── TransactionCreatePath.cs
└── PNDD.Demo.Tests/
├── BasePathTest.cs
└── TransactionCreatePathTest.cs
```

---

## Status

- [x] Playwright + NUnit integration  
- [x] PathNode design pattern  
- [x] Single feature coverage  
- [ ] Multiple path chaining  
- [ ] Dynamic path discovery  
- [ ] Fixture-based runtime graph

