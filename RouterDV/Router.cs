using System.Collections.Generic;

namespace RouterDV
{
    public class Router
    {
        public int ID { set; get; }
        public List<RouterTabItem> RouterTableItems { set; get; } = new List<RouterTabItem>();

        public void UpdateRouterTable(Router neighborRouter, int distance)
        {
            RouterTabItem routerTab;
            RouterTabItem neighborRouterTab;
            for (int i = 0; i < RouterTableItems.Count; i++)
            {
                routerTab = RouterTableItems[i];
                neighborRouterTab = neighborRouter.RouterTableItems[i];
                // 设定跳数为16时，不能达到路由，因此这里判断一下
                if (routerTab.Distance == 16)
                {
                    // 邻居目前可以到达该路由，则：更新（此处单独分类，防止：10 + 13 > 16 而未更新）
                    if (neighborRouterTab.Distance != 16)
                    {
                        routerTab.Distance = distance + neighborRouterTab.Distance;
                        routerTab.NextHop = neighborRouter.ID;
                    }
                }
                else
                { // 按照公式进行路由更新
                    int tempDistance = distance + neighborRouterTab.Distance;
                    if (routerTab.Distance > tempDistance)
                    {
                        routerTab.Distance = tempDistance;
                        routerTab.NextHop = neighborRouter.ID;
                    }
                }

            }
        }

    }
}
