using System.Threading;
using unittest_with_otel.Framework;
using Xunit;
using static unittest_with_otel.Framework.OpenTelemetryMonitoredFixture;

namespace unittest_with_otel;

[TraceTest(TracePerTest = true)]
public class TestsWithCausality : BaseTestWithAssemblyFixture
{
    [Fact]
    public void Causality_DoStuffMethod_DoesStuff()
    {
        var sut = new TestClassWithActivity();
        
        sut.DoSomeStuff();

        Assert.True(true);
    }

    [Fact]
    public void Causality_DoStuffMethod_DoesDifferentStuff()
    {
        var sut = new TestClassWithActivity();
        
        sut.DoSomeStuff();

        Assert.True(true);
    }
}


public class TestClassWithActivity
{
    public void DoSomeStuff()
    {
        using var span = ActivitySource.StartActivity("Do Some Stuff");
        Thread.Sleep(500);
        DoSomeInternalStuff();
    }

    private void DoSomeInternalStuff()
    {
        using var span = ActivitySource.StartActivity("Do Internal Stuff");
        Thread.Sleep(200);
    }
}
