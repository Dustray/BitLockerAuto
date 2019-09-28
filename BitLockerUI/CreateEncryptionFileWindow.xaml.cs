using BitlockerCore;
using BitlockerCore.Encryptions;
using BitLockerUI.Utils;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BitLockerUI
{
    /// <summary>
    /// CreateEncryptionFileWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CreateEncryptionFileWindow : BaseWindow
    {
        private int[] _simplePassWord = new int[8];
        private TextBox[] _textBoxes;
        private int _currentIndex = 0;
        private string _driveName = "";
        public CreateEncryptionFileWindow(string driveName)
        {
            InitializeComponent();
            _textBoxes = new TextBox[8] { tboxCode0, tboxCode1, tboxCode2, tboxCode3, tboxCode4, tboxCode5, tboxCode6, tboxCode7};
            foreach(var tb in _textBoxes)
            {
                InputMethod.SetIsInputMethodEnabled(tb, false);
            }
            _driveName = driveName;
        }
        #region 成员控制
        private void TboxCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var tb = sender as TextBox;
            if (int.TryParse(e.Text, out int number))
            {
                if (tb.Text.Length == 6 && _currentIndex != 7)
                {
                    _textBoxes[++_currentIndex].Focus();
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void TboxCode_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            _currentIndex = int.Parse( tb.Tag.ToString());
            tb.BorderBrush = SystemColors.ControlTextBrush;
        }

        private bool CheckNumber()
        {
            foreach (var tb in _textBoxes)
            {
                var text = tb.Text;
                var isNumber = int.TryParse(tb.Text, out int number);
                if (text.Length < 6 || !isNumber)
                {
                    tb.BorderBrush = new SolidColorBrush(Colors.Red);
                    return false;
                }
            }
            return true;
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (CheckNumber())
            {
                var list = new List<string>();
                foreach (var tb in _textBoxes)
                {
                    list.Add(tb.Text);
                }
                string RecoveryPassword = string.Join("-", list.ToArray());
                var bytes = Encoding.UTF8.GetBytes(RecoveryPassword);
                string str2 = Encoding.UTF8.GetString(bytes);

                var key = "12345678876543211234567887654abc";
                var aes = new AESUtil();
                var afterAESStr = aes.AesEncrypt(RecoveryPassword, key);
                var recoveryFileStream = new RecoveryFileStream();
                var result = recoveryFileStream.Write(@".\Data\",$"bla_{_driveName}.rp", afterAESStr);
                if (result)
                {
                    new AlertWindow("创建密钥文件成功！").Show();
                }
                else
                {
                    new AlertWindow("创建密钥文件失败！").Show();
                }
            }
        }

        private void TboxCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                var tb = _textBoxes[_currentIndex];
                if (string.IsNullOrEmpty(tb.Text) && _currentIndex > 0)
                {
                    _textBoxes[--_currentIndex].Focus();
                }
            }
            else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.V)
            {
                //同时按下了Ctrl + H键（H要最后按，因为判断了此次事件的e.Key）
                //修饰键只能按下Ctrl，如果还同时按下了其他修饰键，则不会进入
                var clipStr = new ClipBoardUtil().GetString();
                AnalyseCodeStr(clipStr);
            }
        }
        
        private bool AnalyseCodeStr(string str)
        {
            str = str.Replace(" ", "");
            var strArray = str.Split('-');
            if (strArray.Length <= 1)
            {
                return false;
            }
            for(int i = 0,j=_currentIndex; i < strArray.Length&&j< _textBoxes.Length; i++,j++)
            {
                var ele = strArray[i];
                if(int.TryParse(ele, out int a))
                {
                    if (ele.Length <= 6)
                    {
                        _textBoxes[j].Text = ele;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            return true;
        }
        #endregion


    }
}
