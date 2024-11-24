using System;

namespace WpfPlayground.DesignPatterns.Creational.AbstractFactoryPatterns
{
    //Create a person with an arm object that if normal, 5 fingers, retard has 6

    public class AbstractFactoryPractice
    {
        public AbstractFactoryPractice()
        {
            var p = new Person(new RetardedPersonFactory());
            Console.WriteLine(p.Arm.fingers);
        }
    }

    public interface IPersonFactory
    {
        IArm CreateArm();
    }

    public class NormalPersonFactory : IPersonFactory
    {
        public IArm CreateArm()
        {
            return new NormalArm();
        }
    }
    public class RetardedPersonFactory : IPersonFactory
    {
        public IArm CreateArm()
        {
            return new RetardArm();
        }
    }

    public interface IArm
    {
        int fingers { get; set; }
    }

    public class NormalArm : IArm
    {
        public int fingers { get; set; }
        public NormalArm() { fingers = 5; }
    }

    public class RetardArm : IArm
    {
        public int fingers { get; set; }
        public RetardArm() { fingers = 6; }
    }

    public class Person
    {
        public IArm Arm;
        IPersonFactory IPF;
        public Person(IPersonFactory IPF)
        {
            this.IPF = IPF;
            Arm = IPF.CreateArm();
        }
    }
}