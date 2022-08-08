using System.Diagnostics;
using OpenTelemetry;

namespace unittest_with_otel.Framework;

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
