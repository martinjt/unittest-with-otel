using System;
using System.Diagnostics;
using System.Reflection;

namespace unittest_with_otel.Framework;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,
    AllowMultiple = false, Inherited = true)]
public class TracePerTestRunAttribute : BaseTraceTestAttribute
{
    private Activity? activityForThisTest;

    public override void Before(MethodInfo methodUnderTest)
    {
        if (ActivityForTestRun == null)
            throw new ArgumentNullException(nameof(ActivityForTestRun),
            "The test run Activity was null, and therefore can't be used");
        
        activityForThisTest = OpenTelemetryMonitoredFixture
            .ActivitySource
            .StartActivity(methodUnderTest.Name, 
                ActivityKind.Internal,
                ActivityForTestRun.Context);

        base.Before(methodUnderTest);
    }

    public override void After(MethodInfo methodUnderTest)
    {
        activityForThisTest?.Stop();
        base.After(methodUnderTest);
    }
}