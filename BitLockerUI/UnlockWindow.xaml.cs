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
        private static string _driveNumber;
        public UnlockWindow(string driveNumber)
        {
            InitializeComponent();
            _driveNumber = driveNumber;
            lblDeviceNumber.Content = $"解锁（{ driveNumber}）";
        }

    }
}
