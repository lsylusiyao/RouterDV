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

namespace RouterDV
{
    /// <summary>
    /// Input.xaml 的交互逻辑
    /// </summary>
    public partial class Input : Window
    {
        public Input()
        {
            InitializeComponent();
            DataContext = MainWindow.data;
            
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.data.ReadConfig();
            }
            catch (Exception ee)
            {
                var press = MessageBox.Show($"{ee.Message}{System.Environment.NewLine}请修改输入参数后重新点击提交按钮"
                    ,"读取参数错误",MessageBoxButton.OK,MessageBoxImage.Error);
                if (press == MessageBoxResult.OK) return;
            }

            MessageBox.Show("数据读取完成", "数据读取", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            Close();

        }
    }
}
