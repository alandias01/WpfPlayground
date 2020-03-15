
namespace WpfPlayground.DesignPatterns.MVCPatterns.MVC01
{
    public interface IControllerInterface 
    {
        void start(); void stop();
        void increaeBPM(); void decreaseBPM(); void setBPM(int bpm);
    }

    public class BeatController : IControllerInterface
    {
        IBeatModelInterface model; DJView view;

        public BeatController(IBeatModelInterface model)
        {
            this.model = model;
            view = new DJView(this, model);

            //the controller is passed the model and creates the view
            view.createView();
            view.createControls();
            model.initialize();        
        }

        public void start() { model.on(); }
        public void stop() { model.off(); }

        public void increaeBPM()
        { int bpm = model.getBPM(); model.setBPM(bpm + 1); }

        public void decreaseBPM()
        { int bpm = model.getBPM(); model.setBPM(bpm - 1); }

        public void setBPM(int bpm)
        { model.setBPM(bpm); }        
    }



}
