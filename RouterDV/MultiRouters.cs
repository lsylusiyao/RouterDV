using System.Collections.Generic;

namespace RouterDV
{
    public class MultiRouters
    {
        public Dictionary<int, int> RoutersNum2NO { set; get; } = new Dictionary<int, int>(); //所有路由序号，即从编号查找序号

        //对应另一个是DataStore中的RoutersNO2Num
        public void CreateDict()
        {
            // 根据List创建字典
            for (int i=0;i<MainWindow.data.RoutersNO2Num.Count;i++)
            {
                RoutersNum2NO.Add(MainWindow.data.RoutersNO2Num[i], i);
            }

        }


    }
}
