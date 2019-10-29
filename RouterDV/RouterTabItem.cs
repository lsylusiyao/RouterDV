using System;
using System.Collections.Generic;

namespace RouterDV
{
    public class RouterTabItem
    {
        public int Distance { set; get; } //两节点距离
        public int NextHop { set; get; } //这里是路由器序号，从0开始

        public RouterTabItem(int distance, int nextHopNum)
        {
            Distance = distance;
            NextHop = nextHopNum;
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

        public override int GetHashCode()
        {
            var hashCode = -462717367;
            hashCode = hashCode * -1521134295 + Distance.GetHashCode();
            hashCode = hashCode * -1521134295 + NextHop.GetHashCode();
            return hashCode;
        }
    }
}
