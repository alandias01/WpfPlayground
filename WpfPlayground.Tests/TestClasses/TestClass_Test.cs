using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfPlayground.TestClasses;

/* Testing
 * To create tests, RightClick solution->add new project->NUnit test project.  Syntax for Project name is $Project$.Tests
 * Manually, add packages NUnit, NUnit3TestAdapter, Microsoft.NET.Test.Sdk
 * 
 * Test->Test Explorer to open window
 * 
 * Fast: If tests are slow, developers are reluctant to run them
 * Repeatable: Same results every time;
 * Isolation:  test only the code that we plan to test. Should not test underlying classes, which should be checked independently
 * Self Containment: Tests thatrely on external information are prone to fail and require configurations and setups. 
 *                   This leads to test discarding. To make sure that this doesn’t happen, all the information 
 *                   required for the tests should be in the tests. For example, instead of relying on an environment 
 *                   variable to be set, we must set it ourselves in the test.
 * Race safe:If test depends on values from an external source like a db that changes and multiple devs are running tests, bad
 * Well documented
 * Maintainable: Maintained and refactored whn needed.  Tests can become stale and not reflect current state of software, even if it passes
 * TODO
 * Exceptions
 * 
 */

namespace WpfPlayground.Tests.TestClasses
{
    [TestFixture]   //Not needed in NUnit3
    //[Ignore("")]  //Ignore at class level
    //[Category("AlanCategory")]
    public class TestClass_Test
    {
        private TestClass sut;

        [OneTimeSetUp]  //Runs once before the tests are run
        public void OneTimeSetup()
        {
            //If you create a collection here that a test will modify, then other 
            //tests will be using the modified collection.  This breaks Test isolation            
        }

        [SetUp] //Runs before each test is run
        public void Setup()
        {
            this.sut = new TestClass(1);
        }

        [TearDown] //Runs after each test is run
        public void TearDown()
        {
            //this.sut.Dispose
        }

        [OneTimeTearDown] //Runs once after all tests are run
        public void OneTimeTearDown()
        {
            
        }

        [Test]
        public void Basics()
        {
            //Arrange
            
            //Act
            sut.DoubleValue = 1.0 / 3.0;
            sut.TestingMethodsList = Enumerable.Range(0, 5).Select(x => new TestClass(x)).ToList();
            TestClass sut2 = sut;

            //Assert
            Assert.That(sut, Has.Property("Years"));
            Assert.That(sut.Years, Is.EqualTo(1));  //If constructor set this value
            Assert.That(sut.Years, Is.Not.EqualTo(2));  //If constructor set this value
            Assert.That(sut.Years, Is.GreaterThan(0));  //If constructor set this value
            Assert.That(sut.Years, Is.InRange(0, 2));
            Assert.That(sut.Years, Is.EqualTo(3).Within(2));  //If using Datetime you can do  " .Within(2).Days"
            Assert.That(sut, Is.SameAs(sut2));  //Reference comparison
            Assert.That(sut.TestingMethodsList, Is.Not.Null);
            Assert.That("", Is.Empty);
            Assert.That("abc", Is.EqualTo("ABC").IgnoreCase);
            Assert.That("abc", Does.StartWith("a").And.EndsWith("c").And.Contains("c"));
            Assert.That(true);
            Assert.That(true, Is.True);
                        
            Assert.DoesNotThrow(() => new TestClass());
            //Assert.That(sut.DivTwoNumbers(3, 0), Throws.TypeOf<DivideByZeroException>());
            
            Assert.That(sut.DivTwoNumbers(8, 4), Is.EqualTo(2));
            Assert.That(sut.DoubleValue, Is.EqualTo(.33).Within(.004)); //Tests float value
            Assert.That(sut.DoubleValue, Is.EqualTo(.33).Within(10).Percent);   //Tests float value

            //COLLECTIONS
            Assert.That(sut.TestingMethodsList, Has.Exactly(5).Items);
            Assert.That(sut.TestingMethodsList, Has.Count.GreaterThan(4));
            Assert.That(sut.TestingMethodsList, Is.Unique);
            Assert.That(sut.TestingMethodsList, Does.Contain(new TestClass(1)));   //Implemented IEquatable
            
            Assert.That(sut.TestingMethodsList, Has.Exactly(5).Property("Name").EqualTo("Alan"));            
            Assert.That(sut.TestingMethodsList, Has.Exactly(5).Matches<TestClass>(item => item.Name == "Alan" && item.Years >= 0));
            
            Assert.That(sut.TestingMethodsList, Is.All.Not.Null);
            Assert.That(sut.TestingMethodsList, Is.All.InstanceOf<TestClass>());
            Assert.That(sut.TestingMethodsList, Has.One.EqualTo(new TestClass(4)));
            Assert.That(sut.TestingMethodsList, Has.Some.EqualTo(new TestClass(4)));
            Assert.That(sut.TestingMethodsList, Has.No.One.EqualTo(new TestClass(6)));
            
            //Assert.That(, );
        }

