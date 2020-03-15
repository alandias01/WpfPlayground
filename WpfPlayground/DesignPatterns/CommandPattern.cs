using System; 

namespace WpfPlayground.DesignPatterns
{
    public class CommandPattern
    {
        public CommandPattern()
        {
            Remote remote = new Remote();
            Light livingroomlight = new Light("livingroom");
            LightOnCommand livingroomlighton = new LightOnCommand(livingroomlight);
            LightOffCommand livingroomlightoff = new LightOffCommand(livingroomlight);

            remote.SetCommand(0, livingroomlighton, livingroomlightoff);
            remote.OnButtonWasPushed(0); //livingroom light is on

            SimpleRemoteControl remote2 = new SimpleRemoteControl();
            remote2.SetCommand(livingroomlighton);
            remote2.ButtonWasPressed(); //livingroom light is on
        }
    }

    public interface Command { void Execute(); }

    public class Light
    {
        string location;
        public Light(string location) { this.location = location; }
        public void On() { Console.WriteLine(location + " light is on"); }
        public void Off() { Console.WriteLine(location + " light is off"); }
    }

    public class LightOnCommand : Command
    {
        Light light;
        public LightOnCommand(Light light) { this.light = light; }
        public void Execute() { light.On(); }
    }

    public class LightOffCommand : Command
    {
        Light light;
        public LightOffCommand(Light light) { this.light = light; }
        public void Execute() { light.Off(); }
    }

    public class NoCommand : Command
    { public NoCommand() { } public void Execute() { } }

    public class SimpleRemoteControl
    {
        Command slot;
        public SimpleRemoteControl() { }
        public void SetCommand(Command command) { slot = command; }
        public void ButtonWasPressed() { slot.Execute(); }
    }

    public class Remote
    {
        Command[] onCommands;
        Command[] offCommands;

        public Remote()
        {
            onCommands = new Command[7];
            offCommands = new Command[7];
            Command noCommand = new NoCommand();
            for (int i = 0; i < 7; i++)
            {
                onCommands[i] = noCommand;
                offCommands[i] = noCommand;
            }
        }

        public void SetCommand(int slot, Command onCommand, Command offCommand)
        {
            onCommands[slot] = onCommand;
            offCommands[slot] = offCommand;
        }

        public void OnButtonWasPushed(int slot) { onCommands[slot].Execute(); }
        public void OffButtonWasPushed(int slot) { offCommands[slot].Execute(); }
    }
}