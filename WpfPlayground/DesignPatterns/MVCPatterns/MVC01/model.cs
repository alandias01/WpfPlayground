using System.Collections.Generic;

namespace WpfPlayground.DesignPatterns.MVCPatterns.MVC01
{
    public interface BeatObserver { void updateBeat();}
    public interface BPMObserver { void updateBPM();}
    public class Sequencer
    {
        public void on(){} public void off(){}
        public void setTempoInBPM(int a){}
    }

    public interface IBeatModelInterface
    {
        void initialize(); 
        void on(); void off(); 
        void setBPM(int bpm); int getBPM();
        void registerObs(BeatObserver o); 
        void registerObs(BPMObserver o);
    }

    public class BeatModel : IBeatModelInterface
    {
        Sequencer sequencer=new Sequencer(); int bpm = 90;
        List<BeatObserver> beatObservers = new List<BeatObserver>();
        List<BPMObserver> bpmObservers = new List<BPMObserver>();

        public void initialize() { }

        public void on() { sequencer.on(); setBPM(90); }

        public void off() { setBPM(0); sequencer.off(); }

        public void setBPM(int bpm)
        { 
            this.bpm = bpm; sequencer.setTempoInBPM(getBPM());
            notifyBPMObservers();
        }

        public int getBPM() { return bpm; }

        public void beatEvent() { notifyBeatObservers(); }

        public void registerObs(BeatObserver o) { beatObservers.Add(o); }
        public void registerObs(BPMObserver o) { bpmObservers.Add(o); }
        
        public void notifyBPMObservers() 
        {
            foreach (BPMObserver obs in bpmObservers) { obs.updateBPM();}

        }
        public void notifyBeatObservers()
        {
            foreach(BeatObserver obs in beatObservers){obs.updateBeat();}
        }
    }


}
