﻿using System;
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
    /// CreateEncryptionFileWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CreateEncryptionFileWindow : BaseWindow
    {
        private int[] _simplePassWord = new int[8];
        private TextBox[] _textBoxes;
        private int _currentIndex = 0;
        public CreateEncryptionFileWindow()
        {
            InitializeComponent();
            _textBoxes = new TextBox[8] { tboxCode0, tboxCode1, tboxCode2, tboxCode3, tboxCode4, tboxCode5, tboxCode6, tboxCode7};
            foreach(var tb in _textBoxes)
            {

                System.Windows.Input.InputMethod.SetIsInputMethodEnabled(tb, false);
            }
        }

        private void TboxCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var tb = sender as TextBox;
            if (int.TryParse(e.Text, out int number))
            {
                if (tb.Text.Length == 6 && _currentIndex != 7)
                {
                    _textBoxes[++_currentIndex].Focus();
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void TboxCode_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            _currentIndex = int.Parse( tb.Tag.ToString());
            tb.BorderBrush = SystemColors.ControlTextBrush;
        }

        private bool CheckNumber()
        {
            foreach (var tb in _textBoxes)
            {
                var text = tb.Text;
                var isNumber = int.TryParse(tb.Text, out int number);
                if (text.Length < 6 || !isNumber)
                {
                    tb.BorderBrush = new SolidColorBrush(Colors.Red);
                    return false;
                }
            }
            return true;
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            CheckNumber();
        }

        private void TboxCode_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Back:
                    var tb = _textBoxes[_currentIndex];
                    if (string.IsNullOrEmpty(tb.Text)&& _currentIndex>0)
                    {
                        _textBoxes[--_currentIndex].Focus();
                    }
                    break;
            }
        }
    }
}
