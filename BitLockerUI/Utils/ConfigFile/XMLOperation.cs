using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BitLockerUI.Utils.ConfigFile
{
    public class XMLOperation
    {
        private const string path= "userConfig.xml";
        private XmlDocument _xml;
        public XMLOperation()
        {
            _xml = new XmlDocument();
        }
        //<root>
        //    <user>
        //        <currentdrivename>Windows 10</currentdrivename>
        //        <lastdrivenum>D:\</lastdrivenum>
        //        <scretpassword>shafihweafghdsagasjkhfsda==</scretpassword>
        //        <unlockpassword>fdsahgfjghfdhgfdhgfdsh==</unlockpassword>
        //    </user>    
        //</root>
        public void CreateNewFile()
        {
            var rootNode = _xml.CreateElement("root");
            
            var userNode = _xml.CreateElement("user");
            rootNode.AppendChild(userNode);

            var currentdrivenameNode = _xml.CreateElement("currentdrivename");
            var lastdrivenumNode = _xml.CreateElement("lastdrivenum");
            var scretpasswordNode = _xml.CreateElement("scretpassword");
            var unlockpasswordNode = _xml.CreateElement("unlockpassword");
            userNode.AppendChild(currentdrivenameNode);
            userNode.AppendChild(lastdrivenumNode);
            userNode.AppendChild(scretpasswordNode);
            userNode.AppendChild(unlockpasswordNode);
            _xml.Save(path);
        }

    }
}
