using System;
using System.Collections.Generic;
using System.IO;
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
        /// <summary>
        /// 创建新的文件
        /// </summary>
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
        /// <summary>
        /// 获取指定节点数据
        /// </summary>
        /// <param name="nodeLevel"></param>
        /// <returns></returns>
        public string GetNodeValue(string nodeLevel)
        {
            //string result = "";
            XmlNode root = _xml.SelectSingleNode(nodeLevel);//   /root/user
            //XmlNodeList childlist = root.ChildNodes;
            return root.InnerText;
        }

        /// <summary>
        /// 设置指定节点数据
        /// </summary>
        /// <param name="nodeLevel"></param>
        /// <param name="value"></param>
        public void SetNodeValue(string nodeLevel,string value)
        {
            if (!File.Exists(path))
            {
                CreateNewFile();
            }
            XmlNode root = _xml.SelectSingleNode(nodeLevel);//   /root/user
            //XmlNodeList childlist = root.ChildNodes;
            root.InnerText=value;
        }

    }
}
