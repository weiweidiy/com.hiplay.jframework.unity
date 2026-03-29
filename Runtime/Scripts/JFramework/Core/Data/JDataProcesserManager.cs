using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JFramework
{
    public class JDataProcesserManager
    {
        /// <summary>
        /// 数据加工处理器列表
        /// </summary>
        protected List<IProcesser> _lstProcessers = new List<IProcesser>();

        public JDataProcesserManager(List<IProcesser> processers)
        {
            _lstProcessers = processers ?? _lstProcessers;
        }

        public JDataProcesserManager(params IProcesser[] processers)
        {
            _lstProcessers = processers.ToList();
        }

        /// <summary>
        /// 加添加工处理器
        /// </summary>
        /// <param name="processer"></param>
        /// <returns></returns>
        public JDataProcesserManager AddProcesser(IProcesser processer)
        {
            _lstProcessers.Add(processer);
            return this;
        }

        /// <summary>
        /// 获取处理结果
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public byte[] GetResult(byte[] bytes)
        {
            byte[] result = (byte[])bytes.Clone();
            if (_lstProcessers != null)
            {
                foreach (var processer in _lstProcessers)
                {
                    result = processer.Process(result);
                }
            }
            return result;
        }
    }
}
