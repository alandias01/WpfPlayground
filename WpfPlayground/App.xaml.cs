using Prism.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfPlayground.DependencyInjection;
using Unity.Lifetime;

namespace WpfPlayground
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // When overriding Application methods like OnStartUp() which raise its corresponding event, like startup event
            //its customary to call their method like base.OnStartUp first, then add your code            
            base.OnStartup(e);

            /*  This is what happens in the background
            Application app=new application()
            Windwo1 w=new window()
            app.run(w)
            */

            //This checks for arguments
            if (e.Args.Length > 0)
            {
                foreach (var v in e.Args)
                {
                }
            }

            new AnimationShapesEtc.Transform01().Show();

            //wpfnewtechniques has startup code

            //Setup unity, then use the event aggregator
            //UnityManager.container.RegisterType<IEventAggregator, EventAggregator>();

            //you can get access to the current application instance from anywhere
            //using application.current

            //Here we can shut down the application but this just returns Application.Run()
            //Or you can set in xaml  ShutdownMode="OnMainWindowClose"
            //If you handled the Application.exit event, that will run after Run()
            //Application.Current.Shutdown();

            //this gets you access to the main window but the main window is custom
            //Window w = Application.Current.MainWindow;

            //Its called public partial class MainWindow : Window so
            //to get access to everything in your mainwindow
            //MainWindow mw = (MainWindow)Application.Current.MainWindow;

            //If you want to communicate btwn windows
            //You can create a list here and access from other windows
            //((App)Application.Current).DTCMessages.Add("MSG01");

        }
    }
}
