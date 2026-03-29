namespace JFramework.Game
{
    public interface IJCombatUnitBuilder
    {
        /// <summary>
        /// Builds a combat unit with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the combat unit to build.</param>
        /// <returns>A new instance of JCombatUnitInfo with the specified ID.</returns>
        IJCombatUnitInfo Build();
    }
}
