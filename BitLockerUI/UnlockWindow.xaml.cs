    using BitlockerCore;
using BitlockerCore.Encryptions;
using BitLockerUI.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BitLockerUI
{
    /// <summary>
    /// UnlockWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UnlockWindow : BaseWindow
    {
        private string _driveNumber;
        private Action _onWindowCloseCallback;
        //private const string key = "ahs75jg8skbjg837dhfi98ujg5f4dpla";
        private  string _key = "12345678876543211234567887654abc";
        private bool _isSimplePassword;
        private int[] _simplePassWord = new int[4];
        private PasswordBox[] _passwordBoxes  ;
        private int _simplePassWordIndex=0;
        public UnlockWindow(Action onWindowCloseCallback, string driveNumber, bool simplePassword = true)
        {
            InitializeComponent();
            _key = new AppConfigOperation().Read("AppSecret");
            _onWindowCloseCallback = onWindowCloseCallback;
            _driveNumber = driveNumber;
            lblDeviceNumber.Content = $"解锁（{ driveNumber}）";
            _isSimplePassword = simplePassword;
            tboxPassword.Visibility = simplePassword ? Visibility.Collapsed : Visibility.Visible;
            btnSubmit.Visibility = simplePassword ? Visibility.Collapsed : Visibility.Visible;
            gridSimplePassword.Visibility = !simplePassword ? Visibility.Collapsed : Visibility.Visible;
            if (simplePassword)
            {
                _passwordBoxes = new PasswordBox[4] { lblPassword0, lblPassword1, lblPassword2, lblPassword3 };
                _passwordBoxes[0].Focus();
            }
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Submit();
        }

        private void Submit()
        {
            var recoveryFileStream = new RecoveryFileStream();
            var byteFile = recoveryFileStream.Read(@".\Data\bitlockerauto.rp");
            if (0 == byteFile.Length)
            {
                btnErrorHint.Content = "未找到用户密钥文件";
                btnErrorHint.Visibility = Visibility.Visible;
                ClearSimplePassword();
                return;
            }
            var aes = new AESUtils();
            var afterAESStr = aes.AesDecrypt(byteFile, _key);
            if (string.IsNullOrEmpty(afterAESStr))
            {
                btnErrorHint.Content = "密钥文件解析失败";
                btnErrorHint.Visibility = Visibility.Visible;
                ClearSimplePassword();
                return;
            }
            var bl = new BitLockerExecute(_driveNumber[0].ToString());
            if (!bl.Unlock(afterAESStr))
            {
                btnErrorHint.Content = "加载了非此驱动器的密钥文件";
                btnErrorHint.Visibility = Visibility.Visible;
                ClearSimplePassword();
                return;
            }

            _onWindowCloseCallback();
            Close();
        }

        private void TboxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            btnErrorHint.Visibility = Visibility.Hidden;
            btnSubmit.IsEnabled = tboxPassword.Password.Length >= 4;

        }



        private void LblPassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var lbl = sender as PasswordBox;
            if (char.IsDigit(e.Text[0]))
            {
                _simplePassWord[_simplePassWordIndex++] = int.Parse(e.Text);
                
                for(int i = 0; i < 4; i++)
                {
                    var thisChar = i == _simplePassWordIndex;//i是下一个框
                   _passwordBoxes[i].Focusable = thisChar;//下一个框允许聚焦
                    if (thisChar)//i是下一个框
                    {
                        if(i>0)//并且i大于0
                        _passwordBoxes[i - 1].Password = "1";//前一个框输入数字
                        _passwordBoxes[i].Focus();//下一个框聚焦
                    }
                }
                if (_simplePassWordIndex == 4)//最后一个框
                {
                    _simplePassWordIndex = 0;
                    _passwordBoxes[3].Password = "1";//前一个框输入数字
                    for (int i = 0; i < 4; i++)
                    {
                        _passwordBoxes[i].Focusable = false;
                    }
                    Submit();
                }
            }
            e.Handled = true;

        }
        private void ClearSimplePassword()
        {
            if (_isSimplePassword)
            {
                _simplePassWordIndex = 0;
                for (int i = 0; i < 4; i++)
                {
                    _passwordBoxes[i].Focusable = false;
                    _passwordBoxes[i].Clear();
                }
                _passwordBoxes[0].Focusable = true;
                _passwordBoxes[0].Focus();
            }
        }
       
    }
}
