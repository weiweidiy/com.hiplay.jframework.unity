using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// 找主目标同一行的所有活着的单位
    /// </summary>
    public class JCombatFindRow : JCombatFindOppoDefault
    {
        public JCombatFindRow(float[] args) : base(args)
        {
        }

        protected override List<IJCombatCasterTargetableUnit> GetTargets()
        {
            var targetTeam = GetTargetTeam();
            var targets = base.GetTargets(); //这个是只有主目标的

            if (targets.Count == 0)
            {
                return targets; //如果没有主目标，直接返回空列表
            }

            var q = query as IJCombatSeatBasedQuery;
            var mainTarget = targets[0]; //获取主目标
            var targetSeatId = q.GetSeat(mainTarget.Uid); ; //获取主目标所在行
            var otherSeats = GetOtherSeats(targetSeatId); //获取同一行的其他座位

            foreach (var seat in otherSeats)
            {
                var targetsUnit = q.GetUnit(targetTeam, seat);
                if (targetsUnit != null && !targetsUnit.IsDead())
                {
                    targets.Add(targetsUnit);
                }
            }

            return targets;
        }

        /// <summary>
        /// 获取同一行的其他座位
        /// </summary>
        /// <param name="seat"></param>
        /// <returns></returns>
        public virtual List<int> GetOtherSeats(int seat)
        {       
            return GetOtherSeatsInRow(seat);
        }

        List<int> GetOtherSeatsInRow(int seat)
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
                        return result;
                    }
                }
            }
            return result;
        }
    }
}
