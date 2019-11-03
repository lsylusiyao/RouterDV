using System.Collections.Generic;

namespace RouterDV
{
    public class MultiRouters
    {
        public Dictionary<int, int> RoutersNum2NO { set; get; } = new Dictionary<int, int>(); //所有路由序号，即从编号查找序号

        public List<Router> Routers { get; } = new List<Router>();
        //对应另一个是DataStore中的RoutersNO2Num

        int[,] isNeighbor;
        public void CreateDict()
        {
            RoutersNum2NO.Clear();
            // 根据List创建字典
            for (int i=0;i<MainWindow.data.RoutersNO2Num.Count;i++)
            {
                RoutersNum2NO.Add(MainWindow.data.RoutersNO2Num[i], i);
            }

        }
        /// <summary>
        /// 初始化Multi，但是要确保之前执行过ReadConfig()
        /// </summary>
        public void Init()
        {
            int[,] configInfoArray = MainWindow.data.RoutersConnection;
            for (int i = 0; i < configInfoArray.Rank; i++)
            {
                Router router = new Router();
                router.ID = i;
                for(int j=0;j<configInfoArray.Rank;j++) //反正是正方形数组，就这样吧
                {
                    int distance = configInfoArray[i, j];
                    int nextHop = j;
                    router.RouterTableItems.Add(new RouterTabItem(distance, nextHop));
                }
                Routers.Add(router);
            }
        }
        
        /// <summary>
        /// 对List的深拷贝函数
        /// </summary>
        /// <param name="from">复制的source</param>
        /// <param name="to">复制的destination</param>
        void Copy(List<RouterTabItem>from, List<RouterTabItem>to)
        {
            foreach(var v in from)
            {
                to.Add(new RouterTabItem(v.Distance, v.NextHop));
            }
        }

        /// <summary>
        /// 创建相邻路由的访问数组
        /// </summary>
        void CreateIsNeighbor()
        {
            int length = MainWindow.data.RoutersConnection.Rank;
            isNeighbor = new int[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (i == j || MainWindow.data.RoutersConnection[i, j] == 16)
                        isNeighbor[i, j] = 0;
                    else
                        isNeighbor[i, j] = 1;
                }
            }
        }

        public void Update()
        {
            Router router; //准备更新的路由
            Router neighboRouter; //router的邻居

            List<RouterTabItem> old = new List<RouterTabItem>();

            for (int i = 0; i < Routers.Count; i++)
            {
                router = Routers[i];
                old.Clear();
                Copy(router.RouterTableItems, old);

                for (int j = 0; j < Routers.Count; j++)
                {
                    neighboRouter = Routers[j];
                    if (isNeighbor[i, j] != 0)
                    {
                        router.UpdateRouterTable(neighboRouter, old[j].Distance);
                    }
                }
            }
        }
    }
}
