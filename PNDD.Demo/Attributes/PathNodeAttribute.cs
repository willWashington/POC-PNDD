// Attributes/PathNodeAttribute.cs
using System;

namespace PNDD.Demo.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class PathNodeAttribute : Attribute
{
    public string Path { get; }
    public string? View { get; set; }
    public string? Script { get; set; }
    public string? Fixture { get; set; }

    public PathNodeAttribute(string path)
    {
        Path = path;
    }
}
