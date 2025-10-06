using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPlayground.DesignPatterns.Creational.FactoryPatterns;

namespace WpfPlayground.DesignPatterns.Creational.AbstractFactoryPatterns
{
    /* Gemini
     * To provide an interface for creating families of related or dependent objects without specifying their concrete classes.
     * 
     * Structure
     * Abstract Factory: An interface or abstract class declaring methods for creating abstract products.
     * Concrete Factory: Implementations of the Abstract Factory interface, each responsible for creating a family of concrete products.
     * Abstract Product: An interface or abstract class for a type of product within the family.
     * Concrete Product: Implementations of the Abstract Product interface, created by the Concrete Factory.
     * Abstract Factory focuses on object creation and provides a way to create families of related objects without exposing their concrete classes.
     * 
     * copilot
     * Create families of related objects (payment processors, receipts, notifications) that should
     * work together without the client knowing the concrete classes
     * 
     * Why this is Abstract Factory
     * Families of related objects: Each factory produces a processor + receipt that belong together.
     * Client isolation: PaymentService doesn't know or care whether it's PayPal or Credit Card — it just uses the abstract interfaces.
     * Extensibility: Adding a new payment method (e.g., CryptoPaymentFactory) requires no changes to existing client code.
     * 
     */
    public class AbstractFactory_PaymentService
    {
        public AbstractFactory_PaymentService()
        {
            // Use Credit Card factory
            var creditCardService = new PaymentService(new CreditCardPaymentFactory());
            creditCardService.MakePayment(100.00m);

            Console.WriteLine();

            // Use PayPal factory
            var paypalService = new PaymentService(new PayPalPaymentFactory());
            paypalService.MakePayment(50.00m);
        }
    }

    //Interfaces for the objects your factories will create

    //Abstract product A
    public interface IPaymentProcessor
    {
        void ProcessPayment(decimal amount);
    }

    //Abstract product B
    public interface IReceipt
    {
        void GenerateReceipt(decimal amount);
    }

    // Define Abstract factory
    // Declares methods for creating each product type
    public interface IPaymentFactory
    {
        IPaymentProcessor CreatePaymentProcessor();
        IReceipt CreateReceipt();
    }

    // Concrete products

    // Each payment method (Creadit card, paypal) has its own payment processor and receipt

    // Credit Card family
    public class CreditCardProcessor : IPaymentProcessor
    {
        public void ProcessPayment(decimal amount) { Console.WriteLine($"Processing credit card payment of {amount:C}"); }
    }

    public class CreditCardReceipt : IReceipt
    {
        public void GenerateReceipt(decimal amount) { Console.WriteLine($"Credit Card receipt for {amount:C}"); }
    }

    // PayPal family
    public class PayPalProcessor : IPaymentProcessor
    {
        public void ProcessPayment(decimal amount) { Console.WriteLine($"Processing PayPal payment of {amount:C}"); }
    }

    public class PayPalReceipt : IReceipt
    {
        public void GenerateReceipt(decimal amount) { Console.WriteLine($"PayPal receipt for {amount:C}"); }
    }

    // Concrete Factories
    // Each factory knows how to create the correct family of objects.

    public class CreditCardPaymentFactory : IPaymentFactory
    {
        public IPaymentProcessor CreatePaymentProcessor() => new CreditCardProcessor();
        public IReceipt CreateReceipt() => new CreditCardReceipt();
    }

    public class PayPalPaymentFactory : IPaymentFactory
    {
        public IPaymentProcessor CreatePaymentProcessor() => new PayPalProcessor();
        public IReceipt CreateReceipt() => new PayPalReceipt();
    }

    // Client Code (Context)
    // The client uses the abstract factory, not the concrete classes.

    public class PaymentService
    {
        private readonly IPaymentProcessor _processor;
        private readonly IReceipt _receipt;

