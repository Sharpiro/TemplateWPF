using TemplateWPF.Windows;
using System.Windows;
using System.Configuration;
using Ninject;
using System;
using NLog;
using TemplateWpf.Models;

namespace TemplateWPF
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            ILogger logger = null;
            MessageBoxFacade messageBoxFacade = null;
            try
            {
                logger = LogManager.GetLogger("AppLogger");
                messageBoxFacade = new MessageBoxFacade(logger);
                var config = ReadConfiguration();
                var kernel = new StandardKernel(new InjectionModule(config));
                var window = kernel.Get<MainWindow>();
                window.Show();
                await window.Initialize();
                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                messageBoxFacade?.ShowError("Fatal Error, check logs");
                messageBoxFacade?.ShowError(ex);
                Shutdown();
            }
        }

        private ConfigurationModel ReadConfiguration()
        {
            var configModel = new ConfigurationModel();
            configModel.UserName = ConfigurationManager.AppSettings["UserName"];
            configModel.Password = ConfigurationManager.AppSettings["Password"];

            if (string.IsNullOrEmpty(configModel.UserName)) throw new ArgumentNullException(nameof(configModel.UserName));
            if (string.IsNullOrEmpty(configModel.Password)) throw new ArgumentNullException(nameof(configModel.Password));

            return configModel;
        }
    }

    public class EntryPoint
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                // ...
            }
            else
            {
                var app = new App();
                app.InitializeComponent();
                app.Run();
            }
        }
    }
}