using System;
using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// 基于战位的查找器, 基础查找器，深度找
    /// </summary>
    public class JCombatFindOppoDefault : JCombatFinderBase
    {

        protected int[,] seats = new int[3, 3]
        {
           { 0,1,2 },
           { 3,4,5 },
           { 6,7,8 }
        };


        public JCombatFindOppoDefault(float[] args) : base(args) { }

        /// <summary>
        /// 获取有效参数数量
        /// </summary>
        /// <returns></returns>
        protected override int GetValidArgsCount()
        {
            return 0;
        }

        /// <summary>
        /// 获取目标数据
        /// </summary>
        /// <returns></returns>
        public override IJCombatExecutorExecuteArgs GetTargetsData()
        {
            executeArgs.TargetUnits = GetTargets();
            return executeArgs;
        }

        /// <summary>
        /// 获取目标列表
        /// </summary>
        /// <returns></returns>
        protected virtual List<IJCombatCasterTargetableUnit> GetTargets()
        {
            var result = new List<IJCombatCasterTargetableUnit>();

            var primaryTarget = FindPrimaryTarget();

            if(primaryTarget != null)
            {
                result.Add(primaryTarget);
            }
            else
            {
                // 如果没有找到主目标，则返回空列表
                return result;
            }

            return result;
        }

        /// <summary>
        /// 查找主目标
        /// </summary>
        /// <returns></returns>
        IJCombatCasterTargetableUnit FindPrimaryTarget()
        {
            var myUnitUid = GetOwner().GetCaster();
            var targetTeam = GetTargetTeam();

            var q = query as IJCombatSeatBasedQuery;
            if (q != null)
            {
                var mySeat = q.GetSeat(myUnitUid);
                var targetsSeats = FindTargetsSeat(mySeat);

                foreach (var seat in targetsSeats)
                {
                    var targetsUnit = q.GetUnit(targetTeam, seat);
                    if (targetsUnit != null && !targetsUnit.IsDead())
                    {
                        return targetsUnit;
                    }
                }
            }

            return null;
        }



        

        /// <summary>
        /// 根据自己的位置获取目标位置
        /// </summary>
        /// <param name="mySeat"></param>
        /// <returns></returns>
        protected virtual List<int> FindTargetsSeat(int mySeat)
        {
            return GetSortedSeats(seats, mySeat);
        }

        /// <summary>
        /// 游戏规则：根据输入的座位号，返回排序后的座位列表
        /// </summary>
        /// <param name="seats"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        List<int> GetSortedSeats(int[,] seats, int input)
        {
            // 1. 确定输入所在的行
            int targetRow = -1;
            for (int i = 0; i < seats.GetLength(0); i++)
            {
                for (int j = 0; j < seats.GetLength(1); j++)
                {
                    if (seats[i, j] == input)
                    {
                        targetRow = i;
                        break;
                    }
                }
                if (targetRow != -1) break;
            }

            // 2. 定义行的优先级顺序
            List<int> rowOrder = new List<int>();
            if (targetRow == 0) // 输入在第1行 → 1,2,3,4,5,6,7,8,9
            {
                rowOrder = new List<int> { 0, 1, 2 };
            }
            else if (targetRow == 1) // 输入在第2行 → 4,5,6,1,2,3,7,8,9
            {
                rowOrder = new List<int> { 1, 0, 2 };
            }
            else if (targetRow == 2) // 输入在第3行 → 7,8,9,4,5,6,1,2,3
            {
                rowOrder = new List<int> { 2, 1, 0 };
            }

            // 3. 按优先级顺序拼接行
            List<int> result = new List<int>();
            foreach (int row in rowOrder)
            {
                for (int j = 0; j < seats.GetLength(1); j++)
                {
                    result.Add(seats[row, j]);
                }
            }

            return result;
        }


    }
}
