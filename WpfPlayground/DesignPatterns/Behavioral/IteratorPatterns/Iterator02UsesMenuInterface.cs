using System;
using System.Collections;

namespace WpfPlayground.DesignPatterns.Behavioral.IteratorPatternsUsesMenuInterface
{
    public interface IIterator { bool HasNext(); object Next(); }  //New    
    public interface Menu { IIterator CreateIterator(); }  //New

    public class DinnerMenuIterator : IIterator  //New
    {
        MenuItem[] items;
        int position = 0;

        public DinnerMenuIterator(MenuItem[] items)
        { this.items = items; }

        public bool HasNext()
        {
            if (position >= items.Length || items[position] == null)
            { return false; }

            else { return true; }
        }

        public object Next()
        {
            MenuItem menuItem = items[position];
            position += 1;
            return menuItem;
        }
    }

    public class PancakeHouseIterator : IIterator  //New
    {
        ArrayList menuItems = new ArrayList();
        int position = 0;

        public PancakeHouseIterator(ArrayList menuItems)
        { this.menuItems = menuItems; }

        public bool HasNext()
        {
            if (position >= menuItems.Count || menuItems[position] == null)
            { return false; }

            else { return true; }
        }

        public object Next()
        {
            MenuItem menuItem = (MenuItem)menuItems[position];
            position += 1;
            return menuItem;
        }
    }

    public class Waitress
    {
        Menu phMenu, dMenu;

        public Waitress(Menu phMenu, Menu dMenu)
        { this.phMenu = phMenu; this.dMenu = dMenu; }



        public void printMenu()
        {
            IIterator pancakeIterator = phMenu.CreateIterator();
            IIterator dinnerIterator = dMenu.CreateIterator();
            Console.WriteLine("BreakFastMenu\n");
            printMenu(pancakeIterator);
            Console.WriteLine("DinnerMenu\n");
            printMenu(dinnerIterator);
        }

        private void printMenu(IIterator iter)
        {
            while (iter.HasNext())
            {
                MenuItem menuItem = (MenuItem)iter.Next();
                Console.WriteLine(menuItem.getName() + "\n" + menuItem.getPrice());
            }
        }
    }

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
            AddItem("Pancake 1", "P w/e_and_t", true, 2.99);
            AddItem("Pancake 2", "P w/e_and_s", false, 2.99);
            AddItem("Pancake 3", "P w/e_and_b", true, 3.49);
            AddItem("Waffles", "W w/b_and_s", true, 3.59);
        }

        public void AddItem(string name, string desc, bool isVeg, double price)
        {
            MenuItem menuItem = new MenuItem(name, desc, isVeg, price);
            menuItems.Add(menuItem);
        }

        public ArrayList getMenuItems() { return menuItems; }

        public IIterator CreateIterator() //New
        {
            return new PancakeHouseIterator(menuItems);
            //return menuItems.  Iterator(); NEW

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

            AddItem("Veg BLT", "(F)BLT", true, 2.99);
            AddItem("BLT", "BLT W", false, 2.99);
            AddItem("Soup", "S w/salad", false, 3.29);
            AddItem("Hotdog", "HD srotc", false, 3.05);
            AddItem("Veg 2", "veg Wbr", true, 3.99);
            AddItem("Pasta", "spgti w mb", true, 3.89);
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

        public IIterator CreateIterator() //New
        { return new DinnerMenuIterator(menuItems); }
    }

    public class Iterator02UsesMenuInterface
    {
        public Iterator02UsesMenuInterface()
        {
            PancakeHouseMenu phm = new PancakeHouseMenu();
            ArrayList breakfastItems = phm.getMenuItems();
            DinnerMenu dm = new DinnerMenu();
            MenuItem[] lunchItems = dm.getMenuItems();

            for (int i = 0; i < breakfastItems.Count; i++)
            {
                MenuItem menuItem = (MenuItem)breakfastItems[i];
                Console.WriteLine(menuItem.getName());
            }

            for (int j = 0; j < lunchItems.Length; j++)
            {
                MenuItem menuItem = lunchItems[j];
                Console.WriteLine(menuItem.getName());
            }

            //New 1
            PancakeHouseMenu phMenu2 = new PancakeHouseMenu();
            DinnerMenu dMenu2 = new DinnerMenu();
            Waitress waitress = new Waitress(phMenu2, dMenu2);
            waitress.printMenu();
        }
    }
}