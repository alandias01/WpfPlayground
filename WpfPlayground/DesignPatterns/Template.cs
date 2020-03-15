using System;

namespace WpfPlayground.DesignPatterns
{
    //Original problem
    public class CoffeeOld
    {
        private void boilWater() { }
        private void brewCoffeeGrinds() { }
        private void pourInCup() { }
        private void addSugerAndMilk() { }

        public void prepareRecipe() 
        { boilWater(); brewCoffeeGrinds(); pourInCup(); addSugerAndMilk(); }
    }

    public class TeaOld
    {
        private void boilWater() { }
        private void steepTeaBag() { }
        private void pourInCup() { }
        private void addLemon() { }

        public void prepareRecipe() 
        { boilWater(); steepTeaBag(); pourInCup(); addLemon(); }
    }

    //boil, pourincup | Here we have 2 methods in both that are similar
    //addLemon, addSugar| 2 different methods which relate to adding a condiment
    //steepTeaBag, brewCoffeeGrinds| They are in same category of brewing

    //Revised Version
    public abstract class CaffeineBeverage
    {
        public void PrepareRecipe()
        { boilWater(); brew(); pourInCup(); addCondiment(); }

        public abstract void brew();
        public abstract void addCondiment();
        public void boilWater() { }
        public void pourInCup() { }

        //Optional Hook version
        public void PrepareRecipeWithHook(bool ans)
        {
            boilWater(); brew(); pourInCup(); 
            if ( CustomerWantsCondiments(ans) ) { addCondiment(); } 
        }

        public virtual bool CustomerWantsCondiments(bool ans) { return true; }//Hook
    }

    public class CoffeeNew : CaffeineBeverage
    {
        public override void brew() {/*Dripping Coffee */ }
        public override void addCondiment() { /*Sugar and Milk*/ }
    }

    public class TeaNew : CaffeineBeverage
    {
        public override void brew() {/*Steeping Tea*/}
        public override void addCondiment() {/*Adding Milk*/ }
    }

    public class CoffeeNewWithHook : CaffeineBeverage
    {
        public override void brew() {/*Dripping Coffee */ }
        public override void addCondiment() { Console.WriteLine("added"); /*Sugar and Milk*/ }
        public override bool CustomerWantsCondiments(bool ans)
        { if (ans) { return true;} else {return false;} } //Option to override hook. We can ask if user wants milk         
    }

    //We generalized the prepareRecipe method
    //boiling and pouring were abstracted
    //We have combined Brewing into one function with different implementation per class
    //Hook
    //We have hook in abstrat class and let the lower level classes override
    //Hook gives subclass option to get involved in the steps of the template
    //pg 295 has more details for Hooks purpose
    
    public class Template
    {
        public Template()
        {
            TeaOld oldTea = new TeaOld(); oldTea.prepareRecipe();
            CoffeeNew coffee = new CoffeeNew(); coffee.PrepareRecipe();

            CoffeeNewWithHook coffeeHook = new CoffeeNewWithHook();
            coffeeHook.PrepareRecipeWithHook(true); //added
        }
    }
}