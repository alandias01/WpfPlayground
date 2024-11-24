using System;

namespace WpfPlayground.DesignPatterns.Structural
{
    public abstract class Beverage
    {
        public string description = "Unknown Beverage";
        public virtual string getDescription() { return description; }
        public abstract double cost();
    }

    public abstract class CondimentDecorator : Beverage
    {
        //all condiment decorators required to reimplement getdescription
        //public abstract string getDescription(); //
    }

    public class Espresso : Beverage
    {
        public Espresso() { description = "Espresso"; }
        public override double cost() { return 1.99; }
    }

    public class Mocha : CondimentDecorator
    {
        Beverage beverage;
        public Mocha(Beverage beverage) { this.beverage = beverage; }
        public override string getDescription() { return beverage.getDescription() + ", Mocha"; }
        public override double cost() { return .2 + beverage.cost(); }
    }

    public class Whip : CondimentDecorator
    {
        Beverage beverage;
        public Whip(Beverage beverage) { this.beverage = beverage; }
        public override string getDescription() { return beverage.getDescription() + ", Whip"; }
        public override double cost() { return .3 + beverage.cost(); }
    }

    public class Decorator
    {
        public Decorator()
        {
            Beverage beverage = new Espresso();
            Console.WriteLine(beverage.getDescription() + " $" + beverage.cost());
            //Espresso $1.99

            Beverage beverage2 = new Espresso();
            beverage2 = new Mocha(beverage2);
            beverage2 = new Whip(beverage2);
            Console.WriteLine(beverage2.getDescription() + " $" + beverage2.cost());
            //Espresso, Mocha, Whip $2.49


            string z = Console.ReadLine();
        }
    }
}