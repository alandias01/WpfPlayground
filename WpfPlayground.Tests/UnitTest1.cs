using NUnit.Framework;

/* Testing
 * To create tests, RightClick solution->add new project->NUnit test project.  Syntax for Project name is $Project$.Tests
 * Manually, add packages NUnit, NUnit3TestAdapter, Microsoft.NET.Test.Sdk
 * 
 */

namespace WpfPlayground.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}