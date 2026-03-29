using System.Collections.Generic;

namespace JFramework.Game
{
    public class JCombatFindCross : JCombatFindRow
    {
        public JCombatFindCross(float[] args) : base(args)
        {
        }

        public override List<int> GetOtherSeats(int seat)
        {
            return GetOtherSeatsInCross(seat);
        }
        /// <summary>
        /// 获取同一行和同一列的其他点位（不包含自身）
        /// </summary>
        List<int> GetOtherSeatsInCross(int seat)
        {
            var result = new List<int>();
            for (int i = 0; i < seats.GetLength(0); i++)
            {
                for (int j = 0; j < seats.GetLength(1); j++)
                {
                    if (seats[i, j] == seat)
                    {
                        // 找到所在行
                        for (int col = 0; col < seats.GetLength(1); col++)
                        {
                            if (col != j)
                                result.Add(seats[i, col]);
                        }
                        // 找到所在列
                        for (int row = 0; row < seats.GetLength(0); row++)
                        {
                            if (row != i)
                                result.Add(seats[row, j]);
                        }
                        return result;
                    }
                }
            }
            return result;
        }
    }
}
