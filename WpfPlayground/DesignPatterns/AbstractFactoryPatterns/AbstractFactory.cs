using System;

namespace WpfPlayground.DesignPatterns.AbstractFactoryPatterns
{
    public class AbstractFactory
    {
        public AbstractFactory()
        {
            PizzaStore nystore = new NYPizzaStore();
            nystore.OrderPizza("clam");
            /* Preparing New York Style Clam Pizza
             * Bake for 25 minutes at 350
             * Cutting the pizza into diagonal slices
             * Place pizza in official PizzaStore box
             */

            Console.WriteLine(nystore.OrderPizza("clam").Name);
            /* Preparing New York Style Clam Pizza
             * Bake for 25 minutes at 350
             * Cutting the pizza into diagonal slices
             * Place pizza in official PizzaStore box
             * New York Style Clam Pizza (This is the additional line)
             * To just get the previous line, you can do this
             * Pizza pizza=nystore.OrderPizza("clam"); writeLine(pizza.Name);
             */

            string z = Console.ReadLine();
        }
    }

    public abstract class Pizza
    {
        public string Name { get; set; }
        protected IDough dough;
        protected ISauce sauce;
        protected IClams clam;        

        public Pizza() { }
        public abstract void Prepare();

        public virtual void Bake() { Console.WriteLine("Bake for 25 minutes at 350 \n"); }

        public virtual void Cut() { Console.WriteLine("Cutting the pizza into diagonal slices \n"); }

        public virtual void Box() { Console.WriteLine("Place pizza in official PizzaStore box \n"); }
    }

    public abstract class PizzaStore
    {
        public PizzaStore() { }
        public Pizza OrderPizza(string type)
        {
            Pizza pizza = CreatePizza(type);
            pizza.Prepare();
            pizza.Bake();
            pizza.Cut();
            pizza.Box();
            return pizza;
        }

        protected abstract Pizza CreatePizza(string type);
    }

    public interface IDough { string toString();}
    public interface ISauce { string toString();}
    public interface IClams { string toString(); }

    public interface IPizzaIngredientFactory
    {
        IDough CreateDough();
        ISauce CreateSauce();
        IClams CreateClam();
    }

    public class ThinCrustDough : IDough
    {
        public ThinCrustDough() { }
        public string toString() { return "Thin Crust Dough"; }
    }

    public class MarinaraSauce : ISauce
    {
        public MarinaraSauce() { }
        public string toString() { return "Marinara Sauce"; }
    }

    public class FreshClams : IClams
    {
        public FreshClams() { }
        public string toString() { return "Fresh Clams"; }
    }

    public class ClamPizza : Pizza
    {
        IPizzaIngredientFactory ingredientFactory;

        public ClamPizza(IPizzaIngredientFactory ingredientFactory)
        {
            this.ingredientFactory = ingredientFactory;
        }

        public override void Prepare()
        {
            Console.WriteLine("Preparing " + Name);
            dough = ingredientFactory.CreateDough();
            sauce = ingredientFactory.CreateSauce();
            clam = ingredientFactory.CreateClam();
        }
    }

    public class NYPizzaIngredientFactory : IPizzaIngredientFactory
    {
        public NYPizzaIngredientFactory() { }

        public IDough CreateDough() { return new ThinCrustDough(); }
        public ISauce CreateSauce() { return new MarinaraSauce(); }
        public IClams CreateClam() { return new FreshClams(); }
    }

    public class NYPizzaStore : PizzaStore
    {
        public NYPizzaStore() { }

        protected override Pizza CreatePizza(string type)
        {
            Pizza pizza = null;
            IPizzaIngredientFactory ingredientFactory = new NYPizzaIngredientFactory();

            switch (type)
            {
                case "clam":
                    pizza = new ClamPizza(ingredientFactory);
                    pizza.Name = "New York Style Clam Pizza";
                    break;
            }
            return pizza;
        }
    }    
}