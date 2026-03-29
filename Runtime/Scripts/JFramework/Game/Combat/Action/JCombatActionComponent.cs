using System;

namespace JFramework.Game
{
    public abstract class JCombatActionComponent : BaseRunable, IJCombatActionComponent
    {
        IJCombatAction owner;

        protected IJCombatQuery query;

        float[] args;

        public JCombatActionComponent(float[] args)
        {
            this.args = args;

            var validArgsCount = GetValidArgsCount();
            if (args != null && args.Length != validArgsCount)
            {
                throw new ArgumentException($"Invalid number of arguments. Expected {validArgsCount}, but got {args.Length}. {GetType().ToString()}");
            }

            if (args == null && validArgsCount > 0)
            {
                throw new ArgumentException($"Arguments cannot be null. Expected {validArgsCount} arguments.. {GetType().ToString()}");
            }
        }

        protected float GetArg(int index)
        {
            if (args == null || index < 0 || index >= args.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"Index is out of range for the provided arguments.. {GetType().ToString()}");
            }
            return args[index];
        }

        /// <summary>
        /// 有效参数的数量
        /// </summary>
        /// <returns></returns>
        protected abstract int GetValidArgsCount();



        public IJCombatAction GetOwner() => owner;

        public virtual void SetOwner(IJCombatAction owner) => this.owner = owner;

        public virtual void SetQuery(IJCombatQuery query)
        {
            this.query = query;
        }
    }
}
