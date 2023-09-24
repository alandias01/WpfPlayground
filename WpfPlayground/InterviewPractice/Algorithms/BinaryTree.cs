using System;
using System.Collections.Generic;
using System.Text;

namespace WpfPlayground.InterviewPractice.Algorithms
{
    public class BinaryTree
    {
        public BinaryTree()
        {

        }
    }

    public class Node
    {
        public Node(int num) => this.Current = num;

        public int Current { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public void Reverse()
        {
            Node temp = this.Left;
            this.Left = this.Right;
            this.Right = temp;
        }
    }

    public class BTree
    {
        public void AddNode(int num)
        {
        }
    }
}
