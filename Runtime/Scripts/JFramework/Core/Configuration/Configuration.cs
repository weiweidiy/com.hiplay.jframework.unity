using System.Collections.Generic;

namespace JFramework.Configuration
{
    public class Configuration : IChainData
    {
        public IChainData this[object key] => _chainData[key];

        /// <summary>
        /// 配置节点
        /// </summary>
        private IChainData _chainData;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contentParaser"></param>
        public Configuration(string content, IParaser contentParaser)
        {
            _chainData = contentParaser.Parase(content);
        }

        public override string ToString()
        {
            return _chainData.ToString();
        }

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="queryCommand">
        /// Operator	Description
        /// $                           The root element to query.This starts all path expressions.
        /// @                           The current node being processed by a filter predicate.
        /// *                           Wildcard.Available anywhere a name or numeric are required.
        /// ..                          Deep scan. Available anywhere a name is required.
        /// .<name>	                    Dot-notated child
        /// ['<name>' (, '<name>')]	    Bracket-notated child or children
        /// [< number > (, < number >)] Array index or indexes
        /// [start:end]                 Array slice operator
        /// [?(<expression>)]	        Filter expression.Expression must evaluate to a boolean value. example:"$[?(@.group == 9 && @.id == 9)].name"</param>
        /// <returns></returns>
        public IEnumerable<IChainData> SelectMany(string queryCommand)
        {
            return _chainData.SelectMany(queryCommand);
        }

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="queryCommand"></param>
        /// <returns></returns>
        public IChainData Select(string queryCommand)
        {
            return _chainData.Select(queryCommand);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetValue(object key)
        {
            return _chainData.GetValue(key);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            return _chainData.GetValue();
        }

        public T GetValue<T>(object key)
        {
            return _chainData.GetValue<T>(key);
        }

        public T GetValue<T>()
        {
            return _chainData.GetValue<T>();
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(object value)
        {
            _chainData.SetValue(value);
        }

        /// <summary>
        /// 添加一个json键值对对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddObject(string key, object value)
        {
            _chainData.AddObject(key, value);
        }

        /// <summary>
        /// 序列化成C#对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public T ToObject<T>(object serializer = null)
        {
            return _chainData.ToObject<T>(serializer);
        }

        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveObject(string key)
        {
            return _chainData.RemoveObject(key);
        }

        /// <summary>
        /// 删除自身节点
        /// </summary>
        public void RemoveObject()
        {
            _chainData.RemoveObject();
        }


    }


    ///// <summary>
    ///// 配置对象
    ///// </summary>
    //public class Configuration
    //{
    //    /// <summary>
    //    /// 配置文件内容数据表
    //    /// </summary>
    //    private DataTable _dtConfiguration;

    //    /// <summary>
    //    /// 数据key列名字
    //    /// </summary>
    //    private string _dcKeyName = "name";

    //    /// <summary>
    //    /// 数据content列名字
    //    /// </summary>
    //    private string _dcContentName = "content";

    //    /// <summary>
    //    /// 内容解析器
    //    /// </summary>
    //    private IParaser _paraser;

    //    internal Configuration(string content , IParaser contentParaser)
    //    {
    //        _dtConfiguration = CreateDataTable();
    //        _paraser = contentParaser;
    //    }

    //    /// <summary>
    //    /// 获取符合条件的对象
    //    /// </summary>
    //    /// <returns></returns>
    //    public object GetObject()
    //    {
    //        return null;
    //    }

    //    /// <summary>
    //    /// 创建新行
    //    /// </summary>
    //    /// <param name="name"></param>
    //    /// <param name="content"></param>
    //    internal Configuration CreateDataRow(string name, string content = "")
    //    {
    //        _dtConfiguration.Rows.Add(name, content);
    //        return this;
    //    }



    //    /// <summary>
    //    /// 创建一个数据表，保存所有配置的信息
    //    /// </summary>
    //    private DataTable CreateDataTable()
    //    {
    //        DataTable dt = new DataTable();
    //        //添加主key列
    //        DataColumn dpc = new DataColumn(_dcKeyName, typeof(string));
    //        dt.Columns.Add(dpc);

    //        //设置主key列
    //        dt.PrimaryKey = new DataColumn[1] { dpc };

    //        //添加其他列
    //        dt.Columns.Add(_dcContentName, typeof(string));

    //        return dt;
    //    }

    //    /// <summary>
    //    /// 是否已经包含
    //    /// </summary>
    //    /// <param name="key"></param>
    //    private bool ContainsConfiguration(string key)
    //    {
    //        return _dtConfiguration.Rows.Contains(key);
    //    }

    //    /// <summary>
    //    /// 选择主键值
    //    /// </summary>
    //    /// <param name="name"></param>
    //    /// <returns></returns>
    //    private DataRow SelectByName(string name)
    //    {
    //        string filter = string.Format("{0} = '{1}'", _dcKeyName, name);
    //        DataRow[] result = _dtConfiguration.Select(filter);
    //        if (result == null || result.Length == 0)
    //        {
    //            return null;
    //        }
    //        return result[0];
    //    }
    //}
}
