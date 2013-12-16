using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Web
{
    public class RootPathProvider : Nancy.IRootPathProvider
    {
        public string GetRootPath()
        {
#if DEBUG
            // Use the project directory in `DEBUG` mode so i can edit my resources in VS instead of some other gymnastics
            string debugRootPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../views");

            if (Directory.Exists(debugRootPath))
            {
                return debugRootPath;
            }
#endif
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "views");
        }
    }
}
