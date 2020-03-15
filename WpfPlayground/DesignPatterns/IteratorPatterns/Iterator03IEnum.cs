using System;
using System.Collections;

namespace WpfPlayground.DesignPatterns.IteratorPatterns.IteratorPatterns03
{
    public interface Menu { IEnumerator CreateIterator();}

    public class MenuItem
    {
        string name, description; bool vegetarian; double price;
        public MenuItem(string name, string description, bool vegetarian, double price)
        {
            this.name = name; this.description = description;
            this.vegetarian = vegetarian; this.price = price;
        }
        public string getName() { return name; }
        public string getDescription() { return description; }
        public bool isVegetarian() { return vegetarian; }
        public double getPrice() { return price; }
    }

    public class PancakeHouseMenu : Menu
    {
        ArrayList menuItems;

        public PancakeHouseMenu()
        {
            menuItems = new ArrayList();
            AddItem("Pancake 1", "P w/e_and_t", true, 2.99); AddItem("Pancake 2", "P w/e_and_s", false, 2.99);
            AddItem("Pancake 3", "P w/e_and_b", true, 3.49); AddItem("Waffles", "W w/b_and_s", true, 3.59);
        }

        public void AddItem(string name, string desc, bool isVeg, double price)
        {
            MenuItem menuItem = new MenuItem(name, desc, isVeg, price);
            menuItems.Add(menuItem);
        }

        public ArrayList getMenuItems() { return menuItems; }

        public IEnumerator CreateIterator() //New
        {
            return menuItems.GetEnumerator();
        }
    }

    public class DinnerMenu : Menu
    {
        static int MAX_ITEMS = 6;
        int numberOfItems = 0;
        MenuItem[] menuItems;

        public DinnerMenu()
        {
            menuItems = new MenuItem[MAX_ITEMS];

            AddItem("Veg BLT", "(F)BLT", true, 2.99); AddItem("BLT", "BLT W", false, 2.99);
            AddItem("Soup", "S w/salad", false, 3.29); AddItem("Hotdog", "HD srotc", false, 3.05);
            AddItem("Veg 2", "veg Wbr", true, 3.99); AddItem("Pasta", "spgti w mb", true, 3.89);
        }

        public void AddItem(string name, string desc, bool isVeg, double price)
        {
            MenuItem menuItem = new MenuItem(name, desc, isVeg, price);
            if (numberOfItems >= MAX_ITEMS)
            { Console.WriteLine("Sorry, menu is full! Can't add item to menu"); }
            else
            { menuItems[numberOfItems] = menuItem; numberOfItems += 1; }
        }

        public MenuItem[] getMenuItems() { return menuItems; }

        public IEnumerator CreateIterator() //New
        { return menuItems.GetEnumerator(); }
    }

    public class Waitress
    {
        ArrayList menus;

        public Waitress(ArrayList menus)
        { this.menus = menus; }

        public void printMenu()
        {
            IEnumerator menuIterator = menus.GetEnumerator();
            while (menuIterator.MoveNext())
            {
                Menu menu = (Menu)menuIterator.Current;
                printMenu(menu.CreateIterator());
            }
        }

        private void printMenu(IEnumerator iter)
        {
            while (iter.MoveNext())
            {
                MenuItem menuItem = (MenuItem)iter.Current;
                Console.WriteLine(menuItem.getName() + "\n" + menuItem.getPrice());
            }
        }
    }

    public class Iterator03IEnum
    {
        public Iterator03IEnum()
        {
            PancakeHouseMenu phMenu2 = new PancakeHouseMenu();
            DinnerMenu dMenu2 = new DinnerMenu();

            ArrayList menus = new ArrayList();
            menus.Add(phMenu2); menus.Add(dMenu2);
            Waitress waitress = new Waitress(menus);
            waitress.printMenu();
        }
    }
}