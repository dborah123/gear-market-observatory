using Amazon.CDK;
using Constructs;

namespace GearMarketObservatory.Cdk;

public static class Program
{
    public static void Main(string[] args)
    {
        var app = new App();
        _ = new BaselineStack(app, "GearMarketObservatoryBaseline");
        app.Synth();
    }
}

/// <summary>
/// Empty CDK application baseline. AWS resources are intentionally introduced in PR2.
/// </summary>
public sealed class BaselineStack : Stack
{
    public BaselineStack(Construct scope, string id)
        : base(scope, id)
    {
    }
}
