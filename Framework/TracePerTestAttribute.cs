using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Xunit.Sdk;

namespace unittest_with_otel.Framework;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,
    AllowMultiple = false, Inherited = true)]
public class TracePerTestAttribute : BaseTraceTestAttribute
{
    private Activity? activityForThisTest;
    public override void Before(MethodInfo methodUnderTest)
    {
        var linkToTestRunActivity = ActivityForTestRun == null ?
            null :
            new List<ActivityLink> { new ActivityLink(ActivityForTestRun.Context) };

        activityForThisTest = OpenTelemetryMonitoredFixture.ActivitySource.StartActivity(
            methodUnderTest.Name,
            ActivityKind.Internal,
            new ActivityContext(), links: linkToTestRunActivity);

        base.Before(methodUnderTest);
    }

    public override void After(MethodInfo methodUnderTest)
    {
        activityForThisTest?.Stop();
        base.After(methodUnderTest);
    }
}