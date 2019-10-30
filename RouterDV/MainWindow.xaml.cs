using System;
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
using System.Collections;

namespace RouterDV
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Input input; //Input子窗口
        public static DataStore data = new DataStore();

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
            public int[,] RouterConnection { set; get; } //一定是一个方阵
            public List<string> RoutersName { set; get; } //所有路由名称的合集

            public string OriginRoute { set; get; } //从文本框读进来的数据

            public List<int> RouterNum { set; get; } // 所有路由自身端口数

            public void ReadConfig()
            //将节点关系数组读取并变化为正方形距离关系数组，默认两节点的距离是5，设置为可调整的，然后不在输入中的节点对，直接给16
            {
                int distance = 5; //写在这里方便改
                #region 检查输入是否合法，只能输入数字字母空格逗号回车

                string result = System.Text.RegularExpressions.Regex.Replace(OriginRoute, "[a-zA-Z0-9 ,\r\n\t]", "");
                if (result != "") throw new Exception("输入字符非法，存在非合理字符");

                #endregion

                OriginRoute = OriginRoute.Replace("\r", ""); //防止有\r，先删了
                string[] lineofRoutes = OriginRoute.Split('\n'); // 按回车分割
                List<List<string>> allRoutes = new List<List<string>>(); // 所有的路由信息
                int routerNumCount = 0; //总共输了多少个


                foreach (var el in lineofRoutes) // 把所有路由信息切割生成
                {
                    if (el == "") continue;
                    string s1 = el.Replace(" ", ""); // 删除所有空格
                    string[] s2 = s1.Split(',');
                    allRoutes.Add(new List<string>(s2)); // 按照逗号分割
                    RoutersName.Add(s2[0]); //拿到所有名字
                    RoutersName.Add(s2[1]); //拿到所有自身端口
                    routerNumCount++;
                }

                // 构建矩阵的时候千万别忘了不直接到达写16，打算直接填充
                RouterConnection = new int[routerNumCount, routerNumCount];
                for (int i = 0; i < routerNumCount; i++)
                {
                    for (int j = 0; j < routerNumCount; j++)
                    {
                        if (j == i) RouterConnection[i, j] = 0;
                        else RouterConnection[i, j] = 16;
                    }
                }

            }

        }

    }
}
