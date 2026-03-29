namespace JFramework.Game
{
    public class JCombatDamageData : JCombatExtraData, IJCombatDamageData
    {
        int damage;
        int damageType;
        public JCombatDamageData(string uid, string sourceUnitUid, string actionUid, int damage, int damageType, string targetUid) : base(uid, sourceUnitUid, actionUid, targetUid)
        {
            this.damage = damage;
            this.damageType = damageType;
        }

        public int GetDamage()
        {
            return damage;
        }

        public void SetDamage(int damage)
        {
            this.damage = damage;
        }

        public int GetDamageType()
        {
            return (int)damageType;
        }

        public void SetDamageType(int damageType)
        {
            this.damageType = damageType;
        }
    }
}
