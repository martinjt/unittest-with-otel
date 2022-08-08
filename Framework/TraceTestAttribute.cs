using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace unittest_with_otel.Framework;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,
    AllowMultiple = false, Inherited = true)]
public class TraceTestAttribute : BaseTraceTestAttribute
{
    public bool TracePerTest { get; set; }
    private Activity? activityForThisTest;

    public TraceTestAttribute(bool tracePerTest = false)
    {
        TracePerTest = tracePerTest;
    }

    public override void Before(MethodInfo methodUnderTest)
    {
        if (TracePerTest || ActivityForTestRun == null)
        {
            var testRunActivityLink = ActivityForTestRun == null ?
                null :
                new List<ActivityLink> { new ActivityLink(ActivityForTestRun.Context) };
            activityForThisTest = OpenTelemetryMonitoredFixture.ActivitySource.StartActivity(
                methodUnderTest.Name,
                ActivityKind.Internal,
                new ActivityContext(), links: testRunActivityLink);
        }
        else
            activityForThisTest = OpenTelemetryMonitoredFixture.ActivitySource.StartActivity(methodUnderTest.Name, ActivityKind.Internal, ActivityForTestRun.Context);

        base.Before(methodUnderTest);
    }

    public override void After(MethodInfo methodUnderTest)
    {
        activityForThisTest?.Stop();
        base.After(methodUnderTest);
    }
}