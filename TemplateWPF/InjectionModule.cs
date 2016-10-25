using Ninject.Modules;
using NLog;
using TemplateWpf.Models;

namespace TemplateWPF
{
    public class InjectionModule : NinjectModule
    {
        private readonly ConfigurationModel _config;

        public InjectionModule(ConfigurationModel config)
        {
            _config = config;
        }
        public override void Load()
        {
            Bind<MainViewModel>().ToSelf().InSingletonScope();
            Bind<ConfigurationModel>().ToConstant(_config);
            Bind<ILogger>().ToMethod(p => LogManager.GetLogger(p?.Request.Target?.Member.DeclaringType?.Name ?? "TemplateLogger"));
        }
    }
}