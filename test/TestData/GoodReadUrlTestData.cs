using GoodReadScripts.Domain;

namespace GoodReadScripts.Test.TestData;

public static class GoodReadUrlTestData
{
    public static GoodReadUrl Create(string url)
    {
        var result = GoodReadUrl.Create(url);
        return result.Match(
            goodReadUrl => goodReadUrl,
            errors => throw new ArgumentException($"Invalid GoodReadUrl: {string.Join(", ", errors)}")
        );
    }
}