using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RouterDV
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Input input; //Input子窗口

        public MainWindow()
        {
            InitializeComponent();
        }

        public class MultiRouters
        {
            public List<Router> routers { get; } = new List<Router>();
            // 双向字典不好处理，因此用两个字典，作为双向字典
            


        }

        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            input = new Input();
            input.ShowDialog();
        }

        public class DataStore
        {
            public int[][] RouterConnection { set; get; }

            public static void ReadConfig() //将节点关系数组读取并变化为正方形距离关系数组，默认两节点的距离是5，设置为可调整的，然后不在输入中的节点对，直接给16
            {

            }

        }

    }
}
