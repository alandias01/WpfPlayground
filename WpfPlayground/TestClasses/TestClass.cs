using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace WpfPlayground.TestClasses
{
    //No Implementation, used for Mocking
    public interface IService
    {
        int Count { get; set; }
        int SomeValue { get; set; }

        event EventHandler<ResolveEventArgs> ValidateHappened;

        bool Validate();
        bool ValidateWithArgs(int x, int y);
    }

    public interface ITestClass
    {
        double DoubleValue { get; set; }
        string Name { get; set; }
        int Years { get; set; }

        int DivTwoNumbers(int x, int y);        
    }

    public class TestClass : IEquatable<TestClass>, ITestClass
    {
        private IService _service;

        public TestClass()
        {
        }

        public TestClass(int y)
        {
            this.Years = y;
            this.Name = "Alan";
        }

        public TestClass(IService service)
        {
            this._service = service;
            this._service.SomeValue = 1;
        }

        public string Name { get; set; }
        public int Years { get; set; }
        public List<TestClass> TestingMethodsList = new List<TestClass>();
        public double DoubleValue { get; set; }

        public int DivTwoNumbers(int x, int y)
        {
            var z = x / y;
            return z;
        }

        public int DivTwoNumbersDependsOnService(int x, int y)
        {
            int z = 0;

            try
            {
                z = x / y;

                var validate = this._service.Validate();
                if (!validate)
                    throw new Exception("Faild Validation");

                var validateWithArgs = this._service.ValidateWithArgs(x, y);
                if (!validateWithArgs)
                    throw new Exception("Faild Validation with Args");

                this.Years = this._service.SomeValue < 10 ? 1 : 15;

                this._service.Count++;
            }
            catch (Exception e)
            {
                //Log
            }
            
            return z;
        }

        public bool Equals([AllowNull] TestClass other) => this.Years.Equals(other.Years);
    }
}
