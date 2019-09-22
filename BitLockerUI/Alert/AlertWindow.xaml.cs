using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BitLockerUI.Alert
{
    /// <summary>
    /// AlertWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AlertWindow : Window
    {
        public int NotifyTimeSpan { get; set; } = 100;
        private double topPosition;
        private Action _onBtn1ClickCallback;
        private Action _onBtn2ClickCallback;
        public AlertWindow(string content)
        {
            InitializeComponent();
            tBlockAlertContent.Text = content;
        }
        public void SetBtn1Click(string btnText,Action onBtn1ClickCallback)
        {
            btn1.Content = btnText;
            btn1.Visibility = Visibility.Visible;
            _onBtn1ClickCallback = onBtn1ClickCallback;
        }
        public void SetBtn2Click(string btnText, Action onBtn2ClickCallback)
        {
            btn2.Content = btnText;
            btn2.Visibility = Visibility.Visible;
            _onBtn2ClickCallback = onBtn2ClickCallback;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AlertWindow self = sender as AlertWindow;
            if (self != null)
            {
                self.UpdateLayout();
                SystemSounds.Question.Play();//播放提示声

                 topPosition= SystemParameters.WorkArea.Top;//工作区最上边的值
                self.Left = (SystemParameters.WorkArea.Right - ActualWidth) / 2;
                DoubleAnimation animation = new DoubleAnimation();
                animation.Duration = new Duration(TimeSpan.FromMilliseconds(NotifyTimeSpan));//NotifyTimeSpan是自己定义的一个int型变量，用来设置动画的持续时间
                animation.From = topPosition - self.ActualHeight;
                animation.To = topPosition;
                self.BeginAnimation(TopProperty, animation);//设定动画应用于窗体的Left属性

                Task.Factory.StartNew(delegate
                {
                    int seconds = 3;
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(seconds));
                    //Invoke到主进程中去执行
                    Invoke(self, delegate
                    {
                        CloseAlert();
                    });
                });
            }
        }

        static void Invoke(Window win, Action a)
        {
            win.Dispatcher.Invoke(a);
        }
        public  void CloseAlert()
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(NotifyTimeSpan));
            animation.Completed += (s, a) => { Close(); };//动画执行完毕，关闭当前窗体
            animation.From = topPosition;
            animation.To = topPosition - ActualHeight;
            BeginAnimation(TopProperty, animation);
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            CloseAlert();
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            _onBtn1ClickCallback();
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            _onBtn2ClickCallback();
        }
    }
}
