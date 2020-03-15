using System;

namespace WpfPlayground.DesignPatterns.MVCPatterns.MVC01
{
    public class DJView : BeatObserver, BPMObserver
    {
        IBeatModelInterface model;
        IControllerInterface controller;
        string beatBarLabel; string bpmOutputLabel; //Mocking winform labels

        public DJView(IControllerInterface c, IBeatModelInterface m)
        {
            this.controller = c; this.model = m;
            model.registerObs((BeatObserver)this); 
            model.registerObs((BPMObserver)this);
        }

        public void createView() { /*creates swing components*/}
        public void createControls() { /*creates swing controls*/}

        public void updateBPM()
        {
            //update is called when a statechange happens in model
            //When this happens, we update display with current bpm
            //we can get this value by requesting it firectly from the model
            int bpm=model.getBPM();
            if (bpm == 0) 
            { 
                bpmOutputLabel = "offline";
                Console.WriteLine(bpmOutputLabel);
            }
            else {
                bpmOutputLabel = "current BPM: " + bpm;
                Console.WriteLine(bpmOutputLabel);
            }
        }

        public void updateBeat() 
        {
            beatBarLabel = "setting val to 100";
            Console.WriteLine(beatBarLabel);
        }
    }

}
