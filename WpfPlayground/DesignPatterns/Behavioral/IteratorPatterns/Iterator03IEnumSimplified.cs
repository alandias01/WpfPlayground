using System;
using System.Collections;

namespace WpfPlayground.DesignPatterns.Behavioral.IteratorPatterns
{
    interface Menu { IEnumerator createIterator(); }

    class MenuItem
    {
        public MenuItem(string n, double p) { this.name = n; this.price = p; }
        string name; double price; public string getName() { return name; }
        public double getPrice() { return price; }
    }

    class PancakeMenu : Menu
    {
        ArrayList menuItems = new ArrayList();
        public PancakeMenu()
        { menuItems.Add(new MenuItem("pancake 1", 2.00)); menuItems.Add(new MenuItem("pancake 2", 3.00)); }

        public IEnumerator createIterator() { return menuItems.GetEnumerator(); }

    }

    class DinnerMenu : Menu
    {
        MenuItem[] menuItems = new MenuItem[2];
        public DinnerMenu() { menuItems[0] = new MenuItem("Dinner 1", 3.50); menuItems[1] = new MenuItem("Dinner 2", 4.50); }
        public IEnumerator createIterator() { return menuItems.GetEnumerator(); }
    }

    class Waitress
    {
        ArrayList menus;
        public Waitress(ArrayList m) { this.menus = m; }
        public void printMenu()
        {
            foreach (Menu myMenu in menus)
            {
                IEnumerator iter = myMenu.createIterator();
                while (iter.MoveNext())
                {
                    MenuItem mi = (MenuItem)iter.Current;
                    Console.WriteLine(mi.getName());
                }
            }
        }
    }

    public class Iterator03IEnumSimplified
    {
        public Iterator03IEnumSimplified()
        {
            PancakeMenu p = new PancakeMenu(); DinnerMenu d = new DinnerMenu();
            ArrayList aw = new ArrayList() { p, d };
            Waitress w = new Waitress(aw);
            w.printMenu();
        }
    }
}