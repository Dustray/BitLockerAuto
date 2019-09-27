
using System.Windows;

namespace BitLockerUI.Utils
{
    public class ClipBoardUtil
    {
        public string GetString()
        {
            if (Clipboard.ContainsText())
            {
                IDataObject data = Clipboard.GetDataObject();
                return (string)data.GetData(typeof(string));
            }
             return "";
        }
        public bool IsEmpty()
        {
            return Clipboard.GetDataObject().GetFormats().Length == 0;
        }
    }
}
