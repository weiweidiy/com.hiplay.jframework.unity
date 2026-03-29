using System;
using System.Collections.Generic;

namespace JFramework.Game
{
    public abstract class JCombatBaseUnitBuilder : IJCombatUnitBuilder
    {
        protected IJCombatAttrBuilder attrBuilder;
        protected IJCombatActionBuilder actionBuilder;
 
        public JCombatBaseUnitBuilder(IJCombatAttrBuilder attrBuilder, IJCombatActionBuilder actionBuilder)
        {
            this.actionBuilder = actionBuilder;
            this.attrBuilder = attrBuilder;

        }

        public abstract IJCombatUnitInfo Build();

        //{
        //    var info = new IJCombatUnitInfo();
        //    info.Uid = Guid.NewGuid().ToString();
        //    info.AttrList = attrBuilder.Create(key);
        //    info.Actions = actionBuilder.Create(key);
        //    return info;
        //}
    }
}
