using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfPlayground
{
    public class Playground
    {
        string name;

        // Property accessors sometimes are 1 liners (get) to return the result of an expression
        // You can implement these properties as expression-bodied members
        public string ExpressionBodyDefinitionToImplementReadOnlyProperty => name;

        public string ExpressionBodyDefinitionToImplementPropertyGetSet
        {
            get => name;
            set => name = value;
            //set
            //{
            //    name = value;
            //}
        }

        public string ExpressionBodyDefinitionForMember(int x) => x > 5 ? "Greater than 5" : "Less than or = to 5";

        // An expression body definition for a constructor typically consists of a single assignment expression
        // or a method call that handles the constructor's arguments or initializes instance state.
        public Playground(string _name) => name = _name;
        public Playground(string _name, int age) => CtorInit(_name, age);
        public void CtorInit(string _name, int age) { }

        public Playground()
        {
        }
    }
}
