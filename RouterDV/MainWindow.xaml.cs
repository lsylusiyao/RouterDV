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
            Input input = new Input();
            input.ShowDialog();
        }

        public class DataStore
        {
            public int[][] RouterConnection { set; get; }

        }
    }
}
