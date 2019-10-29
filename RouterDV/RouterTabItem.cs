using System;
using System.Collections.Generic;

namespace RouterDV
{
    public class RouterTabItem
    {
        public int Distance { set; get; } //两节点距离
        public string NextHop { set; get; } //这里成品情况应该是路由器信息，即"router 3"这种

        public RouterTabItem(int distance, string nextHop) //最普通的初始化方式
        {
            Distance = distance;
            NextHop = nextHop;
        }

        public RouterTabItem(int distance, int nextHopNum) //传进来3的话，就设置为"router 3"
        {
            Distance = distance;
            NextHop = $"router {nextHopNum}";
        }
        public override bool Equals(object obj) //自动生成
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                RouterTabItem routerTabItem = (RouterTabItem)obj;
                return routerTabItem.Distance == Distance && routerTabItem.NextHop == NextHop;
            }

            // TODO: write your implementation of Equals() here
            throw new NotImplementedException();
        }

        public override int GetHashCode() //自动生成
        {
            var hashCode = -462717367;
            hashCode = hashCode * -1521134295 + Distance.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(NextHop);
            return hashCode;
        }
    }
}