        [Test]
        public void BasicsFluent()
        {
            sut.DoubleValue = 1.0 / 3.0;
            sut.TestingMethodsList = Enumerable.Range(0, 5).Select(x => new TestClass(x)).ToList();
            sut.Years = 2;


            //have property
            sut.Years.Should().Be(2);
            sut.Name.Should().NotBeNull();
            sut.Name.Should().BeOfType(typeof(string));
            sut.Name.Should().Contain("Alan");  //.StartWith(), .Match("*n"),  .MatchRegex("[A-Z]{4}")            
            sut.DoubleValue.Should().BeApproximately(.33, .004);

            Action act = () => sut.MethodThrowException();
            act.Should().Throw<Exception>();

            sut.TestingMethodsList.Should().HaveCount(5);
            sut.TestingMethodsList.Should().HaveCountGreaterThan(3);
            sut.TestingMethodsList.Should().NotBeNullOrEmpty();

            //AssertionScope is if a test fails, it will keep going and at end you can get a list of failed tests
            using (new AssertionScope())
            {
                sut.TestingMethodsList.Should().OnlyHaveUniqueItems();
                sut.TestingMethodsList.Should().Contain(new TestClass(1));
            }
            sut.TestingMethodsList.Should().ContainItemsAssignableTo<ITestClass>();

            //You can assert on methods and properties
            typeof(TestClass).Methods().ThatArePublicOrInternal.ThatReturn<int>().Should().NotBeVirtual();


            //Events

            //Special built in feature for MVVM
            //subject.Should().Raise().PropertyChangeFor(x => x.SomeProperty);

            //TEST THAT AN EVENT WAS RAISED
            //var subject = new EditCustomerViewModel();
            //using (var monitoredSubject = subject.Monitor())
            //{
            //    subject.Foo();
            //    monitoredSubject.Should().Raise("NameChangedEvent");
            //}
        }

        [Test]
        [Category("AlanCategory")]
        [Ignore("Because I felt like it")]
        [TestCase(1, 2, 2)]
        [TestCase(2, 4, 8)]
        public void DivTwoNumbers_ValidInputs_ReturnGoodValue(int x, int y, int expectedResult)
        {            
            var result = sut.DivTwoNumbers(8, 4);                        
            Assert.That(result, Is.EqualTo(expectedResult));            
        }

        [Test]
        [Category("AlanCategory")]
        [TestCase(2, 1, ExpectedResult = 2)]
        [TestCase(8, 2, ExpectedResult = 4)]
        //[TestCaseSource(typeof(ClassThatGetsData), "TestCases")]    //Create a class with public static IEnumerable TestCases {yiled return ...
        public int DivTwoNumbers_ValidInputs_ReturnGoodValue2(int x, int y)
        {
            var result = sut.DivTwoNumbers(x, y);
            //Assert.That(result, Is.EqualTo(2));   //We remove this
            return result;
        }

        //Combinatorial test to see if exceptions are thrown
        //You can specify values or a range.  Range(4,6,1) means from 4 to 6 incremented by 1
        [Test]
        //[Sequential] Instead of 9 tests, only three.  Doesnt do combinations
        public void DivTwoNumbers_CombinatorialTests([Values(1, 2, 3)]int x, [Range(4, 6, 1)]int y) => sut.DivTwoNumbers(x, y);

        [Test][Sequential]
        public void DivTwoNumbers_SequentialTests([Values(2, 10, 20)]int x, [Values(1, 2, 4)]int y, [Values(2, 5, 5)]int expResult) => Assert.That(sut.DivTwoNumbers(x, y), Is.EqualTo(expResult));
        
        [Test]
        public void MockTests()
        {
            var mockService = new Mock<IService>();

            //We can setup all properties to be accessed. Must be done after creating mock and before any subsequent code
            //mockService.SetupAllProperties();

            var sutWithService = new TestClass(mockService.Object);

            mockService.Setup(x => x.Validate()).Returns(true);
            mockService.Setup(x => x.ValidateWithArgs(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            mockService.Setup(x => x.SomeValue).Returns(11);

            //This is an interface with no implementation but in the TestClass code, we do this._service.Count++;
            //To make this incremented, we do SetupProperty
            mockService.SetupProperty(x => x.Count);
            //mockService.SetupProperty(x => x.Count, 3);  //We can also set an initial value


            Assert.That(sutWithService.DivTwoNumbersDependsOnService(6,2), Is.EqualTo(3));
            Assert.That(sutWithService.Years, Is.EqualTo(15));
            Assert.That(mockService.Object.Count, Is.EqualTo(1));

            /* BEHAVIOR TESTING
             * Verify Method was called (w/ args) or certain number of times
             * Verify Property setters and getters were called and set
             * No other calls were made
             */

            mockService.Verify(x => x.Validate());
            mockService.Verify(x => x.ValidateWithArgs(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            
            mockService.VerifyGet(foo => foo.SomeValue);    // Verify getter invocation, regardless of value.
            mockService.VerifySet(foo => foo.SomeValue = It.IsAny<int>());  // Verify setter called with specific value            
            mockService.VerifySet(foo => foo.SomeValue = It.IsInRange(1, 5, Moq.Range.Inclusive));  // Verify setter with an argument matcher

            //mockService.Setup(x => x.Validate()).Throws<InvalidOperationException>();
            //Assert.That(sutWithService.DivTwoNumbersDependsOnService(6, 2), Throws.Exception.TypeOf<InvalidOperationException>());

            //Manually raises the event
            mockService.Raise(x => x.ValidateHappened += null, new ResolveEventArgs("args"));            
        }
    }
}
