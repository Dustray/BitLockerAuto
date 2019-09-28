using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitLockerUI.Utils
{
    class AppConfigOperation
    {
        public string Read(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        public void write(string key,string value)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings[key].Value = value;
            cfa.Save();
        }
    }
}
