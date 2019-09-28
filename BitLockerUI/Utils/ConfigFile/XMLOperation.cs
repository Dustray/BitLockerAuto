using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BitLockerUI.Utils
{
    public class XMLOperation
    {
        private const string path= @"Data\userConfig.xml";
        private XmlDocument _xml;
        public XMLOperation()
        {
            _xml = new XmlDocument();
            if (File.Exists(path))
            {
                _xml.Load(path);
            }
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
            XmlDeclaration xmldecl;
            xmldecl = _xml.CreateXmlDeclaration("1.0", "utf-8", null);
            _xml.AppendChild(xmldecl);

            var rootNode = _xml.CreateElement("root");
            _xml.AppendChild(rootNode);
            var userNode = _xml.CreateElement("user");
            rootNode.AppendChild(userNode);

            var currentdrivenameNode = _xml.CreateElement("currentdrivename");
            var lastdrivenumNode = _xml.CreateElement("lastdrivenum");
            var lastfileNode = _xml.CreateElement("lastfile");
            var scretpasswordNode = _xml.CreateElement("secretpassword");
            var unlockpasswordNode = _xml.CreateElement("unlockpassword");
            userNode.AppendChild(currentdrivenameNode);
            userNode.AppendChild(lastdrivenumNode);
            userNode.AppendChild(lastfileNode);
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
            if (!File.Exists(path))
            {
                return null;
            }
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
        public bool SetNodeValue(string nodeLevel,string value)
        {
            try
            {
                if (!File.Exists(path))
                {
                    CreateNewFile();
                }
                XmlNode root = _xml.SelectSingleNode(nodeLevel);//   /root/user
                if (null == root)
                    return false;
                //XmlNodeList childlist = root.ChildNodes;
                root.InnerText = value;
                _xml.Save(path);
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}
