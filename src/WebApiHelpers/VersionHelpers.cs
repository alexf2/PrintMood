using System.Linq;
using System.Reflection;

namespace WebApiHelpers
{
    public class VersionHelpers
    {
        public static string GetProductVersion(Assembly ass)
        {
            var attribute = (AssemblyInformationalVersionAttribute)(ass ?? typeof(VersionHelpers).GetTypeInfo().Assembly)
              .GetCustomAttributes<AssemblyInformationalVersionAttribute>()
              .Single();

            return attribute.InformationalVersion;            
        }
    }
}
