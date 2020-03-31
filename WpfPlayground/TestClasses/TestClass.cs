using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace WpfPlayground.TestClasses
{
    public class TestClass : IEquatable<TestClass>
    {
        public TestClass()
        {
        }

        public TestClass(int y)
        {
            this.Years = y;
            this.Name = "Alan";
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

        public bool Equals([AllowNull] TestClass other)
        {
            return this.Years.Equals(other.Years);
        }
    }
}
