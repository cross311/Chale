using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/index"] = parameters => 
                {
                    return View["index"];
                };
        }
    }
}