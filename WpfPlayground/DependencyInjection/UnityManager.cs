using Prism.Events;
using Unity;

namespace WpfPlayground.DependencyInjection
{
    public class UnityManager
    {
        public static IUnityContainer container = new UnityContainer();

        public UnityManager()
        {
            //IContainerRegistry c = new conta
            //New in prism 7.1 we setup a container using IContainerRgistry
            container = new UnityContainer();            
        }
    }    
}
