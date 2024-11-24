using System;

//Added extra namespace suffix (AbstractFactory2) to avoid class name duplicates like ClamPizza
namespace WpfPlayground.DesignPatterns.Creational.AbstractFactoryPatterns
{
    public interface IDough { string getName(); }
    public class ThinCrustDough : IDough { public string getName() { return "TCD"; } }
    public interface IPIF { IDough createDough(); }
    public class NYPIF : IPIF { public IDough createDough() { return new ThinCrustDough(); } }

    public abstract class Pizza
    {
        public string Name { get; set; }
        public IDough dough;
        public abstract void prepare();
    }

    public class ClamPizza : Pizza
    {
        IPIF ingFac;
        public ClamPizza(IPIF ingFac) { this.ingFac = ingFac; }
        public override void prepare()
        {
            dough = ingFac.createDough();
            Name = "thincrust";
            Console.WriteLine("Preparing " + dough.getName());
        }
    }

    public abstract class PizzaStore
    {
        public void orderPizza()
        {
            Pizza pizza;
            pizza = createPizza(); pizza.prepare();
        }

        protected abstract Pizza createPizza();
    }

    public class NYPizzaStore : PizzaStore
    {
        IPIF ingFac;
        Pizza pizza;
        protected override Pizza createPizza()
        {
            ingFac = new NYPIF();
            //switch
            pizza = new ClamPizza(ingFac);
            //end switch
            return pizza;
        }
    }

    public class AbstractFactorySimple
    {
        public AbstractFactorySimple()
        {
            PizzaStore a = new NYPizzaStore();
            a.orderPizza();
        }
    }
}