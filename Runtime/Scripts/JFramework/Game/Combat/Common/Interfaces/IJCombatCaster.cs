namespace JFramework.Game
{
    public interface IJCombatCaster : IUnique
    {
        /// <summary>
        /// 是否可以释放
        /// </summary>
        /// <returns></returns>
        bool CanCast();

        /// <summary>
        /// 释放
        /// </summary>
        void Cast();


    }
}
