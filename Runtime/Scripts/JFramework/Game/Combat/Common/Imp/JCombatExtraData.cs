using JFramework.Game;

namespace JFramework.Game
{
    public class JCombatExtraData : IJCombatExtraData
    {
        string sourceUnitUid;
        string actionUid;
        string targetUnitUid;
        public JCombatExtraData(string uid, string sourceUnitUid, string actionUid, string targetUnitUid)
        {
            this.actionUid = actionUid;
            this.sourceUnitUid = sourceUnitUid;
            this.targetUnitUid = targetUnitUid;
            Uid = uid;
        }

        public string Uid { get; protected set; }

        public string GetActionSourceUid()
        {
            return this.actionUid;
        }

        public string GetCasterUid()
        {
            return this.sourceUnitUid;
        }

        public string GetTargetUid()
        {
            return this.targetUnitUid;
        }
    }
}
