using System.Collections.Generic;
using System.Threading.Tasks;

namespace JFramework.Game
{
    /// <summary>
    /// 具体创建游戏对象的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IJCombatAnimationPlayer
    {
        Task Initialize<T>(JCombatTurnBasedReportData<T> reportData) where T : IJCombatUnitData;
        Task PlayAcion(JCombatTurnBasedEvent actionEvent);
        Task PlayTurnStart(int frame);

    }
}
