using System; 

namespace WpfPlayground.DesignPatterns
{
    public interface Duck {void Quack(); void Fly();}
    public interface Turkey { void Gobble(); void Fly();}

    public class DuckAdapter : Turkey
    {
        Duck duck;
        public DuckAdapter(Duck duck) {this.duck = duck;}
        public void Gobble() { duck.Quack(); }
        public void Fly() { duck.Fly(); }              
    }

    public class TurkeyAdapter : Duck
    {
        Turkey turkey;
        public TurkeyAdapter(Turkey turkey){this.turkey = turkey;}
        public void Quack() {turkey.Gobble();}

        public void Fly()
        {
            for (int i = 0; i < 5; i++)
                turkey.Fly();
        }        
    }

    public class MallardDuck : Duck
    {
        public MallardDuck(){ }
        public void Quack() { Console.WriteLine("Quack"); }
        public void Fly() { Console.WriteLine("I'm flying"); }        
    }

    public class WildTurkey : Turkey
    {
        public WildTurkey(){ }
        public void Gobble() { Console.WriteLine("Gooble, gooble"); }
        public void Fly() { Console.WriteLine("I'm flying a short distance"); }        
    }    
    
    public class Adapter
    {
        public Adapter()
        {
            MallardDuck myduck = new MallardDuck();
            WildTurkey myturkey = new WildTurkey();
            Duck turkeyadapter = new TurkeyAdapter(myturkey);
            myturkey.Gobble();
            myturkey.Fly();
            myduck.Quack();
            myduck.Fly();
            turkeyadapter.Quack();
            turkeyadapter.Fly();
        }        
    }
}