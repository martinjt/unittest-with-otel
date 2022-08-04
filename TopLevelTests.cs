using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using OpenTelemetry.Trace;
using Xunit;

namespace unittest_with_otel;

[Collection("OpenTelemetryFixture")]
public class TopLevelTestsOnly : IAsyncLifetime
{
    private TelemetrySpan TestSpan;

    public void SetTestName([CallerMemberName]string functionName = null!) => TestSpan.UpdateName(functionName);

    public TopLevelTestsOnly(OpenTelemetryMonitoredFixture fixture)
    {
        TestSpan = fixture.StartTestSpan();
    }

    [Fact]
    public void Test_DoStuffMethod_DoesStuff()
    {
        SetTestName();

        var sut = new TestClass();
        
        sut.DoSomeStuff();

        Assert.True(true);
    }

    [Fact]
    public void Test_DoStuffMethod_DoesDifferentStuff()
    {
        SetTestName();

        var sut = new TestClass();
        
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


public class TestClass
{
    public void DoSomeStuff()
    {
        Thread.Sleep(500);
    }
}
