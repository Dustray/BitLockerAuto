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

        public SetPasswordWindow()
        {
            InitializeComponent();
        }

        private void cboxIsSimplePassword_Checked(object sender, RoutedEventArgs e)
        {
            pboxPassword1.Clear();
            pboxPassword2.Clear();
        }
        private void cboxIsSimplePassword_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void PboxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
