using System;
using System.Collections;

namespace WpfPlayground.DesignPatterns
{
    public abstract class MenuComponent
    {
        public virtual void Add(MenuComponent menuComponent)
        {throw new NotImplementedException();}        

        public virtual void Remove(MenuComponent menuComponent)
        {throw new NotImplementedException();}

        public virtual string getName()
        { throw new NotImplementedException(); }

        public virtual string getDesc()
        { throw new NotImplementedException(); }

        public virtual double getPrice()
        { throw new NotImplementedException(); }

        public virtual MenuComponent GetChild(int i)
        {throw new NotImplementedException();}

        public virtual void Print()
        { throw new NotImplementedException(); }
    }

    public class Menu : MenuComponent
    {
        ArrayList menuComponents = new ArrayList();
        string name, description;

        public Menu(string n, string d) { this.name = n; this.description = d; }

        public override void Add(MenuComponent menuComponent)
        {menuComponents.Add(menuComponent);}

        public override void Remove(MenuComponent menuComponent)
        { menuComponents.Remove(menuComponent); }

        public override string getName() {return name;}
        public override string getDesc(){return description;}
        public override MenuComponent GetChild(int i){ return (MenuComponent)menuComponents[i]; }
        
        public override void Print()
        {
            Console.WriteLine("\n" + getName() + " (" + getDesc() + ")");
            Console.WriteLine("------------------");

            IEnumerator iter = menuComponents.GetEnumerator();
            while (iter.MoveNext())
            {
                MenuComponent menuComponent = (MenuComponent)iter.Current;
                menuComponent.Print();            
            }            
        }
    }

    public class MenuItem : MenuComponent
    {
        string name; double price;
        public MenuItem(string n, double p) { this.name = n; this.price = p; }
        public override string getName() { return name; } 
        public override double getPrice() { return price; }
        public override void Print()
        { Console.WriteLine(" "+getName()+" "+getPrice()); }
    }

    public class Waitress
    {
        MenuComponent allMenus;
        public Waitress(MenuComponent allMenus) { this.allMenus = allMenus; }
        public void printMenu() { allMenus.Print(); }
    }

    public class Composite
    {        
        public Composite()
        {
            MenuComponent allMenus = new Menu("All Menus", "All Menus combined");
            MenuComponent phMenu = new Menu("Pancake Menu", "Breakfast");
            MenuComponent dMenu = new Menu("Dinner Menu", "Dinner");
            MenuComponent dessert = new Menu("Dessert Menu","Dessert");


            allMenus.Add(phMenu);
            allMenus.Add(dMenu);
            
            phMenu.Add(new MenuItem("dinner 1", 3.00)); //adding a menuitem to breakfast

            dMenu.Add(dessert); //Adding a SubMenu to a Menu

            dessert.Add(new MenuItem("dessert 1", 4.00));

            Waitress w = new Waitress(allMenus);
            w.printMenu();

            string z = Console.ReadLine();
        }
    }
}