using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;

namespace JFramework
{
    /// <summary>
    /// 有问题，数组索引会报错
    /// </summary>
    public class XmlChainData : IChainData
    {
        public IChainData this[object key]
        {
            get
            {
                if (key is int)
                {
                    //数组索引
                    var nodes = _xmlElement.ChildNodes;
                    var node = nodes[(int)key];
                    return new XmlChainData((XmlElement)node);
                }
                return new XmlChainData(_xmlElement[(string)key]);
            }
        }

        /// <summary>
        /// xml元素
        /// </summary>
        private XmlElement _xmlElement;

        public XmlChainData(XmlElement xmlElement)
        {
            _xmlElement = xmlElement;
            //_xmlElement.SelectNodes
        }

        public override string ToString()
        {
            return _xmlElement.OuterXml;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public object GetValue(object attributeName)
        {
            return _xmlElement.GetAttributeNode((string)attributeName).InnerText;
        }

        /// <summary>
        /// 获取内容文本
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            return _xmlElement.InnerText;
        }

        public T GetValue<T>(object attributeName)
        {
            throw new NotImplementedException();
        }

        public T GetValue<T>()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="value"></param>
        void IChainData.SetValue(object value)
        {
            _xmlElement.InnerText = (string)value;
        }

        /// <summary>
        /// 筛选单个节点数据
        /// </summary>
        /// <param name="xPathQueryCommand">"service[@name='reader' or @name='writer']"</param>
        /// <returns></returns>
        public IChainData Select(string xPathQueryCommand)
        {
            return new XmlChainData((XmlElement)_xmlElement.SelectSingleNode(xPathQueryCommand));
        }

        /// <summary>
        /// 筛选多个节点 
        /// </summary>
        /// <param name="queryCommand">"service[@name='reader' or @name='writer']"</param>
        /// <returns></returns>
        public IEnumerable<IChainData> SelectMany(string queryCommand)
        {
            XmlNodeList results = _xmlElement.SelectNodes(queryCommand);
            int count = results.Count;
            //Console.WriteLine(" count = " + count);
            XmlChainData[] arr = new XmlChainData[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = new XmlChainData((XmlElement)results[i]);
            }
            return arr;
        }

        public T ToObject<T>(object serializer = null)
        {
            throw new NotImplementedException();
        }

        public void AddObject(string key, object value)
        {
            throw new NotImplementedException();
        }

        public bool RemoveObject(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveObject()
        {
            throw new NotImplementedException();
        }


    }
}
