using System;
using System.Diagnostics;
using Honeycomb.OpenTelemetry;
using Microsoft.Extensions.Configuration;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Xunit;

namespace unittest_with_otel;

[CollectionDefinition("OpenTelemetryFixture")]
public class OpenTelemetryCollection : ICollectionFixture<OpenTelemetryMonitoredFixture>
{

}

public class OpenTelemetryMonitoredFixture : IDisposable
{
    private const string TracerName = "unittest-with-otel";
    private readonly TracerProvider _tracerProvider;
    internal readonly Tracer TestTracer;
    public static string TestRunId { get; } = Guid.NewGuid().ToString();

    private TelemetrySpan _testRunSpan;

    public static ActivitySource ActivitySource = new ActivitySource(TracerName);

    public OpenTelemetryMonitoredFixture()
    {
        var honeycombOptions = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .AddEnvironmentVariables()
            .Build()
            .GetSection("Honeycomb").Get<HoneycombOptions>();

        _tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddHoneycomb(honeycombOptions)
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(TracerName))
            .AddProcessor(new TestRunSpanProcessor(TestRunId))
            .AddSource(TracerName)
            .Build();

        TestTracer = _tracerProvider.GetTracer(TracerName);
        _testRunSpan = TestTracer.StartActiveSpan("TestRun");
    }

    internal TelemetrySpan StartTestSpan()
    {
        return TestTracer.StartSpan("Test started", SpanKind.Internal, parentSpan: _testRunSpan);
    }

    public void Dispose()
    {
        _testRunSpan.End();
        _tracerProvider.Dispose();
    }
}

/// <summary>
/// Adds an attribute to every span with the current TestRunId
/// </summary>
public class TestRunSpanProcessor : BaseProcessor<Activity>
{
    private readonly string _testRunId;

    public TestRunSpanProcessor(string testRunId)
    {
        _testRunId = testRunId;
    }

    public override void OnStart(Activity data)
    {
        data?.SetTag("test.run_id", _testRunId);
    }
}