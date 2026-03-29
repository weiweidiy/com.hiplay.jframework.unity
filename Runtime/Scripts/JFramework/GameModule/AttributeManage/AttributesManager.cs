using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JFramework
{

    public class AttributesManager
    {
        /// <summary>
        /// 属性被添加了事件
        /// </summary>
        public event Action<BaseAttribute> onAttrAdded;

        /// <summary>
        /// 属性被移除了事件
        /// </summary>
        public event Action<BaseAttribute> onAttrRemoved;

        /// <summary>
        /// 所有属性对象列表
        /// </summary>
        List<BaseAttribute> attributes = new List<BaseAttribute>();


        /// <summary>
        /// 增加属性
        /// </summary>
        /// <param name="attrId"></param>
        /// <param name="value"></param>
        /// <param name="sysSource">系统源，默认采用配置表源</param>
        /// <returns></returns>
        public string AddValueAttribute(int attrId, float value, AttributeSubType subType, AttributeType attrType, AttributeSource sysSource = AttributeSource.Null)
        {
            string uid = Guid.NewGuid().ToString();
            BaseAttribute attr = null;
            switch (subType)
            {
                case AttributeSubType.BaseUp://基础加
                    attr = new PlusAttribute(uid, attrId, attrType, value, sysSource, subType);
                    break;
                case AttributeSubType.BaseDown://基础减
                    attr = new MinusAttribute(uid, attrId, attrType, value, sysSource, subType);
                    break;
                case AttributeSubType.MultiUp://乘加
                    attr = new MultiUpAttribute(uid, attrId, attrType, value, sysSource, subType);
                    break;
                case AttributeSubType.MultiDown://乘减
                    attr = new MultiDownAttribute(uid, attrId, attrType, value, sysSource, subType);
                    break;
                case AttributeSubType.ExtraUp://额外加
                    attr = new PlusAttribute(uid, attrId, attrType, value, sysSource, subType);
                    break;
                case AttributeSubType.ExtraDown://额外减
                    attr = new MinusAttribute(uid, attrId, attrType, value, sysSource, subType);
                    break;
                default:
                    throw new Exception("没有实现的属性算法类型 " + subType);
            }

            attributes.Add(attr);
            onAttrAdded?.Invoke(attr);
            return uid;
        }

        /// <summary>
        /// 删除指定属性
        /// </summary>
        /// <param name="uid"></param>
        public bool RemoveAttribute(string uid)
        {
            var attr = GetAttribute(uid);
            Debug.Assert(attr != null, "要删除的属性为空");
            var result = attributes.Remove(attr);

            onAttrRemoved?.Invoke(attr);

            return result;
        }

        /// <summary>
        /// 更新属性
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="value"></param>
        public void UpdateValueAttribute(string uid, float value)
        {
            var attr = GetAttribute(uid);
            Debug.Assert(attr != null, "要更新的属性为空");
            try
            {
                (attr as ValueAttribute).UpdateValue(value);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "UpdateValueAttribute 尝试ValueAttribute类型转换失败 " + attr.GetType() + " id:" + attr.Id);
                //Debug.LogError(e.Message + "UpdateValueAttribute 尝试ValueAttribute类型转换失败 " + attr.GetType() + " id:" + attr.Id);
            }
        }

        /// <summary>
        /// 获取指定属性
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        BaseAttribute GetAttribute(string uid)
        {
            return attributes.Where(attr => attr.Uid.Equals(uid)).SingleOrDefault();
        }

        /// <summary>
        /// 获取所有指定类型的属性
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<BaseAttribute> GetAttributes(AttributeType type)
        {
            var result = attributes.Where(attr => attr.Type.Equals(type));
            if (result.Count() == 0)
                return null;

            return result.ToList();
        }

        /// <summary>
        /// 根据属性获取所有符合条件的属性对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<BaseAttribute> GetAttributes(int id)
        {
            var result = attributes.Where(attr => attr.Id.Equals(id));
            if (result.Count() == 0)
                return null;

            return result.ToList();
        }

        /// <summary>
        /// 获取总值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public float GetTotalValue(AttributeType type)
        {
            return GetTotalValue(type, AttributeSource.Null);
        }

        /// <summary>
        /// 获取总值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sysSource"></param>
        /// <returns></returns>
        public float GetTotalValue(AttributeType type, AttributeSource sysSource)
        {
            var baseValue = GetBaseValue(type, sysSource);
            //计算基础值(左值）
            var extraValue = GetExtraValue(type, sysSource);
            var multiValue = GetMultiValue(type, sysSource);

            //Debug.Log("type = " + type + " base value =" + baseValue + " multivalue=" + multiValue + " extraValue =" + extraValue + " total = " + baseValue * multiValue + extraValue);
            return baseValue * multiValue + extraValue;
        }

        /// <summary>
        /// 获取所有基础值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public float GetBaseValue(AttributeType type)
        {
            return GetBaseValue(type, AttributeSource.Null);
        }

        /// <summary>
        /// 获取基础值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sysSource"></param>
        /// <returns></returns>
        public float GetBaseValue(AttributeType type, AttributeSource sysSource)
        {
            var attrs = GetAttributes(type);
            if (attrs == null)
                return 0;

            var baseAttrs = attrs.Where(attr => ((attr as ValueAttribute).SubType == AttributeSubType.BaseUp
                                        || (attr as ValueAttribute).SubType == AttributeSubType.BaseDown)
                                        && sysSource == AttributeSource.Null ? true : (attr as ValueAttribute).SysSource == sysSource);

            if (baseAttrs == null || baseAttrs.Count() == 0)
                return 0;

            var sum = baseAttrs.Aggregate(0f, (cur, attr) => (attr as ValueAttribute).GetResult(cur));
            return sum;
        }

        /// <summary>
        /// 获取乘值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public float GetMultiValue(AttributeType type)
        {
            return GetMultiValue(type, AttributeSource.Null);
        }

        /// <summary>
        /// 获取乘值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sysSource"></param>
        /// <returns></returns>
        public float GetMultiValue(AttributeType type, AttributeSource sysSource)
        {
            var attrs = GetAttributes(type);
            if (attrs == null)
                return 1;

            var multiAttrs = attrs.Where(attr => ((attr as ValueAttribute).SubType == AttributeSubType.MultiUp
                                                || (attr as ValueAttribute).SubType == AttributeSubType.MultiDown)
                                                && sysSource == AttributeSource.Null ? true : (attr as ValueAttribute).SysSource == sysSource);

            if (multiAttrs == null || multiAttrs.Count() == 0)
                return 1;

            var groups = multiAttrs.GroupBy(attr => (attr as ValueAttribute).SysSource);
            //                        .Select(g => g.Aggregate((cur, attr) => attr + cur))
            //                        .Aggregate((cur, attr) => cur * attr);

            float result = 1;
            foreach (var group in groups)
            {
                float internalValue = 0;
                foreach (var attr in group)
                {
                    var impAttr = attr as ValueAttribute;
                    switch (impAttr.SubType)
                    {
                        case AttributeSubType.MultiUp:
                            internalValue += (attr as ValueAttribute).Value;
                            break;
                        case AttributeSubType.MultiDown:
                            internalValue -= (attr as ValueAttribute).Value;
                            break;
                        default:
                            throw new Exception("没有实现属性类型的乘值 " + impAttr.SubType);

                    }
                }

                result *= 1 + internalValue / 100f;
            }

            return result;
        }

        /// <summary>
        /// 获取附加加值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public float GetExtraValue(AttributeType type)
        {
            return GetExtraValue(type, AttributeSource.Null);
        }

        /// <summary>
        /// 获取附加加值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public float GetExtraValue(AttributeType type, AttributeSource sysSource)
        {
            var attrs = GetAttributes(type);
            if (attrs == null)
                return 0;

            var extraAttrs = attrs.Where(attr => ((attr as ValueAttribute).SubType == AttributeSubType.ExtraUp
                                                || (attr as ValueAttribute).SubType == AttributeSubType.ExtraDown)
                                                && sysSource == AttributeSource.Null ? true : (attr as ValueAttribute).SysSource == sysSource);

            if (extraAttrs == null || extraAttrs.Count() == 0)
                return 0;

            var sum = extraAttrs.Aggregate(0f, (cur, attr) => (attr as ValueAttribute).GetResult(cur));
            return sum;
        }
    }
}
