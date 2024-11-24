using System;
using System.Text;

namespace WpfPlayground.DesignPatterns.Behavioral.StatePatterns
{
    public class GumballMachine
    {
        State soldOutState, noQuarterState, hasQuarterState, soldState, state;
        int count = 0;

        public GumballMachine(int numberOfGumballs)
        {
            state = soldOutState;
            soldOutState = new SoldOutState(this);
            noQuarterState = new NoQuarterState(this);
            hasQuarterState = new HasQuarterState(this);
            soldState = new SoldState(this);
            count = numberOfGumballs;

            if (numberOfGumballs > 0) { state = noQuarterState; }
            else { state = soldOutState; }
        }

        public State getsoldOutState() { return soldOutState; }
        public State getNoQuarterState() { return noQuarterState; }
        public State gethasQuarterState() { return hasQuarterState; }
        public State getsoldState() { return soldState; }
        public int getCount() { return count; }


        public void InsertQuarter() { state.InsertQuarter(); }
        public void EjectQuarter() { state.EjectQuarter(); }
        public void TurnCrank() { state.TurnCrank(); state.Dispense(); }
        public void setState(State state) { this.state = state; }

        public void ReleaseBall()
        {
            Console.WriteLine("A gumball comes rolling out the slot...\n");
            if (count != 0) { count -= 1; }
        }

        public void Refill(int numberOfGumballs)
        {
            count += numberOfGumballs;
            state = noQuarterState;
            Console.WriteLine("\nRefill: " + numberOfGumballs +
                " gumballs were added. " +
                "The gumball count in now: " + count + "\n");
        }

        public string MachineStateHeader()
        {
            StringBuilder result = new StringBuilder();
            result.Append("Mighty Gumball, Inc.");
            result.Append("\nC# Enabled Standing Gumball Model #2005\n");
            result.Append("Inventory: " + count + " gumball");
            if (count != 1) { result.Append("s"); }
            result.Append("\nMachine is " + state.ToString());
            return result.ToString();
        }

        public override string ToString() { return MachineStateHeader(); }
    }

    public interface State
    {
        void InsertQuarter(); void EjectQuarter();
        void TurnCrank(); void Dispense();
    }

    public class SoldOutState : State
    {
        GumballMachine gumBallMachine;
        public SoldOutState(GumballMachine gumballMachine)
        { gumBallMachine = gumballMachine; }

        public void InsertQuarter() { Console.WriteLine("Can't insert quarter, machine is soldout\n"); }
        public void EjectQuarter() { Console.WriteLine("Can't eject, you haven't inserted a quarter yet\n"); }
        public void TurnCrank() { Console.WriteLine("Sorry, you already turned the crank\n"); }
        public void Dispense()
        {
            gumBallMachine.ReleaseBall();
            Console.WriteLine("You turned, but there are no gumballs\n");
        }
        public override string ToString() { return "sold out"; }
    }

    public class NoQuarterState : State
    {
        GumballMachine gumBallMachine;
        public NoQuarterState(GumballMachine gumballMachine)
        { gumBallMachine = gumballMachine; }

        public void InsertQuarter()
        {
            Console.WriteLine("You inserted a quarter\n");
            gumBallMachine.setState(gumBallMachine.gethasQuarterState());
        }

        public void EjectQuarter() { Console.WriteLine("You haven't inserted a quarter\n"); }
        public void TurnCrank() { Console.WriteLine("You turned but there's no quarter\n"); }
        public void Dispense() { Console.WriteLine("You need to pay first\n"); }
        public override string ToString() { return "waiting for quarter"; }
    }

    public class SoldState : State
    {
        GumballMachine gumBallMachine;
        public SoldState(GumballMachine gumballMachine)
        { gumBallMachine = gumballMachine; }

        public void InsertQuarter() { Console.WriteLine("Please wait, we're already giving you a gumball\n"); }
        public void EjectQuarter() { Console.WriteLine("Sorry, you already turned the crank\n"); }
        public void TurnCrank() { Console.WriteLine("Turning twice doesn't get you another gumball!\n"); }
        public void Dispense()
        {
            gumBallMachine.ReleaseBall();
            if (gumBallMachine.getCount() > 0)
            { gumBallMachine.setState(gumBallMachine.getNoQuarterState()); }
            else
            {
                Console.WriteLine("Oops, out of gumballs!\n");
                gumBallMachine.setState(gumBallMachine.getsoldOutState());
            }
        }
        public override string ToString() { return "delivering a gumball"; }
    }

    public class HasQuarterState : State
    {
        GumballMachine gumballMachine;

        public HasQuarterState(GumballMachine gumballMachine)
        { this.gumballMachine = gumballMachine; }

        public void InsertQuarter()
        { Console.WriteLine("You can't insert another quarter\n"); }

        public void EjectQuarter()
        {
            Console.WriteLine("Quarter returned\n");
            gumballMachine.setState(gumballMachine.getNoQuarterState());
        }

        public void TurnCrank()
        {
            Console.WriteLine("You turned...\n");
            gumballMachine.setState(gumballMachine.getsoldState());
        }

        public void Dispense()
        {
            //gumballMachine.ReleaseBall(); //not sure
            Console.WriteLine("No gumball dispensed\n");
        }

        public override string ToString() { return "waiting for turn of crank"; }
    }

    public class StateExample
    {
        public StateExample()
        {
            GumballMachine gbm = new GumballMachine(3);

            Console.WriteLine(gbm);
            gbm.InsertQuarter(); gbm.TurnCrank();

            Console.WriteLine(gbm);
            gbm.TurnCrank();

            Console.WriteLine(gbm);
            gbm.InsertQuarter(); gbm.TurnCrank();

            Console.WriteLine(gbm);
            gbm.InsertQuarter(); gbm.EjectQuarter(); gbm.TurnCrank();

            Console.WriteLine(gbm);
            gbm.InsertQuarter(); gbm.TurnCrank();

            Console.WriteLine(gbm);
            gbm.InsertQuarter(); gbm.TurnCrank();

            /*
                Mighty Gumball, Inc.
                C# Enabled Standing Gumball Model #2005
                Inventory: 3 gumballs
                Machine is waiting for quarter
                You inserted a quarter

                You turned...

                A gumball comes rolling out the slot...

                Mighty Gumball, Inc.
                C# Enabled Standing Gumball Model #2005
                Inventory: 2 gumballs
                Machine is waiting for quarter
                You turned but there's no quarter

                You need to pay first

                Mighty Gumball, Inc.
                C# Enabled Standing Gumball Model #2005
                Inventory: 2 gumballs
                Machine is waiting for quarter
                You inserted a quarter

                You turned...

                A gumball comes rolling out the slot...

                Mighty Gumball, Inc.
                C# Enabled Standing Gumball Model #2005
                Inventory: 1 gumball
                Machine is waiting for quarter
                You inserted a quarter

                Quarter returned

                You turned but there's no quarter

                You need to pay first

                Mighty Gumball, Inc.
                C# Enabled Standing Gumball Model #2005
                Inventory: 1 gumball
                Machine is waiting for quarter
                You inserted a quarter

                You turned...

                A gumball comes rolling out the slot...

                Oops, out of gumballs!

                Mighty Gumball, Inc.
                C# Enabled Standing Gumball Model #2005
                Inventory: 0 gumballs
                Machine is sold out
                You can't insert a quarter, the machine is sold out

                Sorry, you already turned the crank

                A gumball comes rolling out the slot...

                You turned, but there are no gumballs       
             */

            string z = Console.ReadLine();
        } //Main

    }//Program

} //namespace Console_practice