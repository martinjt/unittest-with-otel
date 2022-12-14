using System.Threading;
using unittest_with_otel.Framework;
using Xunit;

namespace unittest_with_otel;

[TraceTest]
public class TopLevelTestsOnly : BaseTestWithAssemblyFixture
{
    [Fact]
    public void Test_DoStuffMethod_DoesStuff()
    {
        var sut = new TestClass();
        
        sut.DoSomeStuff();

        Assert.True(true);
    }

    [Fact]
    public void Test_DoStuffMethod_DoesDifferentStuff()
    {
        var sut = new TestClass();
        
        sut.DoSomeStuff();

        Assert.True(true);
    }

}

public class TestClass
{
    public void DoSomeStuff()
    {
        Thread.Sleep(500);
    }
}
