using System;

namespace WpfPlayground.DesignPatterns.Structural
{
    //Facade just consolidates everything into one call to simplify things

    public class Amplifier { public void on() { Console.WriteLine("amp on"); } }
    public class CdPlayer { public void on() { Console.WriteLine("cd player on"); } }
    public class DvdPlayer
    {
        public void play(string movie)
        { Console.WriteLine("Watching " + movie); }
    }

    public class HomeTheaterFacade
    {
        Amplifier amp;
        DvdPlayer dvd;
        CdPlayer cd;

        public HomeTheaterFacade(Amplifier amp, DvdPlayer dvd, CdPlayer cd)
        { this.amp = amp; this.dvd = dvd; this.cd = cd; }

        public void watchMovie(string movie)
        { amp.on(); cd.on(); dvd.play(movie); }
    }

    public class Facade
    {
        public Facade()
        {
            Amplifier amp = new Amplifier();
            DvdPlayer dvd = new DvdPlayer();
            CdPlayer cd = new CdPlayer();
            HomeTheaterFacade htf = new HomeTheaterFacade(amp, dvd, cd);
            htf.watchMovie("The Ring");
        }
    }
}