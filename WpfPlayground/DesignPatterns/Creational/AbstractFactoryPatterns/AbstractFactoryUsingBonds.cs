using System;

namespace WpfPlayground.DesignPatterns.Creational.AbstractFactoryPatterns
{
    /*
     * Bond has attributes like maturity date
     * Different types of bonds like cmo and muni
     * The value of the attribute depends on where the muni was created
     * Maturity date is different in NY and NJ
     */

    public interface IMaturity { DateTime getDate(); }

    public class NYMaturity : IMaturity
    { public DateTime getDate() { return new DateTime(2010, 9, 1); } }

    public class NJMaturity : IMaturity
    { public DateTime getDate() { return new DateTime(2012, 8, 8); } }

    public interface IExchangeAttributeFactory
    { IMaturity setMaturity(); }

    public class NYExchangeAttributeFactory : IExchangeAttributeFactory
    { public IMaturity setMaturity() { return new NYMaturity(); } }

    public class NJExchangeAttributeFactory : IExchangeAttributeFactory
    { public IMaturity setMaturity() { return new NJMaturity(); } }

    public abstract class Bond
    { public IMaturity maturity; }

    public class MuniBond : Bond
    {
        public MuniBond(IExchangeAttributeFactory IEAF)
        {
            maturity = IEAF.setMaturity();
        }
    }

    public abstract class Exchange
    {
        Bond bond;
        public Exchange() { bond = createBond(); }

        public abstract Bond createBond();

        public void show() { Console.WriteLine(bond.maturity.getDate()); }
    }

    public class NYExchange : Exchange
    {
        IExchangeAttributeFactory IEAF;
        public override Bond createBond()
        {
            IEAF = new NYExchangeAttributeFactory();
            Bond muni = new MuniBond(IEAF);
            return muni;
        }
    }

    public class NJExchange : Exchange
    {
        IExchangeAttributeFactory IEAF;
        public override Bond createBond()
        {
            IEAF = new NJExchangeAttributeFactory();
            Bond muni = new MuniBond(IEAF);
            return muni;
        }
    }

    class AbstractFactoryUsingBonds
    {
        public AbstractFactoryUsingBonds()
        {
            Exchange myExchange = new NYExchange();
            myExchange.show();
        }
    }
}
