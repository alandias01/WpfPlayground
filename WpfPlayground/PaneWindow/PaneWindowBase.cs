using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace WpfPlayground.PaneWindow
{    
    public class PaneWindowBase : Window
    {
        private IPaneWindowBaseViewModel _viewmodel;
        public PaneWindowBase(IPaneWindowBaseViewModel viewmodel)
        {
            try
            {
                this.DataContext = viewmodel;
                this._viewmodel = viewmodel;
                viewmodel.BringIntoView += this.Viewmodel_BringIntoView;
                this.Loaded += this.PaneWindowBase_Loaded;
            }
            catch (Exception e)
            {                
            }
        }

        private void PaneWindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this._viewmodel.Loaded();
            }
            catch (Exception ex)
            {
            }            
        }

        private void Viewmodel_BringIntoView(object sender, EventArgs e)
        {
            try
            {
                this.BringIntoView();
            }
            catch (Exception ex)
            {
            }
        }
        
        protected override void OnClosed(EventArgs e)
        {
            this._viewmodel.Closed();
            base.OnClosed(e);
        }
    }

    public interface IPaneWindowBaseViewModel
    {
        void Closed();
        void Loaded();
        event EventHandler BringIntoView;
    }

    public class PaneWindowBaseViewModel : IPaneWindowBaseViewModel
    {
        public event EventHandler BringIntoView;

        public virtual void Closed()
        {            
        }

        public virtual void Loaded()
        {
            throw new NotImplementedException();
        }

        public void BringWindowIntoView()
        {
            this.BringIntoView?.Invoke(this, new EventArgs());
        }        
    }
}
