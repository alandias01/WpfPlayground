using System;

namespace WpfPlayground.DataStructuresAlgorithms
{   
    public class LinkedLists
    {
        public LinkedLists()
        {
            mynode a1 = new mynode("alan1"); mynode a2 = new mynode("alan2");
            mynode a3 = new mynode("alan3"); mynode a4 = new mynode("alan4");

            MyLinkedList connectAll = new MyLinkedList();
            connectAll.add(a1); connectAll.add(a2);
            connectAll.add(a3); connectAll.addBetween(a2, a4);
            connectAll.showList();    //alan1 alan2 alan4 alan3
        }

        public void m1() { }
    }

    public class mynode
    {
        public object element; //Can hold any type of data
        public mynode link; //Holds reference to next Node

        public mynode()
        {
            element = null;
            link = null;
        }

        public mynode(object theElement)
        {
            element = theElement;
            link = null;
        }
    }

    public class MyLinkedList
    {
        mynode header;
        public MyLinkedList()
        {
            header = new mynode("header"); //Creates intitial node
        }

        public void add(mynode item) //Adds to end of list
        {
            mynode current = new mynode();
            mynode newItem = new mynode(item.element);
            current = header;
            while (current.link != null)
                current = current.link;
            current.link = newItem;
        }

        public void addBetween(mynode afterItem, mynode newItem)
        {
            mynode current = new mynode();
            mynode newNode = new mynode(newItem.element);
            current = find(afterItem);
            newNode.link = current.link;
            current.link = newNode;
        }

        private mynode find(mynode node)
        {
            mynode current = new mynode();
            current = header;
            while (current.element != node.element)
                current = current.link;
            return current;
        }

        public void showList()
        {
            mynode iter = new mynode();
            iter = header;
            while (iter.link != null)
            {
                Console.WriteLine(iter.link.element);
                iter = iter.link;
            }
        }
    }
}
