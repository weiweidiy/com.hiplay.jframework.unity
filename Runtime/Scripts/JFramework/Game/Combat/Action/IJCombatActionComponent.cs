using JFramework;

namespace JFramework.Game
{
    public interface IJCombatActionComponent : IRunable
    {
        void SetOwner(IJCombatAction owner);

        IJCombatAction GetOwner();

        void SetQuery(IJCombatQuery query);
    }
}
