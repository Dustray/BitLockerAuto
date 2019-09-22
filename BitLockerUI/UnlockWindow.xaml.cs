using BitlockerCore;
using BitlockerCore.Encryptions;
using BitLockerUI.Alert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        private const string key = "12345678876543211234567887654abc";
        public UnlockWindow(Action onWindowCloseCallback, string driveNumber)
        {
            InitializeComponent();
            _onWindowCloseCallback = onWindowCloseCallback;
            _driveNumber = driveNumber;
            lblDeviceNumber.Content = $"解锁（{ driveNumber}）";
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var recoveryFileStream = new RecoveryFileStream();
            var byteFile = recoveryFileStream.Read(@".\Data\bitlockerauto.rp");
            if (0 == byteFile.Length)
            {
                btnErrorHint.Content = "未找到用户密钥文件";
                btnErrorHint.Visibility = Visibility.Visible;
                return;
            }
            var aes = new AESUtils();
            var afterAESStr = aes.AesDecrypt(byteFile, key);
            if (string.IsNullOrEmpty(afterAESStr))
            {
                btnErrorHint.Content = "密钥文件解析失败";
                btnErrorHint.Visibility = Visibility.Visible;
                return;
            }
            var bl = new BitLockerExecute(_driveNumber[0].ToString());
            if (!bl.Unlock(afterAESStr))
            {
                btnErrorHint.Content = "密钥文件错误";
                btnErrorHint.Visibility = Visibility.Visible;
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
    }
}
