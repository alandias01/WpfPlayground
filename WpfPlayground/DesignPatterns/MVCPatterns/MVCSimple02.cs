using System;
using System.Collections.Generic;

namespace WpfPlayground.DesignPatterns.MVCPatterns.MVCPatterns02
{
    interface IObservable { void registerObserver(IObserver o); void notifyObserver();}
    interface IObserver { void update();}

    interface IBPMModel : IObservable { void setBPM(int bpm); int getBPM();}
    interface IBPMController { void increaseBPM(); void decreaseBPM(); }

    class BPMView : IObserver
    {
        IBPMModel model; IBPMController controller;
        int BPMLabel;

        public BPMView(IBPMModel model, IBPMController controller)
        {
            this.model = model; this.controller = controller;
            model.registerObserver(this);
        }

        public void createView() { /*code goes here*/ }
        public void createControls() { /*code goes here*/ }

        public void increaseBPM_click() { controller.increaseBPM(); }
        public void decreaseBPM_click() { controller.decreaseBPM(); }

        public void update()
        {
            BPMLabel = model.getBPM();
            Console.WriteLine("Current BPM: " + BPMLabel);
        }
    }

    class BPMController : IBPMController
    {
        IBPMModel model; BPMView bpmview;
        public BPMController(IBPMModel model)
        {
            this.model = model; bpmview = new BPMView(model, this);

            //we create view here
            bpmview.createView();
            bpmview.createControls();
        }

        public void increaseBPM() { model.setBPM(model.getBPM() + 1); }
        public void decreaseBPM() { model.setBPM(model.getBPM() - 1); }
    }

    class BPMModel : IBPMModel
    {
        int bpm = 90;
        List<IObserver> bpmObservers = new List<IObserver>();

        public void setBPM(int bpm) { this.bpm = bpm; notifyObserver(); }
        public int getBPM() { return this.bpm; }
        public void registerObserver(IObserver o) { bpmObservers.Add(o); }
        public void notifyObserver()
        { foreach (IObserver obs in bpmObservers) { obs.update(); } }
    }

    public class MVCSimple02
    {
        public MVCSimple02()
        {
            IBPMModel model = new BPMModel();
            IBPMController c = new BPMController(model);
            c.decreaseBPM(); //should be done by views decrease button
        }
    }
}