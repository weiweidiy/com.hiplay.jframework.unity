using System.Collections.Generic;
using System.Linq;

namespace JFramework
{
    /// <summary>
    /// 可更新帧的容器，避免在update过程中，插入删除单位
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UpdateableContainer<T> : ListContainer<T> where T : IUnique, IUpdateable//, IUpdate<T>
    {
        /// <summary>
        /// 等待添加的action
        /// </summary>
        protected List<T> waitForAddItem = new List<T>();

        /// <summary>
        /// 等待移除的action
        /// </summary>
        protected List<T> waitForRemoveItems = new List<T>();

        /// <summary>
        /// 更新更新的列表
        /// </summary>
        protected List<T> waitForUpdateItems = new List<T>();

        /// <summary>
        /// 更新等待队列
        /// </summary>
        public virtual void UpdateWaitingItems()
        {
            //更新
            foreach (var action in waitForUpdateItems)
            {
                Update(action);
            }
            waitForUpdateItems.Clear();

            //删除
            foreach (var item in waitForRemoveItems)
            {
                Remove(item.Uid);
            }
            waitForRemoveItems.Clear();

            //添加
            foreach (var item in waitForAddItem)
            {
                Add(item);
            }
            waitForAddItem.Clear();
        }

        /// <summary>
        /// 添加一个动作
        /// </summary>
        /// <param name="item"></param>
        public virtual void AddItem(T item)
        {
            waitForAddItem.Add(item);
        }

        /// <summary>
        /// 删除动作
        /// </summary>
        /// <param name="item"></param>
        public virtual void RemoveItem(T item)
        {
            waitForRemoveItems.Add(item);
        }

        /// <summary>
        /// 更新动作
        /// </summary>
        /// <param name="item"></param>
        public virtual void UpdateItem(T item)
        {
            waitForUpdateItems.Add(item);
        }

        /// <summary>
        /// 查找等待队列里的指定单位
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public T FindItemInWaitingAddList(string uid)
        {
            return waitForAddItem.Where(i=> i.Uid == uid).SingleOrDefault();
        }

        /// <summary>
        /// 清理
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            waitForAddItem.Clear();
            waitForRemoveItems.Clear();
            waitForUpdateItems.Clear();
        }
    }

}