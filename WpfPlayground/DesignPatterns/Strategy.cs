using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPlayground.DesignPatterns
{
    public class Strategy
    {
    }

    public interface ICar
    {
        string Start();
        string Stop();
    }

    public interface IEngine
    {
        string StartEngine();
        string StopEngine();
    }

    public class GenericCar : ICar
    {        
        public GenericCar()
        {

        }

        public string Start()
        {
            throw new NotImplementedException();
        }

        public string Stop()
        {
            throw new NotImplementedException();
        }
    }
}
