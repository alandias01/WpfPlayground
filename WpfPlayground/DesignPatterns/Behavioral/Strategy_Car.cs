using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPlayground.DesignPatterns.Behavioral
{

    /* Strategy definition in Strategy_PaymentService
     * ICar delegates its behavior (starting/stopping) to an interchangeable IEngine
     * IEngine is the strategy interface (different engine behaviors)
     * GenericCar is the context and uses IEngine strategy to perform its operations
     * This allows us to change engine without changing the cars code
     * 
     * The behavior (engine type) is encapsulated in separate classes
     * The car doesnt care how the engine works, it just delegates
     * You can add new engines without modifying GenericCar
     */

    public class Strategy_Car
    {
        public Strategy_Car()
        {
            ICar car = new GenericCar(new ElectricEngine());
            car.Start();
            car.ChangeEngine(new GasEngine());
        }
    }    

    // Strategy
    public interface IEngine
    {
        void StartEngine();
        void StopEngine();
    }

    // Concrete strategy 1
    public class GasEngine : IEngine
    {
        public void StartEngine() { Console.WriteLine("Gas engine roars to life!"); }
        public void StopEngine() { Console.WriteLine("Gas engine shuts down."); }
    }

    // Concrete strategy 2
    public class ElectricEngine : IEngine
    {
        public void StartEngine() { Console.WriteLine("Electric engine hums quietly."); }
        public void StopEngine() { Console.WriteLine("Electric engine powers off."); }
    }

    public interface ICar
    {
        void Start();
        void Stop();
        void ChangeEngine(IEngine newEngine);
    }

    // Context
    public class GenericCar : ICar
    {
        private IEngine _engine;

        public GenericCar(IEngine engine)
        {
            _engine = engine;
        }

        public void Start()
        {
            _engine.StartEngine();
        }

        public void Stop()
        {
            _engine.StopEngine();
        }

        // Optional: allow swapping strategies at runtime
        public void ChangeEngine(IEngine newEngine)
        {
            _engine = newEngine;
        }
    }
}
