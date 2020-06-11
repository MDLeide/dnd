using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DND
{
    public static class Resources
    {
        const string InstallCommandsResourceName = "install.txt";

        static string _installCommands;

        public static string InstallCommands => _installCommands ?? (_installCommands = GetInstallCommands());

        static string GetInstallCommands() =>
            GetResourceAsString(InstallCommandsResourceName);
        

        public static string GetResourceAsString(string resourceName)
        {
            using (Stream stream = GetResourceStream(resourceName))
            using (StreamReader sr = new StreamReader(stream))
                return sr.ReadToEnd();
        }

        public static Stream GetResourceStream(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = GetResourceName(assembly, resourceName);
            return assembly.GetManifestResourceStream(resourcePath);
        }

        static string GetResourceName(Assembly assembly, string input)
        {
            return assembly.GetManifestResourceNames()
                       .FirstOrDefault(p => p == input) ??
                   assembly.GetManifestResourceNames()
                       .FirstOrDefault(p => p.EndsWith(input));
        }
    }
}
