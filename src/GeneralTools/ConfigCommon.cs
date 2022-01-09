using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralTools
{
    public static class ConfigCommon
    {
        public static string GetConnectionStringsConfig(string connectionName)
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = "DataBase.config";
            Configuration libConfig = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            return libConfig.AppSettings.Settings[connectionName].Value;

            //string file = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }
    }
}
