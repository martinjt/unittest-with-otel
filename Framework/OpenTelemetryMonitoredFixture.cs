using System;
using System.Diagnostics;
using Honeycomb.OpenTelemetry;
using Microsoft.Extensions.Configuration;
using OpenTelemetry;
using OpenTelemetry.Trace;
using Xunit.Sdk;

namespace unittest_with_otel.Framework;

public class OpenTelemetryMonitoredFixture : IDisposable
{
    public static ActivitySource ActivitySource = new ActivitySource(TracerName);
    private const string TracerName = "unittest-with-otel";
    private readonly TracerProvider? _tracerProvider;

    public OpenTelemetryMonitoredFixture()
    {
        var honeycombOptions = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .AddEnvironmentVariables()
            .Build()
            .GetSection("Honeycomb").Get<HoneycombOptions>();

        _tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddHoneycomb(honeycombOptions)
            .AddProcessor(new TestRunSpanProcessor(Guid.NewGuid().ToString()))
            .AddSource(TracerName)
            .Build();
    }

    public void Dispose()
    {
        BaseTraceTestAttribute.ActivityForTestRun?.Stop();
        _tracerProvider?.Dispose();
    }
}

public abstract class BaseTraceTestAttribute : BeforeAfterTestAttribute
{
    internal static Activity? ActivityForTestRun = OpenTelemetryMonitoredFixture.ActivitySource.StartActivity("TestRun");
}