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
using System.Collections;
using System.Collections.ObjectModel;

namespace RouterDV
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Input input; //Input子窗口
        public static DataStore data = new DataStore();
        private MultiRouters routers = new MultiRouters();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = data;
            neighborRouterListBox.DataContext = this;
            routerConnectGrid.DataContext = this;
            routerConnectGrid.ItemsSource = RouterRelationObs;
            routerTableGrid.DataContext = this;
            routerTableGrid.ItemsSource = RouterTableObs;
        }

        
        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            input = new Input();
            input.ShowDialog();
            neighborRouterCom.SelectedIndex = -1;
            priorityRouterCom.SelectedIndex = -1;
            stopRouterCom.SelectedIndex = -1;
            RenewEverything();
        }

        /// <summary>
        /// 把所有需要刷新的东西都写在这里，省得忘
        /// </summary>
        private void RenewEverything()
        {
            CreateRouterRelationShip();
            CreateRouterTable();
        }

        public ObservableCollection<string> NeighborRouterGridItem { set; get; } = new ObservableCollection<string>();

        /// <summary>
        /// 选择展示邻居路由的选项更改之后，刷新下面的显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NeighborRouterCom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int select = neighborRouterCom.SelectedIndex;
            NeighborRouterGridItem.Clear();
            for (int j = 0; j < data.RoutersNumCount; j++)
            {
                if (!(data.RoutersConnection[select, j] == 0 || data.RoutersConnection[select, j] == 16))
                {
                    NeighborRouterGridItem.Add(data.RoutersName[j]);
                }
            }
        }

        public ObservableCollection<int[]> RouterRelationObs = new ObservableCollection<int[]>(); //路由关系图的集合
        /// <summary>
        /// 自动添加行头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RouterConnectGrid_LoadingRow(object sender, DataGridRowEventArgs e) => e.Row.Header = data.RoutersName[e.Row.GetIndex()];

        /// <summary>
        /// 将二维数组翻译为需要的特殊形式数组并展示出来，每一行都是一个一维数组
        /// </summary>
        private void CreateRouterRelationShip()
        {
            routerConnectGrid.Columns.Clear();
            RouterRelationObs.Clear();
            routerConnectGrid.Items.Refresh();
            for (int i = 0; i < data.RoutersNumCount; i++)
            {
                DataGridTextColumn dc = new DataGridTextColumn
                {
                    Header = $"{data.RoutersName[i]}",
                    Binding = new Binding($"[{i.ToString()}]")
                };
                routerConnectGrid.Columns.Add(dc);
            }

            for (int i = 0; i < data.RoutersNumCount; i++)
            {
                int[] temp = new int[data.RoutersNumCount];
                for (int j = 0; j < data.RoutersNumCount; j++)
                {
                    temp[j] = data.RoutersConnection[i, j];
                }
                RouterRelationObs.Add(temp);
            }
        }
        
        public ObservableCollection<string[]> RouterTableObs = new ObservableCollection<string[]>(); //路由表的集合，内部数组大小应为3
        
        /// <summary>
        /// 路由表的生成函数，自动
        /// </summary>
        private void CreateRouterTable()
        {
            // obj: Dest-Router, Distance, Next-HopRouterTableStruct
            routers.Init();
            routers.Update();
            RouterTableObs.Clear();
            int tempNameNum = 0;

            foreach (var router in routers.Routers) //显示所有的路由表
            {
                int temp = 0;
                RouterTableObs.Add(new string[3] { data.RoutersName[tempNameNum++].ToUpper(), "", "" });
                var routerTableItems = router.RouterTableItems;
                foreach(var routerItem in routerTableItems)
                {
                    string[] s = new string[3];
                    s[0] = data.RoutersName[temp++];
                    s[1] = routerItem.Distance.ToString();
                    var nextHopId = routerItem.NextHop;
                    string tempStr;
                    if (nextHopId == router.ID || routerItem.Distance == 16)
                        tempStr = "-";
                    else
                        tempStr = data.RoutersName[routerItem.NextHop];
                    s[2] = tempStr;
                    RouterTableObs.Add(s);
                }
                // 加一行空白
                RouterTableObs.Add(System.Array.Empty<string>()) ;
            }
        }
        
        
        private void PriorityApplyButton_Click(object sender, RoutedEventArgs e)
        {
            data.RoutersConnection = (int[,])data.OldRoutersConnection.Clone();
            if (priorityRouterCom.SelectedIndex == -1)
            {
                MessageBox.Show("什么都没有选择，将不进行任何更改。");
                return;
            }
            //非0和16的变成1，相当于很近
            int select = priorityRouterCom.SelectedIndex;
            for (int i = 0; i < data.RoutersNumCount; i++)
            {
                if (data.RoutersConnection[select, i] == 16 || data.RoutersConnection[select, i] == 0) { }
                else data.RoutersConnection[select, i] = 1;

                if (data.RoutersConnection[i, select] == 16 || data.RoutersConnection[i, select] == 0) { }
                else data.RoutersConnection[i, select] = 1;
            }
            RenewEverything();
            MessageBox.Show("优先路由设置完成", "设置", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void StopApplyButton_Click(object sender, RoutedEventArgs e)
        {
            data.RoutersConnection = (int[,])data.OldRoutersConnection.Clone();
            if (stopRouterCom.SelectedIndex == -1)
            {
                MessageBox.Show("什么都没有选择，将不进行任何更改。");
                return;
            }
            //非0和16的变成16，相当于断开
            int select = stopRouterCom.SelectedIndex;
            for (int i = 0; i < data.RoutersNumCount; i++)
            {
                if (data.RoutersConnection[select, i] == 0) { }
                else data.RoutersConnection[select, i] = 16;

                if (data.RoutersConnection[i, select] == 0) { }
                else data.RoutersConnection[i, select] = 16;
            }
            RenewEverything();
            MessageBox.Show("停用路由设置完成", "设置", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        /// <summary>
        /// 生成发包顺序表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartSendingButton_Click(object sender, RoutedEventArgs e)
        {
            if(sourceRouterCom.SelectedItem == null || destRouterCom.SelectedItem == null)
            {
                MessageBox.Show("请选择源路由和目标路由，并重试", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }
    }
}
