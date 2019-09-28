using BitlockerCore.Encryptions;
using BitLockerUI.Utils;
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
    /// SetPasswordWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetPasswordWindow : BaseWindow
    {
        private int _maxSize  = 20;
        private int _minSize = 4;
        private bool IsSimplePass = false; 
        public SetPasswordWindow()
        {
            InitializeComponent();
            pboxPassword1.MaxLength = 32;
            pboxPassword2.MaxLength = 32;
        }

        #region 成员控制
        private void cboxIsSimplePassword_Checked(object sender, RoutedEventArgs e)
        {
            _maxSize = 4;
            IsSimplePass = true;
            ChangeToSimplePassword(true);
        }
        private void cboxIsSimplePassword_Unchecked(object sender, RoutedEventArgs e)
        {
            IsSimplePass = false;
            ChangeToSimplePassword(false);
        }

        private void PboxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            bool flag = pboxPassword1.Password.Length >= 4 && pboxPassword2.Password.Length >= 4;
            if (IsSimplePass)
            {
                flag = flag && (pboxPassword1.Password.Length <= 4) && (pboxPassword2.Password.Length <= 4);
            }
            else
            {
                flag = flag && (pboxPassword1.Password.Length <= 32) && (pboxPassword2.Password.Length <= 32);
            }
            flag = flag && (pboxPassword1.Password == pboxPassword2.Password);
            btnSubmit.IsEnabled = flag;
        }

        private void ChangeToSimplePassword(bool flag)
        {

            pboxPassword1.Clear();
            pboxPassword2.Clear();
            pboxPassword1.MaxLength = pboxPassword2.MaxLength = flag ? 4 : 32;
            pboxPassword2.MaxLength = pboxPassword2.MaxLength = flag ? 4 : 32;


        }

        private void pboxPassword1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = CanBeInput(e.Text);
        }

        private void pboxPassword2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = CanBeInput(e.Text);
        }

        private bool CanBeInput(string str)
        {
            if (IsSimplePass)
            {
                if (!int.TryParse(str, out int a))
                {
                    return true;
                }
            }

            return false;
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            SavePassword();
        }
        #endregion
        #region 数据操作
        private void SavePassword()
        {
            string password= pboxPassword1.Password;
            string secretCode = new AppConfigOperation().Read("AppSecret");
            var aesUtil = new AESUtil();
            var byteDatas = aesUtil.AesEncrypt(password, secretCode);
            var strData = Encoding.UTF8.GetString(byteDatas);
            var xml = new XMLOperation();
            if(xml.SetNodeValue("/root/user/unlockpassword", strData))
                {
                new AlertWindow("设置密码成功！").Show();
                Close();
            }
            else
            {

                new AlertWindow("设置密码失败！").Show();
            }

        }
        #endregion
    }
}
