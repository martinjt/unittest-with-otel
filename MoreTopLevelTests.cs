using System.Threading;
using unittest_with_otel.Framework;
using Xunit;

namespace unittest_with_otel;

[TraceTest]
public class MoreTopLevelTestsOnly : BaseTestWithAssemblyFixture
{
    [Fact]
    public void Test_DoStuffMethod_DoesStuff()
    {
        var sut = new AnotherTestClass();
        
        sut.DoSomeStuff();

        Assert.True(true);
    }

    [Fact]
    public void Test_DoStuffMethod_DoesDifferentStuff()
    {
        var sut = new AnotherTestClass();
        
        sut.DoSomeStuff();

        Assert.True(true);
    }

}

public class AnotherTestClass
{
    public void DoSomeStuff()
    {
        Thread.Sleep(300);
    }
}
