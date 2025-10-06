using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPlayground.DesignPatterns.Behavioral
{
    /* Strategy Pattern Google AI 20251005
     * 
     * Define a family of algorithms, encapsulate each one, and make them interchangeable
     * The Strategy pattern lets the algorithm vary independently from the clients that use it.
     * 
     *           Context: The class that uses the strategy.  It holds a reference to the IPaymentStrategy
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