        public PaymentService(IPaymentFactory factory)
        {
            _processor = factory.CreatePaymentProcessor();
            _receipt = factory.CreateReceipt();
        }

        public void MakePayment(decimal amount)
        {
            _processor.ProcessPayment(amount);
            _receipt.GenerateReceipt(amount);
        }
    }



    //Example 2

    // Abstract Product A
    public interface IButton
    {
        void Render();
    }

    // Abstract Product B
    public interface ICheckbox
    {
        void Paint();
    }

    // Concrete Product A1
    public class WindowsButton : IButton
    {
        public void Render() { Console.WriteLine("Rendering a Windows button."); }
    }

    // Concrete Product B1
    public class WindowsCheckbox : ICheckbox
    {
        public void Paint() { Console.WriteLine("Windows Paint"); }
    }

    // Concrete Product A2
    public class MacOSButton : IButton
    {
        public void Render() { Console.WriteLine("Rendering a macOS button."); }
    }

    // Concrete Product B2
    public class MacOSCheckbox : ICheckbox
    {
        public void Paint() { Console.WriteLine("Mac Paint"); }
    }

    // Abstract Factory
    public interface IUIFactory
    {
        IButton CreateButton();
        ICheckbox CreateCheckbox();
    }

    // Concrete Factory 1
    public class WindowsUIFactory : IUIFactory
    {
        public IButton CreateButton() => new WindowsButton();
        public ICheckbox CreateCheckbox() => new WindowsCheckbox();
    }

    // Concrete Factory 2
    public class MacOSUIFactory : IUIFactory
    {
        public IButton CreateButton() => new MacOSButton();
        public ICheckbox CreateCheckbox() => new MacOSCheckbox();
    }

    public class UIClientApp
    {
        private readonly IButton _button;
        private readonly ICheckbox _checkbox;

        public UIClientApp(IUIFactory factory)
        {
            _button = factory.CreateButton();
            _checkbox = factory.CreateCheckbox();
        }

        public void Start()
        {
            _button.Render();
            _checkbox.Paint();
        }
    }

    public class AbstractFactory_Ex2Usage
    {
        IUIFactory _factory;

        public AbstractFactory_Ex2Usage()
        {
            string os = "windows";
            _factory = os == "windows" ? new WindowsUIFactory() : new MacOSUIFactory();

            new UIClientApp(_factory).Start();
        }
    }

    /*
                            +-------------------+
                            |    IUIFactory     |  <<Abstract Factory>>
                            |-------------------|
                            | + CreateButton()  |
                            | + CreateCheckbox()|
                            +---------+---------+
                                      |
                    -----------------------------------------
                    |                                       |
            +---------------------+              +---------------------+
            |  WindowsUIFactory   |              |   MacOSUIFactory    |
            |---------------------|              |---------------------|
            | + CreateButton()    |              | + CreateButton()    |
            | + CreateCheckbox()  |              | + CreateCheckbox()  |
            +----------+----------+              +----------+----------+
                       |                                    |
               ------------------                   ------------------
               |                |                   |                |
            +----------------+  +----------------+  +----------------+  +----------------+
            | WindowsButton  |  | WindowsCheckbox|  | MacOSButton    |  | MacOSCheckbox  |
            |----------------|  |----------------|  |----------------|  |----------------|
            | + Render()     |  | + Render()     |  | + Render()     |  | + Render()     |
            +----------------+  +----------------+  +----------------+  +----------------+
                   ^                        ^               ^                     ^
                   |                        |               |                     |
                   |                        |               |                     |
            +----------------+       +----------------+  +----------------+  +----------------+
            |  IButtonControl|       |ICheckboxControl|  |  IButtonControl|  |ICheckboxControl|
            |----------------|       |----------------|  |----------------|  |----------------|
            | + Render()     |       | + Render()     |  | + Render()     |  | + Render()     |
            +----------------+       +----------------+  +----------------+  +----------------+
            
            
     */
}
