using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPlayground.DesignPatterns.Behavioral
{
    /* Strategy Pattern Google AI 20251005
     * 
     * The context delegates its behavior/operation to the interchangeable strategy (IStrategy)
     * Strategy represents different behaviors, which are encapsulated in separate classes (that are interchangeable)
     * The context uses the strategy to perform its operations
     * Context doesn't care how strategy works, it just delegates
     * You can add/change strategies without modifying the context
     * 
     * The Strategy pattern lets the behavior vary independently from the context that use it.     * 
     * 
     *           Context: The class that uses the strategy.  It holds a reference to the IStrategy
     *         IStrategy: The interface or abstract class defining the common operation for all concrete strategies
     * Concrete Strategy: Implementations of the IStrategy interface, each providing a specific algorithm.
     * 
     * Imagine a PaymentService (Context) that can process payments using different methods like CreditCardPayment, 
     * PayPalPayment, or BankTransferPayment (Concrete Strategies), all implementing an IPaymentStrategy interface.
     */

    // IStrategy
    public interface IPaymentStrategy
    {
        void ProcessPayment(decimal amount);
    }

    // Concrete Strategy 1
    public class CreaditCardPayment : IPaymentStrategy
    {
        public void ProcessPayment(decimal amount) => Console.WriteLine($"Processing credit card payment amount {amount:C}");
    }

    // Concrete Strategy 2
    public class PayPalPayment : IPaymentStrategy
    {
        public void ProcessPayment(decimal amount) => Console.WriteLine($"Processing PayPal payment of {amount:C}");
    }

    // Context
    public class PaymentService
    {
        IPaymentStrategy _paymentStrategy;

        public PaymentService(IPaymentStrategy paymentStrategy)
        {
            _paymentStrategy = paymentStrategy;
        }

        public void ExecutePayment(decimal amount)
        {
            _paymentStrategy.ProcessPayment(amount);
        }
    }
}
