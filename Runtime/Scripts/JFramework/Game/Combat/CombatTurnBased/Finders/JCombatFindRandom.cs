using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// 查找指定数量的单位，参数1：数量
    /// </summary>
    public class JCombatFindRandom : JCombatFindRow
    {
        int count;
        public JCombatFindRandom(float[] args) : base(args)
        {
            count = (int)GetArg(0);
        }

        public override List<int> GetOtherSeats(int seat)
        {
            return GetOtherSeatsInRandom(seat,count);
        }

        List<int> GetOtherSeatsInRandom(int seat, int count)
        {
            var result = new List<int>();
            var random = new System.Random();
            var allSeats = new List<int>();
            // 获取所有座位
            for (int i = 0; i < seats.GetLength(0); i++)
            {
                for (int j = 0; j < seats.GetLength(1); j++)
                {
                    allSeats.Add(seats[i, j]);
                }
            }
            // 移除当前座位
            allSeats.Remove(seat);
            // 随机选择指定数量的座位
            for (int i = 0; i < count && allSeats.Count > 0; i++)
            {
                int index = random.Next(allSeats.Count);
                result.Add(allSeats[index]);
                allSeats.RemoveAt(index); // 确保不重复选择
            }
            return result;


        }
    }
}
