using BitLockerUI.Useless;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BitLockerUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : BaseWindow
    {
        private List<DriveEtt> _driveList;
        public MainWindow()
        {
            InitializeComponent();
            InitDriveList();
        }

        /// <summary>
        /// 初始化驱动器列表
        /// </summary>
        private void InitDriveList()
        {
            var driveList = new DriveUtil().GetDriveList();
            _driveList = driveList;
            var driveArray = new string[driveList.Count];
            for (int i = 0; i < driveList.Count; i++)
            {
                var drive = driveList[i];
                var label = drive.Number;
                if (drive.IsReady)
                {
                    label = $"{drive.Name} ({label})";
                }
                else
                {
                    label = $"^已锁定驱动器 ({label})";
                }
                driveArray[i] = label;
            }
            cBoxDriveList.ItemsSource = driveArray;
            cBoxDriveList.SelectedIndex = 0;
        }
        private void BtnChangeDriveLockState_Click(object sender, RoutedEventArgs e)
        {
            var index = cBoxDriveList.SelectedIndex;
            var window = new UnlockWindow(_driveList[index].Number);
            window.ShowDialog();

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = cBoxDriveList.SelectedIndex;

            btnChangeDriveLockState.IsEnabled = !_driveList[index].IsReady;

        }
    }
}
