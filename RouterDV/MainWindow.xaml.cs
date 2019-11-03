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

        public MainWindow()
        {
            InitializeComponent();
            DataContext = data;
            neighborRouterListBox.DataContext = this;
            routerConnectGrid.DataContext = this;
        }

        
        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            input = new Input();
            input.ShowDialog();
            neighborRouterCom.SelectedIndex = -1;
            priorityRouterCom.SelectedIndex = -1;
            stopRouterCom.SelectedIndex = -1;
            CreateRouterTable();
        }

        public ObservableCollection<string> NeighborRouterGridItem { set; get; } = new ObservableCollection<string>();

        /// <summary>
        /// 选择展示邻居路由的选项更改之后，刷新下面的显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void neighborRouterCom_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        public ObservableCollection<int[]> RouterTableObs = new ObservableCollection<int[]>(); //路由表的集合
        /// <summary>
        /// 自动添加行头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void routerConnectGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = data.RoutersName[e.Row.GetIndex()];
        }

        /// <summary>
        /// 将二维数组翻译为需要的特殊形式数组，每一行都是一个一维数组
        /// </summary>
        private void CreateRouterTable()
        {
            routerConnectGrid.ItemsSource = RouterTableObs;
            routerConnectGrid.Columns.Clear();
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
                RouterTableObs.Add(temp);
            }
        }

        private int[,] oldRoutersConnection; //原先路由表的备份
        private void PriorityApplyButton_Click(object sender, RoutedEventArgs e)
        {
            oldRoutersConnection = (int[,])data.RoutersConnection.Clone();
            if(priorityRouterCom.SelectedIndex == -1)
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
            CreateRouterTable();
        }

        private void StopApplyButton_Click(object sender, RoutedEventArgs e)
        {
            oldRoutersConnection = (int[,])data.RoutersConnection.Clone();
            if (stopRouterCom.SelectedIndex == -1)
            {
                MessageBox.Show("什么都没有选择，将不进行任何更改。");
                return;
            }
            //非0和16的变成16，相当于断开
            int select = priorityRouterCom.SelectedIndex;
            for (int i = 0; i < data.RoutersNumCount; i++)
            {
                if (data.RoutersConnection[select, i] == 0) { }
                else data.RoutersConnection[select, i] = 16;

                if (data.RoutersConnection[i, select] == 0) { }
                else data.RoutersConnection[i, select] = 16;
            }
            CreateRouterTable();
        }

        private void StartSendingButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
