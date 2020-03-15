using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfPlayground.DesignPatterns
{
    public class Bridge
    {
        public Bridge()
        {
            ReverseFormatter r = new ReverseFormatter();
            Book b = new Book(r);
            Paper p = new Paper(r);

            b.Title = "Alans Book";
            p.PaperName = "Alans Paper";

            List<ManuScript> MList = new List<ManuScript>();
            MList.Add(b);
            MList.Add(p);

            foreach (var item in MList)
            {
                item.Print();
            }
        }        
    }

    public interface IFormatter { string Format(string key, string value); }

    public class ReverseFormatter : IFormatter
    {
        public string Format(string key, string value)
        {
            return key + " " + new string(value.Reverse().ToArray());
        }
    }

    public abstract class ManuScript
    {
        protected IFormatter Formatter;
        public ManuScript(IFormatter formatter)
        {
            Formatter = formatter;
        }

        public abstract void Print();        
    }

    public class Book : ManuScript
    {
        public Book(IFormatter formatter) : base(formatter) { }
        
        public string Title { get; set; }
        public string Author { get; set; }

        public override void Print()
        {
            Console.WriteLine(Formatter.Format("Title", Title));
        }
    }

    public class Paper : ManuScript
    {
        public Paper(IFormatter formatter) : base(formatter) { }
        
        public string PaperName { get; set; }
        public string Author { get; set; }

        public override void Print()
        {
            Console.WriteLine(Formatter.Format("Name", PaperName));
        }
    }

}




