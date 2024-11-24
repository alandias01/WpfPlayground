using System;
using System.Collections;

namespace WpfPlayground.DesignPatterns.Behavioral.ObserverPatterns
{
    interface ISubject { void regObs(IObserver o); void notifyObs(); }
    interface IObserver { void update(int a); }

    class WeatherData : ISubject
    {
        int a = 5;
        ArrayList observers;
        public WeatherData() { observers = new ArrayList(); }

        public void regObs(IObserver o) { observers.Add(o); }

        public void notifyObs()
        { foreach (IObserver obs in observers) { obs.update(a); } }
    }

    class DSP1 : IObserver
    {
        int a; WeatherData wd;
        public DSP1(WeatherData wd) { this.wd = wd; wd.regObs(this); }
        public void update(int a) { this.a = a; display(); }
        public void display() { Console.WriteLine("DSP1 " + a); }
    }

    class DSP2 : IObserver
    {
        int a; WeatherData wd;
        public DSP2(WeatherData wd) { this.wd = wd; wd.regObs(this); }
        public void update(int a) { this.a = a; display(); }
        public void display() { Console.WriteLine("DSP2 " + (a + 2)); }
    }

    public class ObserverSimple
    {
        public ObserverSimple()
        {
            WeatherData wd = new WeatherData();
            DSP1 d1 = new DSP1(wd);
            DSP2 d2 = new DSP2(wd);
            wd.notifyObs();
        }
    }
}