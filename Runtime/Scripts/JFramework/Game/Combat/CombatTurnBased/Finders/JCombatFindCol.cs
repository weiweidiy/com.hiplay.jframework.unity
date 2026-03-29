using System.Collections.Generic;

namespace JFramework.Game
{
    public class JCombatFindCol : JCombatFindRow
    {
        public JCombatFindCol(float[] args) : base(args)
        {
        }

        public override List<int> GetOtherSeats(int seat)
        {
            return GetOtherSeatsInColumn(seat);
        }

        /// <summary>
        /// 获取同一列的其他点位（不包含自身）
        /// </summary>
        List<int> GetOtherSeatsInColumn(int seat)
        {
            var result = new List<int>();
            for (int i = 0; i < seats.GetLength(0); i++)
            {
                for (int j = 0; j < seats.GetLength(1); j++)
                {
                    if (seats[i, j] == seat)
                    {
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
