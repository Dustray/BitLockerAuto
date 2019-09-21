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
                    label = $"^加密驱动器 ({label})";
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
            var drive = _driveList[index];
            btnChangeDriveLockState.IsEnabled = !drive.IsReady;
            lblDriveName.Content = string.IsNullOrEmpty(drive.Name) ? "^加密驱动器" : drive.Name;
            lblDriveNumber.Content = $"({drive.Number})";
            lblLockState.Content = drive.IsReady ? "BitLocker未加密或已解锁" : "BitLocker已加密";
            SetDriveSizeInfo(drive.TotalSize, drive.FreeSize);
        }

        public void SetDriveSizeInfo(long totalSize, long freeSize)
        {
            var usingSize = totalSize - freeSize;
            var sizePercent = (int)((double)usingSize / totalSize * 100);
            lblDriveTotalSizeByte.Content = totalSize + "字节";
            lblDriveTotalSizeGB.Content = Math.Round((float)totalSize / 1024 / 1024 / 1024, 1) + "GB";
            lblDriveUsingSizeByte.Content = usingSize + "字节";
            lblDriveUsingSizeGB.Content = Math.Round((float)usingSize / 1024 / 1024 / 1024, 1) + "GB";
            lblDriveFreeSizeByte.Content = freeSize + "字节";
            lblDriveFreeSizeGB.Content = Math.Round((float)freeSize / 1024 / 1024 / 1024, 1) + "GB";
            pbarDriveSize.Value = sizePercent;
        }

        private void BaseWindow_KeyDown(object sender, KeyEventArgs e)
        {
            var index = cBoxDriveList.SelectedIndex;
            if (e.Key == Key.Left)
            {
                cBoxDriveList.SelectedIndex = index == 0 ? _driveList.Count - 1 : index - 1;
            }
            else if (e.Key == Key.Right)
            {
                cBoxDriveList.SelectedIndex = index == _driveList.Count - 1 ? 0 : index + 1;
            }
        }

    }
}
