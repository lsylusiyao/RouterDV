using System;
using System.Collections.Generic;

namespace RouterDV
{
    public class DataStore
    {
        public int[,] RoutersConnection { set; get; } //一定是一个方阵
        public List<string> RoutersName { set; get; } = new List<string>(); //所有路由名称的合集

        public string OriginRoute { set; get; } = string.Empty; //从文本框读进来的原始数据

        public List<int> RoutersNO2Num { set; get; } = new List<int>(); // 所有路由自身端口号，即序号查找编号

        public void ReReadConfig() //在输入变化的时候，重新读取
        {
            // RoutersConnection是new int的，所以不用管
            RoutersName.Clear();
            OriginRoute = string.Empty;
            RoutersNO2Num.Clear();
            ReadConfig();
        }

        public void ReadConfig()
        //将节点关系数组读取并变化为正方形距离关系数组，默认两节点的距离是5，设置为可调整的，然后不在输入中的节点对，直接给16
        {
            int distance = 5; //写在这里方便改

            ///通过循环方式，找到port对应的数组序号
            int FindRouter(int port)
            {
                for (int i = 0; i < RoutersNO2Num.Count; i++)
                {
                    if (RoutersNO2Num[i] == port) return i;
                }
                return -1;
            }

            #region 检查输入是否合法，只能输入数字字母空格逗号回车

            if (OriginRoute == string.Empty) throw new Exception("未填写内容");

            string result = System.Text.RegularExpressions.Regex.Replace(OriginRoute, "[a-zA-Z0-9 ,\r\n\t]", "");
            if (result != string.Empty) throw new Exception("输入字符非法，存在非合理字符");

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
                RoutersNO2Num.Add(Convert.ToInt32(s2[1])); //拿到所有自身端口
                routerNumCount++;
            }

            // 直接对矩阵初始化时，填充0和16
            RoutersConnection = new int[routerNumCount, routerNumCount];
            for (int i = 0; i < routerNumCount; i++)
            {
                for (int j = 0; j < routerNumCount; j++)
                {
                    if (j == i) RoutersConnection[i, j] = 0;
                    else RoutersConnection[i, j] = 16;
                }

                for (int j = 2; j < allRoutes[i].Count; j++)
                {
                    int which = FindRouter(Convert.ToInt32(allRoutes[i][j])); // 找到了给出信息里面的其他路由端口对应的是第几个路由
                    if (which == -1)
                        throw new Exception("路由查找出错，请检查"); //不应该没找到，但是留一个保险
                    else
                        RoutersConnection[i, which] = distance; //找到了就设置为distance
                }
            }




        }

    }
}
