namespace Web
{
    using GameDataLayer;
    using Nancy;
    using Nancy.TinyIoc;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            // Autoregister will actually do this for us, so we don't need this line,
            // but I'll keep it here to demonstrate. By Default anything registered
            // against an interface will be a singleton instance.
            container.Register<IRepository<Tournament>, EntityFrameworkRepository<Tournament>>();
        }
    }
}