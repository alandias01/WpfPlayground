using System;

namespace WpfPlayground.DesignPatterns.Creational.FactoryPatterns
{
    public abstract class Pizza
    {
        public string name;
        public void prepare() { Console.WriteLine("preparing " + name); }
        public virtual void cut() { Console.WriteLine("Cutting diagonal slices"); }
        public string getName() { return name; }
    }

    public class NYStyleCheesePizza : Pizza
    { public NYStyleCheesePizza() { name = "NYCheesePizza"; } }

    public class NYStyleMeatPizza : Pizza
    {
        public NYStyleMeatPizza() { name = "NYMeatPizza"; }
        public override void cut() { Console.WriteLine("Cutting square slices"); }
    }

    public class CHStyleCheesePizza : Pizza
    { public CHStyleCheesePizza() { name = "CHCheesePizza"; } }

    public abstract class PizzaStore
    {
        public Pizza orderPizza(string type)
        {
            Pizza pizza;
            pizza = createPizza(type); //replaces if type=cheese, pizza=new cheesePizza
            pizza.prepare(); pizza.cut();
            return pizza;
        }

        protected abstract Pizza createPizza(string type);
    }

    public class NYPizzaStore : PizzaStore
    {
        protected override Pizza createPizza(string item)
        {
            if (item.Equals("cheese")) { return new NYStyleCheesePizza(); }
            else if (item.Equals("meat")) { return new NYStyleMeatPizza(); }
            else return null;
        }
    }

    public class CHPizzaStore : PizzaStore
    {
        protected override Pizza createPizza(string item)
        {
            if (item.Equals("cheese")) { return new CHStyleCheesePizza(); }
            else return null;
        }
    }
    public class Factory
    {
        public Factory()
        {
            PizzaStore nystore = new NYPizzaStore();
            Pizza pizza = nystore.orderPizza("cheese");
            Console.WriteLine("You ordered a " + pizza.getName() + "\n");
            //Preparing NYCheesePizza, cutting diagonal slices, you ordered a NYCheesePizza 

            pizza = nystore.orderPizza("meat");
            Console.WriteLine("You ordered a " + pizza.getName() + "\n");
            //Preparing NYMeatPizza, cutting square slices, you ordered a NYMeatPizza

            PizzaStore chstore = new CHPizzaStore();
            pizza = chstore.orderPizza("cheese");
            Console.WriteLine("You ordered a " + pizza.getName() + "\n");
            //Preparing CHCheesePizza, cutting diagonal slices, you ordered a CHCheesePizza
            string z = Console.ReadLine();
        }
    }
}