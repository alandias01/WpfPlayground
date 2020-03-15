using System;
using System.Text;

namespace WpfPlayground.DesignPatterns.StatePatterns
{
    public class GumballMachineStart
    {
        const int SOLD_OUT = 0;
        int NO_QUARTER = 1, HAS_QUARTER = 2, SOLD = 3;

        int count = 0, state = SOLD_OUT;        

        public GumballMachineStart(int count)
        { this.count = count; if (count > 0){state = NO_QUARTER;} }
        

        public void InsertQuarter()
        {
            if (state == HAS_QUARTER)
            { Console.WriteLine("You can't insert another quarter"); }

            else if(state ==NO_QUARTER)
            { state = HAS_QUARTER; Console.WriteLine("You inserted a quarter"); }

            else if(state ==SOLD_OUT)
            { Console.WriteLine("You can't insert a quarter, the machine is sold out"); }

            else if (state == SOLD)
            { Console.WriteLine("Please wait, we're already giving you a gumball"); }
            
        }

        public void EjectQuarter()
        {
            if (state == HAS_QUARTER)
            { Console.WriteLine("Quarter returned"); state = NO_QUARTER; }

            else if (state == NO_QUARTER)
            { Console.WriteLine("You haven't inserted a quarter"); }

            else if (state == SOLD_OUT)
            { Console.WriteLine("You can't eject, you haven't inserted a quarter yet"); }

            else if (state == SOLD)
            { Console.WriteLine("Sorry, you already turned the crank"); }

        }

        public void TurnCrank()
        {
            if (state == HAS_QUARTER)
            { Console.WriteLine("You turned..."); state = SOLD; Dispense(); }

            else if (state == NO_QUARTER)
            { Console.WriteLine("You turned but there's no quarter"); }

            else if (state == SOLD_OUT)
            { Console.WriteLine("You turned, but there are no gumballs"); }

            else if (state == SOLD)
            { Console.WriteLine("Turning twice doesn't get you another gumball!"); }

        }

        public void Dispense()
        {
            if (state == HAS_QUARTER)
            { Console.WriteLine("\nNo gumball dispensed"); }

            else if (state == NO_QUARTER)
            { Console.WriteLine("\nYou need to pay first"); }

            else if (state == SOLD_OUT)
            { Console.WriteLine("\nNo gumball dispensed"); }

            else if (state == SOLD)
            { 
                Console.WriteLine("\nA gumball comes rolling out the slot");
                count -= 1;
                if (count == 0)
                {
                    Console.WriteLine("\nNow out of gumballs!");
                    state = SOLD_OUT;
                }
                else {state = NO_QUARTER;}
            }
        }
       
        public void Refill(int newGumballs)
        {this.count = newGumballs; state = NO_QUARTER;}

        public string MachineState()
        {
            StringBuilder result = new StringBuilder();
            result.Append("\nMighty Gumball, Inc.");
            result.Append("\nC# Enabled Standing Gumball Model #2005\n");
            result.Append("Inventory: " + count + " gumball");
            if (count != 1) {result.Append("s");}
            
            result.Append("\nMachine is ");
            if (state == SOLD_OUT){result.Append("sold out");}
            else if (state == NO_QUARTER){result.Append("waiting for quarter");}
            else if (state == HAS_QUARTER){result.Append("waiting for turn of crank");}
            else if (state == SOLD){result.Append("delivering a gumball");}
            return result.ToString()+"\n";
        }

        public override string ToString(){return MachineState();}
    }


    public class StateExampleWithoutStatePattern
    {
        public StateExampleWithoutStatePattern()
        {
            GumballMachineStart gbm = new GumballMachineStart(5);
            Console.WriteLine(gbm);
            gbm.InsertQuarter(); gbm.TurnCrank(); Console.WriteLine(gbm);
            /*
                Mighty Gumball, Inc.
                C# Enabled Standing Gumball Model #2005
                Inventory: 5 gumballs
                Machine is waiting for quarter

                You inserted a quarter
                You turned...

                A gumball comes rolling out the slot

                Mighty Gumball, Inc.
                C# Enabled Standing Gumball Model #2005
                Inventory: 4 gumballs
                Machine is waiting for quarter
            */



            string z = Console.ReadLine();
        } //Main

    }//Program

} //namespace Console_practice