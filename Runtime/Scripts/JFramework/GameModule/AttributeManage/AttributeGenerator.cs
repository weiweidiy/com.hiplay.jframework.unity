using System;
using System.Collections.Generic;

namespace JFramework
{

    /// <summary>
    /// 属性生成器
    /// </summary>
    public class AttributeGenerator
    {
        AttributesManager attrManager;

        /// <summary>
        /// 属性字典 系统的key和属性的uid映射表
        /// </summary>
        Dictionary<string, string> dicAttr = new Dictionary<string, string>();


        /// <summary>
        /// 属性需要被添加的响应方法
        /// </summary>
        /// <param name="o"></param>
        private void OnAttributeAdd(object o)
        {
            var arg = o as Dictionary<string, AttributeGeneratorData>;
            foreach (var key in arg.Keys)
            {
                var attrId = arg[key].AttributeId;
                var value = arg[key].AttributeValue;
                var sysSource = arg[key].SysSource;
                var subType = GetSubType(attrId);
                var attrType = GetAttrType(attrId);
                var uid = attrManager.AddValueAttribute(attrId, value, subType, attrType, sysSource);
                dicAttr.Add(key, uid);
            }
        }

        private AttributeType GetAttrType(int attrId)
        {
            throw new NotImplementedException();
        }

        private AttributeSubType GetSubType(int attrId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 属性需要被移除的响应方法
        /// </summary>
        /// <param name="o"></param>
        private void OnAttributeRemove(object o)
        {
            var arg = o as Dictionary<string, AttributeGeneratorData>;
            foreach (var key in arg.Keys)
            {
                attrManager.RemoveAttribute(dicAttr[key]);
                dicAttr.Remove(key);
            }
        }

        /// <summary>
        /// 属性需要被更新的响应方法
        /// </summary>
        /// <param name="o"></param>
        private void OnAttributeUpdate(object o)
        {
            var arg = o as Dictionary<string, AttributeGeneratorData>;
            foreach (var key in arg.Keys)
            {
                attrManager.UpdateValueAttribute(dicAttr[key], arg[key].AttributeValue);
            }
        }
    }
}
