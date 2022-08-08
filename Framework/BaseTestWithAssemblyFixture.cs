using Xunit;
using Xunit.Extensions.AssemblyFixture;

[assembly: TestFramework(AssemblyFixtureFramework.TypeName, AssemblyFixtureFramework.AssemblyName)]

namespace unittest_with_otel.Framework;

public class BaseTestWithAssemblyFixture : IAssemblyFixture<OpenTelemetryMonitoredFixture>
{
    
}