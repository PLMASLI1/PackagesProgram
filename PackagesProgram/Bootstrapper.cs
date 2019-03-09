using System.Windows;
using Caliburn.Micro;
using PackagesProgram.ViewModels;

namespace PackagesProgram
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<PackagesViewModel>();
            
        }
    }
}