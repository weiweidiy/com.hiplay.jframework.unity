using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;



namespace JFramework
{
    /// <summary>
    /// 基础常用容器，基于list的容器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListContainer<T> : IListContainer<T> where T :  IUpdateable
    {
        public event Action<ICollection<T>> onItemAdded;
        public event Action<T> onItemRemoved;
        public event Action<T> onItemUpdated;

        protected List<T> list = new List<T>();
        public virtual void Add(T member)
        {
            list.Add(member);
            onItemAdded?.Invoke(new List<T>() { member }); 
        }

        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null)
                return;

            list.AddRange(collection);
            var lst = new List<T>();
            lst.AddRange(collection);
            onItemAdded?.Invoke(lst.ToList());
        }

        public virtual T Get(string uid)
        {
            return list.Where(i =>  i.Uid == uid).FirstOrDefault();
        }

        public List<T> Get(Predicate<T> predicate)
        {
            return list.Where(i => predicate(i)).ToList();
        }

        public virtual List<T> GetAll()
        {
            return list;
        }

        public virtual bool Remove(string uid)
        {
            var item = Get(uid);
            if(item != null)
            {
                if(list.Remove(item))
                {
                    onItemRemoved?.Invoke(item);
                    return true;
                }
                return false;
            }

            throw new System.Exception("没有找到要删除的item "  + uid);
        }

        public virtual void Update(T member)
        {
            var item = Get(member.Uid);
            var origin = item;
            if(item != null)
            {
                //item = member;
                //item.Update(member);
                item.Update(member);
                onItemUpdated?.Invoke(item);
                return;
            }

            throw new System.Exception("没有找到要更新的item " + member.Uid);
        }

        public int Count()
        {
            return list.Count;
        }

        public virtual void Clear()
        {
            list.Clear();
        }
    }
}

