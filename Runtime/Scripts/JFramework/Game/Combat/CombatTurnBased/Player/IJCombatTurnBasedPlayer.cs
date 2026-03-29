using System.Threading.Tasks;

namespace JFramework.Game
{

    public interface IJCombatTurnBasedPlayer<T> : IJCombatPlayer where T : IJCombatUnitData
    {
        void LoadReportData(JCombatTurnBasedReportData<T> reportData);
    }
}
