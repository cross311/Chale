namespace Web.Modules
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get[Href.Root] = _ => View["index"];
        }
    }
}