using System;

namespace WpfPlayground.DesignPatterns.Creational.FactoryPatterns
{
    public enum bondType { corp, gov }

    public abstract class Bond
    {
        public string name;
        public void processing() { Console.WriteLine("You are purchasing a " + name); }
    }

    public class Gov : Bond { public Gov() { name = "Government Bond"; } }
    public class Corp : Bond { public Corp() { name = "Corporate Bond"; } }

    public class Exchange
    {
        public Bond buyBond(bondType type)
        {
            Bond bond = createBond(type);
            bond.processing();
            return bond;
        }

        private Bond createBond(bondType type)
        {
            Bond bond = null;
            if (type.Equals(bondType.corp)) { bond = new Corp(); }
            else if (type.Equals(bondType.gov)) { bond = new Corp(); }
            return bond;
        }
    }

    public class FactoryUsingBonds
    {
        public FactoryUsingBonds()
        {
            Exchange exchange = new Exchange();
            exchange.buyBond(bondType.corp);
        }
    }
}