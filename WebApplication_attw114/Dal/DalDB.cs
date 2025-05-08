using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebApplication_attw114.Dal
{
    public class DalDB
    {

        public string getDBConnectStr_Certificate()
        {
            var configuration = GetConfiguration();
            return configuration.GetSection("ConnectionStrings").GetSection("DefaultConnectionSQl").Value.ToString();
        }

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(System.IO.Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}
