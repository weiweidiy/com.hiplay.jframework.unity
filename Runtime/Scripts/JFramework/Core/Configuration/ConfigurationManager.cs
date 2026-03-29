using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JFramework.Configuration
{
    /// <summary>
    /// 配置表管理器
    /// Feature：
    ///     1. 可以从不同的地方加载配置文件，比如本地，远程（默认提供LoacalReader和HttpReader， 可以自定义实现Reader接口）
    ///     2. 可以解析不同格式的配置文件，比如json,xml（默认提供JsonParaser,可以自定义实现IParaser接口和IChainData接口）
    ///     3. 可以修改配置数据
    ///     4. 可以保存到指定位置，比如本地，远程
    /// Example：
    ///     step-1. 实例化管理器
    ///     ConfigurationManager manager = new ConfigurationManager(new HttpReader(), new JsonParaser());
    ///     
    ///     step-2. 注册配置表，加载（可以链式编程直接调用加载）
    ///     await manager.RegistConfiguration("key", "url or filePath")
    ///                  .RegistConfiguration("key", "url or filePath")
    ///                  .LoadAllAsync();
    ///                  
    ///     step-3. 访问数据             
    ///     Console.WriteLine(manager["key"][1].GetValue("name"));
    ///     
    ///     step-4. 修改数据
    ///     manager["key"][1]["name"].SetValue("555");
    /// </summary>
    public class ConfigurationManager
    {
        /// <summary>
        /// 加载进度变更委托
        /// </summary>
        public Action<float> onProgressChanged;

        /// <summary>
        /// 加载进度
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// 配置文件注册信息
        /// </summary>
        private Dictionary<object, RegistInformation> _dicFilePath = new Dictionary<object, RegistInformation>();

        /// <summary>
        /// 默认文件加载器
        /// </summary>
        private IReader _defaultReader;

        /// <summary>
        /// 默认内容解析器
        /// </summary>
        private IParaser _defaultParaser;

        /// <summary>
        /// 默认的写入器
        /// </summary>
        private IWriter _defaultWriter;

        /// <summary>
        /// 数据转换器
        /// </summary>
        private JBytesconverter _converter = new JBytesconverter();

        /// <summary>
        /// 配置对象列表
        /// </summary>
        private Dictionary<object, Configuration> _dicConfiguration = new Dictionary<object, Configuration>();

        #region 构造器
        /// <summary>
        /// 配置索引器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Configuration this[object key]
        {
            get => _dicConfiguration[key];
        }

        /// <summary>
        /// 无参构造函数,默认本地加载，Json格式，写入本地
        /// </summary>
        public ConfigurationManager() : this(new LocalReader(), new XmlParaser(), new LocalWriter()) {}

        /// <summary>
        /// 构造函数,默认Json格式，写入本地
        /// </summary>
        /// <param name="fileReader">文件加载器：LocalReader, HttpReader</param>
        public ConfigurationManager(IReader fileReader):this(fileReader, new XmlParaser(), new LocalWriter()) {}

        /// <summary>
        /// 构造函数，默认写入本地
        /// </summary>
        /// <param name="fileReader"></param>
        /// <param name="contentParaser"></param>
        public ConfigurationManager(IReader fileReader, IParaser contentParaser) : this(fileReader, contentParaser, new LocalWriter()) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="contentParaser">内容解析器：JsonParaser, XmlParaser</param>
        /// <param name="fileReader">文件加载器：LocalReader, HttpReader</param>
        /// <param name="fileWriter">文件写入器：LocalWriter, HttpWriter</param>
        public ConfigurationManager(IReader fileReader, IParaser contentParaser , IWriter fileWriter)
        {
            SetDefaultFileReader(fileReader);
            SetDefaultContentParaser(contentParaser);
            SetDefaultFileWriter(fileWriter);
        }
        #endregion

        /// <summary>
        /// 设置默认的文件解析器
        /// </summary>
        /// <param name="contentParaser">内容解析器：JsonParaser, XmlParaser</param>
        public ConfigurationManager SetDefaultContentParaser(IParaser contentParaser)
        {
            if (contentParaser == null)
                throw new ArgumentNullException("content paraser can not be null！");

            _defaultParaser = contentParaser;

            return this;
        }

        /// <summary>
        /// 设置默认的文件加载器
        /// </summary>
        /// <param name="fileReader">文件加载器：LocalReader, HttpReader</param>
        public ConfigurationManager SetDefaultFileReader(IReader fileReader)
        {
            if (fileReader == null)
                throw new ArgumentNullException("file reader can not be null！");

            _defaultReader = fileReader;

            return this;
        }

        /// <summary>
        /// 设置默认的文件写入器
        /// </summary>
        /// <param name="fileWriter"></param>
        /// <returns></returns>
        public ConfigurationManager SetDefaultFileWriter(IWriter fileWriter)
        {
            if (fileWriter == null)
                throw new ArgumentNullException("file writer can not be null！");

            _defaultWriter = fileWriter;

            return this;
        }

        /// <summary>
        /// 注册配置表信息
        /// </summary>
        /// <param name="key">某个配置表的名字，用于定位是哪个配置表</param>
        /// <param name="location">该配置表的源路径</param>
        /// <param name="reader">用于加载该配置表的加载器，默认为管理器自身</param>
        /// <returns></returns>
        public ConfigurationManager RegistConfiguration(object key, string location, IReader reader = null, IParaser paraser = null , IWriter writer = null)
        {
            IReader targetReader = reader ?? _defaultReader;
            IParaser targetParaser = paraser ?? _defaultParaser;
            IWriter targetWriter = writer ?? _defaultWriter;

            //注册配置信息
            if (Registed(key))
            {
                _dicFilePath[key].Location = location;
                _dicFilePath[key].Reader = targetReader;
                _dicFilePath[key].Writer = targetWriter;
                _dicFilePath[key].Paraser = targetParaser;
            }
            else
            {
                _dicFilePath.Add(key, new RegistInformation(location, targetReader, targetParaser, targetWriter));
            }

            return this;
        }

        /// <summary>
        /// 批量注册配置信息
        /// </summary>
        /// <param name="configurations">key,location</param>
        /// <returns></returns>
        public ConfigurationManager RegistConfiguration(Dictionary<object, string> configurations)
        {
            if (configurations == null)
                throw new ArgumentNullException("the argument configurations is null");

            foreach(var key in configurations.Keys)
            {
                string location = configurations[key];

                RegistConfiguration(key, location);
            }

            return this;
        }

        /// <summary>
        /// 异步加载所有注册过的配置
        /// </summary>
        public async Task LoadAllAsync(Action<object, Configuration> completed = null)
        {
            int total = _dicFilePath.Keys.Count;
            float done = .0f;
            foreach (var key in _dicFilePath.Keys)
            {
                string filePath = GetLocation(key);
                IReader reader = GetReader(key);
                IParaser paraser = GetParaser(key);
                IWriter writer = GetWriter(key);

                //加载任务（异步）
                await LoadAsync(key, filePath, reader, paraser, writer, completed);
                
                //加载完成，更新进度
                done++;
                Progress = done / total;
                onProgressChanged?.Invoke(Progress);
            }
        }

        /// <summary>
        /// 加载一个配置文件,如果没注册会自动注册
        /// </summary>
        /// <param name="key">配置文件名称</param>
        /// <param name="filePath">配置文件加载地址</param>
        /// <param name="reader">对应加载器</param>
        /// <param name="paraser">对应解析器</param>
        /// <param name="writer">对应写入器</param>
        /// <param name="completed">加载完成委托</param>
        /// <returns></returns>
        public async Task LoadAsync(object key, string filePath, IReader reader = null, IParaser paraser = null
                                                        , IWriter writer = null , Action<object, Configuration> completed = null)
        {
            //没注册过就注册
            if(!Registed(key))
                RegistConfiguration(key, filePath, reader, paraser,writer);

            IReader targetReader = reader == null ? GetReader(key) : reader;
            IParaser targetParaser = paraser == null? GetParaser(key) : paraser;

            if (targetReader == null)
                throw new Exception("the targetReader is null, you should call SetDefaultFileReader method to set a default reader. ");

            if (targetParaser == null)
                throw new Exception("the targetParaser is null, you should call SetDefaultContentParaser method to set a default paraser. ");

            //加载配置文件
            var task = targetReader.ReadAsync<byte[]>(filePath, _converter);
            byte[] bytes = await task;

            //如果为空，直接跳过
            if (bytes == null)
            {
                completed?.Invoke(key,null);
                return;
            }

            //更新配置数据
            string content = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            Configuration config = new Configuration(content, targetParaser);
            
            UpdateConfiguration(key, config);

            completed?.Invoke(key,config);
        }

        /// <summary>
        /// 同步加载所有配置表
        /// </summary>
        /// <returns></returns>
        public void LoadAll()
        {
            int total = _dicFilePath.Keys.Count;
            float done = .0f;
            foreach (var key in _dicFilePath.Keys)
            {
                string filePath = _dicFilePath[key].Location;
                IReader reader = _dicFilePath[key].Reader;
                IParaser paraser = _dicFilePath[key].Paraser;

                //加载任务（异步）
                Load(key, filePath, reader, paraser);

                //加载完成，更新进度
                done++;
                Progress = done / total;
                onProgressChanged?.Invoke(Progress);
            }
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="key"></param>
        /// <param name="filePath"></param>
        /// <param name="reader"></param>
        /// <param name="paraser"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public void Load(object key, string filePath, IReader reader = null, IParaser paraser = null
                                        , IWriter writer = null)
        {
            Load(key, "", filePath, reader, paraser, writer);
            ////没注册过就注册
            //if (!Registed(key))
            //    RegistConfiguration(key, filePath, reader, paraser, writer);

            //IReader targetReader = reader == null ? GetReader(key) : reader;
            //IParaser targetParaser = paraser == null ? GetParaser(key) : paraser;

            //if (targetReader == null)
            //    throw new Exception("the targetReader is null, you should call SetDefaultFileReader method to set a default reader. ");

            //if (targetParaser == null)
            //    throw new Exception("the targetParaser is null, you should call SetDefaultContentParaser method to set a default paraser. ");

            //byte[] bytes = null;
            //try
            //{
            //    bytes = targetReader.Read(filePath);
            //}
            //catch(Exception e)
            //{
            //    throw e;
            //}
            
            //if (bytes == null)
            //    return;

            //string content = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            ////Console.WriteLine(content);
            ////更新配置数据
            //UpdateConfiguration(key, new Configuration(content, targetParaser));
        }

        /// <summary>
        /// 直接加载一段文本
        /// </summary>
        /// <param name="key"></param>
        /// <param name="text"></param>
        /// <param name="reader"></param>
        /// <param name="paraser"></param>
        /// <param name="writer"></param>
        public void Load(object key, string content, string filePath , IReader reader = null, IParaser paraser = null, IWriter writer = null)
        {
            if (!Registed(key))
                RegistConfiguration(key, filePath, reader, paraser, writer);

            //获取目标解析器
            IParaser targetParaser = paraser == null ? GetParaser(key) : paraser;

            if (content == null || content == "")
            {
                IReader targetReader = reader == null ? GetReader(key) : reader;

                if (targetReader == null)
                    throw new Exception("the targetReader is null, you should call SetDefaultFileReader method to set a default reader. ");

                if (targetParaser == null)
                    throw new Exception("the targetParaser is null, you should call SetDefaultContentParaser method to set a default paraser. ");

                byte[] bytes = null;
                try
                {
                    bytes = targetReader.Read(filePath);
                }
                catch (Exception e)
                {
                    throw e;
                }

                if (bytes == null)
                    return;

                content = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
               
            //更新配置数据
            UpdateConfiguration(key, new Configuration(content, targetParaser));
        }

        /// <summary>
        /// 是否已经注册过
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool Registed(object key)
        {
            return _dicFilePath.ContainsKey(key);
        }

        /// <summary>
        /// 查询是否已经加载过
        /// </summary>
        /// <param name="key">配置文件名称</param>
        /// <returns></returns>
        public bool Loaded(object key)
        {
            return _dicConfiguration.ContainsKey(key) && _dicConfiguration[key] != null;
        }

        /// <summary>
        /// 保存所有已加载的配置文件数据
        /// </summary>
        /// <returns></returns>
        public async Task<ConfigurationManager> SaveAllAsync(IWriter writer = null, Action<object, Configuration> completed = null)
        {
            foreach(var key in _dicConfiguration.Keys)
            {
                await SaveAsync(key, null, writer, completed);
            }
            return this;
        }

        /// <summary>
        /// 保存指定配置文件到指定位置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="toLocation"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public async Task<ConfigurationManager> SaveAsync(object key , string toLocation = null, IWriter writer = null, Action<object, Configuration> completed = null)
        {
            if (!Loaded(key))
            {
                Console.WriteLine("要保存的配置数据还没有注册或还没有加载：" + key);
                return this;
            }

            string targetLocation = toLocation ?? GetLocation(key);
            IWriter targetWriter = writer ?? GetWriter(key);
            Configuration configuration = GetConfiguration(key);

            if (targetWriter == null)
                throw new Exception("the targetWriter is null, you should call SetDefaultFileWriter method to set a default writer. ");

            if (configuration == null)
                throw new Exception("can not save a null configuration: " + key);

            string content = configuration.ToString();

            await targetWriter.WriteAsync(targetLocation, content);

            completed?.Invoke(key,configuration);

            return this;
        }

        /// <summary>
        /// 同步保存
        /// </summary>
        /// <returns></returns>
        public void SaveAll(IWriter writer = null)
        {
            foreach (var key in _dicConfiguration.Keys)
            {
                Save(key, null, writer);
            }
        }

        /// <summary>
        /// 同步保存 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="toLocation"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public void Save(object key, string toLocation = null, IWriter writer = null)
        {
            if (!Loaded(key))
            {
                Console.WriteLine("要保存的配置数据还没有注册或还没有加载：" + key);
                return;
            }

            string targetLocation = toLocation ?? GetLocation(key);
            IWriter targetWriter = writer ?? GetWriter(key);
            Configuration configuration = GetConfiguration(key);

            if (targetWriter == null)
                throw new Exception("the targetWriter is null, you should call SetDefaultFileWriter method to set a default writer. ");

            if (configuration == null)
                throw new Exception("can not save a null configuration: " + key);

            string content = configuration.ToString();

            targetWriter.Write(targetLocation, content , Encoding.UTF8);
        }

        /// <summary>
        /// 获取注册的数量
        /// </summary>
        /// <returns></returns>
        public int GetRegistCount()
        {
            return _dicFilePath.Count;
        }

        #region 私有方法
        /// <summary>
        /// 获取注册的加载器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private IReader GetReader(object key)
        {
            return _dicFilePath[key].Reader;
        }

        /// <summary>
        /// 获取解析器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private IParaser GetParaser(object key)
        {
            return _dicFilePath[key].Paraser;
        }
        
        /// <summary>
        /// 获取配置表源的加载位置，可能是本地path，也可能是url
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetLocation(object key)
        {
            return _dicFilePath[key].Location;
        }

        /// <summary>
        /// 获取写入器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private IWriter GetWriter(object key)
        {
            return _dicFilePath[key].Writer;
        }

        /// <summary>
        /// 获取配置数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Configuration GetConfiguration(object key)
        {
            return _dicConfiguration[key];
        }

        /// <summary>
        /// 更新配置对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void UpdateConfiguration(object key, Configuration value)
        {
            if (_dicConfiguration.ContainsKey(key))
                _dicConfiguration[key] = value;
            else
                _dicConfiguration.Add(key, value);
        }

        #endregion

        /// <summary>
        /// 注册信息类，用于保存注册信息
        /// </summary>
        class RegistInformation
        {
            /// <summary>
            /// 配置文件地址
            /// </summary>
            public string Location { get; set; }

            /// <summary>
            /// 加载器
            /// </summary>
            public IReader Reader { get; set; }

            /// <summary>
            /// 解析器
            /// </summary>
            public IParaser Paraser { get; set; }

            /// <summary>
            /// 写入器
            /// </summary>
            public IWriter Writer { get; set; }

            public RegistInformation(string location, IReader reader , IParaser paraser, IWriter writer)
            {
                Location = location;
                Reader = reader;
                Writer = writer;
                Paraser = paraser;
            }
        }
    }
}
