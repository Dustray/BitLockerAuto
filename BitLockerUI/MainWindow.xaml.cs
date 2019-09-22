using BitlockerCore;
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
        private bool _currentLocked = false;
        public MainWindow()
        {
            InitializeComponent();
            InitDriveList();
        }

        /// <summary>
        /// 初始化驱动器列表
        /// </summary>
        private void InitDriveList(int index = 0)
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
            cBoxDriveList.SelectedIndex = index;
        }
        private void BtnChangeDriveLockState_Click(object sender, RoutedEventArgs e)
        {
            if (_currentLocked)
            {
                var index = cBoxDriveList.SelectedIndex;
                var window = new UnlockWindow(OnUnlockWindowClose, _driveList[index].Number);
                window.ShowDialog();
            }
            else
            {
                var bl = new BitLockerExecute(_driveList[cBoxDriveList.SelectedIndex].Number[0].ToString());
                if (bl.Lock())
                {
                    MessageBox.Show("驱动器已加锁");
                    InitDriveList(cBoxDriveList.SelectedIndex);
                }
                else
                {
                    MessageBox.Show("当前驱动器未使用Bitlocker加密");
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (-1 == cBoxDriveList.SelectedIndex) return;
            var index = cBoxDriveList.SelectedIndex;
            var drive = _driveList[index];
            //加解锁按钮
            //btnChangeDriveLockState.IsEnabled = !drive.IsReady;
            btnChangeDriveLockState.Content = drive.IsReady ? "加锁" : "解锁";
            btnChangeDriveLockState.Background = drive.IsReady ? SystemColors.HighlightTextBrush : SystemColors.HighlightBrush;
            btnChangeDriveLockState.Foreground = drive.IsReady ? SystemColors.HighlightBrush : SystemColors.HighlightTextBrush;
            _currentLocked = !drive.IsReady;
            //加解锁按钮
            lblDriveName.Content = string.IsNullOrEmpty(drive.Name) ? "^加密驱动器" : drive.Name;
            lblDriveNumber.Content = $"({drive.Number})";
            lblLockState.Content = drive.IsReady ? "BitLocker未加密或已解锁" : "BitLocker已加锁";
            SetDriveSizeInfo(drive.TotalSize, drive.FreeSize);
            cBoxDriveList.Focusable = false;

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


        #region Callback
        private void OnUnlockWindowClose()
        {
            InitDriveList(cBoxDriveList.SelectedIndex);
        }
        #endregion
    }
}
