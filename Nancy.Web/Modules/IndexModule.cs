namespace Nancy.Web.Modules
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = _ => View["index"];
        }
    }
}