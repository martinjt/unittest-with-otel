using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using OpenTelemetry.Trace;
using Xunit;
using static unittest_with_otel.OpenTelemetryMonitoredFixture;

namespace unittest_with_otel;

[Collection("OpenTelemetryFixture")]
public class TestsWithCausality : IAsyncLifetime
{
    private TelemetrySpan TestSpan;

    public void SetTestName([CallerMemberName]string functionName = null!) => TestSpan.UpdateName(functionName);

    public TestsWithCausality(OpenTelemetryMonitoredFixture fixture)
    {
        TestSpan = fixture.TestTracer.StartActiveSpan("Test started");
    }

    [Fact]
    public void Causality_DoStuffMethod_DoesStuff()
    {
        SetTestName();

        var sut = new TestClassWithActivity();
        
        sut.DoSomeStuff();

        Assert.True(true);
    }

    [Fact]
    public void Causality_DoStuffMethod_DoesDifferentStuff()
    {
        SetTestName();

        var sut = new TestClassWithActivity();
        
        sut.DoSomeStuff();

        Assert.True(true);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        TestSpan.End();
        return Task.CompletedTask;
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
