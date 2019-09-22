using BitlockerCore;
using BitlockerCore.Encryptions;
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
        public UnlockWindow(Action onWindowCloseCallback,string driveNumber)
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
                MessageBox.Show("未找到用户密钥文件");
                return;
            }
            var aes = new AESUtils();
            var afterAESStr = aes.AesDecrypt(byteFile, key);
            if (string.IsNullOrEmpty(afterAESStr))
            {

                MessageBox.Show("密钥文件解析失败");
                return;
            }
            var bl = new BitLockerExecute(_driveNumber[0].ToString());
            if (!bl.Unlock(afterAESStr))
            {
                MessageBox.Show("密钥文件错误");
                return;
            }

            // bl.Lock();
            _onWindowCloseCallback();
            Close();
        }
    }
}
