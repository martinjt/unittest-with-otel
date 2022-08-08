
using System;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public class OpenTelemetryFixture : IDisposable
{
    private const string TracerName = "unit-test-with-otel";
    private readonly TracerProvider _tracerProvider;

    public OpenTelemetryFixture()
    {
        _tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(TracerName))
            .AddSource(TracerName)
            .Build();
    }

    public void Dispose()
    {
        _tracerProvider?.Dispose();
    }
}