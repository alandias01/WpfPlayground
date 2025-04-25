using System;
using System.Collections.Generic;

namespace WpfPlayground.DesignPatterns.Behavioral.ObserverPatterns
{
    public interface Subject
    {
        void registerObserver(IObserver o);
        void removeObserver(IObserver o);
        void notifyObservers();
    }

    public interface IObserver
    {
        void update(float temp, float humidity, float pressure);
    }

    public interface DisplayElement
    {
        void display();
    }

    public class WeatherData : Subject
    {
        private List<IObserver> observers;
        private float temperature, humidity, pressure;

        public WeatherData() { observers = new List<IObserver>(); }

        public void registerObserver(IObserver o) { observers.Add(o); }

        public void removeObserver(IObserver o) { observers.Remove(o); }

        public void notifyObservers()
        {
            for (int i = 0; i < observers.Count; i++)
                observers[i].update(temperature, humidity, pressure);
        }

        public void measurementsChanged() { notifyObservers(); }

        public void setMeasurements(float temperature, float humidity, float pressure)
        {
            this.temperature = temperature;
            this.humidity = humidity;
            this.pressure = pressure;
            measurementsChanged();
        }
    }

    public class CurrentConditionsDisplay : IObserver, DisplayElement
    {
        private float temperature, humidity;
        private Subject weatherData;

        public CurrentConditionsDisplay(Subject weatherData)
        {
            this.weatherData = weatherData;
            weatherData.registerObserver(this);
        }

        public void update(float temperature, float humidity, float pressure)
        {
            this.temperature = temperature;
            this.humidity = humidity; display();
        }

        public void display() { Console.WriteLine("Temp: " + temperature + "\nHumidity: " + humidity); }

        public void update(int a)
        {
            throw new NotImplementedException();
        }
    }

    public class Observer01
    {
        public Observer01()
        {
            WeatherData weatherData = new WeatherData();
            CurrentConditionsDisplay currentDisplay = new CurrentConditionsDisplay(weatherData);
            weatherData.setMeasurements(80, 65, 30.4f);
        }
    }
}