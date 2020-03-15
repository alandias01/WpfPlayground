using System;

namespace WpfPlayground.DesignPatterns.MVCPatterns.MVC01
{
    public class mvc
    {
        public mvc()
        {
            IBeatModelInterface model = new BeatModel();
            IControllerInterface controller = new BeatController(model);
            controller.increaeBPM(); //current BPM: 91
        }
    }
}