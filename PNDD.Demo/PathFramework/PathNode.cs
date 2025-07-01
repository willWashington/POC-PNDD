using Microsoft.Playwright;

public class PathNode
{
    public string Url { get; set; }
    public string? Action { get; set; } // e.g. GET, POST
    public string? DependsOn { get; set; } // e.g. ID, Cookie
    public Func<IPage, Task>? ExecuteInteraction { get; set; }
    public Func<IPage, Task>? AssertState { get; set; }
}
