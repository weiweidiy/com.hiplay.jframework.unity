using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace JFramework
{
    public class Utility
    {
        /// <summary>
        /// 获取唯一id
        /// </summary>
        /// <returns></returns>
        public string CreateUniqueId()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 二分排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="comparer"></param>
        public void BinarySort<T>(T[] arr, IComparer<T> comparer)
        {
            T temp;
            int j, mid;
            for (int i = 0; i < arr.Length; i++)
            {
                temp = arr[i];
                //把temp当成key，利用二分查找法，找到temp需要插入的位置
                int left = 0;
                int right = i - 1; //取i之前的数组

                while (left <= right)
                {
                    mid = (right - left) / 2 + left;

                    //if (temp > arr[mid])
                    if (comparer.Compare(temp, arr[mid]) > 0)
                    {
                        left = mid + 1;
                    }
                    else
                    {
                        right = mid - 1;
                    }
                }
                //通过二分查找法找到temp需要插入的位置left。
                for (j = i - 1; j >= left; j--)
                {
                    arr[j + 1] = arr[j];
                }
                //把temp 赋值给left的位置
                arr[left] = temp;
            }
        }

        /// <summary>
        /// 二分排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <param name="comparer"></param>
        public void BinarySort<T>(List<T> lst, IComparer<T> comparer)
        {
            T temp;
            int j, mid;
            for (int i = 0; i < lst.Count; i++)
            {
                temp = lst[i];
                //把temp当成key，利用二分查找法，找到temp需要插入的位置
                int left = 0;
                int right = i - 1; //取i之前的数组

                while (left <= right)
                {
                    mid = (right - left) / 2 + left;

                    //if (temp > arr[mid])
                    if (comparer.Compare(temp, lst[mid]) > 0)
                    {
                        left = mid + 1;
                    }
                    else
                    {
                        right = mid - 1;
                    }
                }
                //通过二分查找法找到temp需要插入的位置left。
                for (j = i - 1; j >= left; j--)
                {
                    lst[j + 1] = lst[j];
                }
                //把temp 赋值给left的位置
                lst[left] = temp;
            }
        }

        /// <summary>
        /// 二分查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortedList"></param>
        /// <param name="target"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public int BinarySearch<T>(List<T> sortedList, T target, IComparer<T> comparer)
        {
            int left = 0;
            int right = sortedList.Count - 1;
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                //if (sortedList[mid] == target)
                if (comparer.Compare(sortedList[mid], target) == 0)
                    return mid; // 如果找到，返回其索引
                //if (sortedList[mid] < target)
                if (comparer.Compare(sortedList[mid], target) < 0)
                    left = mid + 1; // 如果目标大于中间值，则在右半部分继续查找
                else
                    right = mid - 1; // 如果目标小于中间值，则在左半部分继续查找
            }
            return left; // 如果未找到，返回应该插入的位置
        }

        /// <summary>
        /// 二分插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortedList"></param>
        /// <param name="target"></param>
        /// <param name="comparer"></param>
        public void BinarySearchInsert<T>(List<T> sortedList, T target, IComparer<T> comparer)
        {
            int left = 0;
            int right = sortedList.Count - 1;
            int result = 0;
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                //if (sortedList[mid] == target)
                if (comparer.Compare(sortedList[mid], target) == 0)
                    result = mid; // 如果找到，返回其索引
                //if (sortedList[mid] < target)
                if (comparer.Compare(sortedList[mid], target) < 0)
                    left = mid + 1; // 如果目标大于中间值，则在右半部分继续查找
                else
                    right = mid - 1; // 如果目标小于中间值，则在左半部分继续查找
            }
            result = left; // 如果未找到，返回应该插入的位置

            sortedList.Insert(result, target);
        }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public T DeepClone<T>(T obj, bool isAll = false)
        {

            if (obj == null) return default(T);

            if(!isAll)
            {
                var json = JsonConvert.SerializeObject(obj);
                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All, // 保留类型信息
                    Formatting = Formatting.None
                };
                string json = JsonConvert.SerializeObject(obj, settings);
                return JsonConvert.DeserializeObject<T>(json, settings);
            }
        }

        /// <summary>
        /// 获取列表中的随机元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public T GetRandomItem<T>(List<T> list)
        {
            // 创建随机数生成器
            Random random = new Random();

            // 生成一个随机索引
            int index = random.Next(list.Count);

            // 返回列表中对应索引的值
            return list[index];
        }

        public List<T> GetRandomItems<T>(List<T> list, int count)
        {
            if (list == null || list.Count == 0 || count <= 0)
                return new List<T>();

            List<T> result = new List<T>(count);
            Random random = new Random();

            if (count <= list.Count)
            {
                // 无放回抽取
                List<T> tempList = new List<T>(list);
                for (int i = 0; i < count; i++)
                {
                    int index = random.Next(tempList.Count);
                    result.Add(tempList[index]);
                    tempList.RemoveAt(index);
                }
            }
            else
            {
                // 有放回抽取
                for (int i = 0; i < count; i++)
                {
                    int index = random.Next(list.Count);
                    result.Add(list[index]);
                }
            }

            return result;
        }

        /// <summary>
        /// 百分比随机是否命中
        /// </summary>
        /// <param name="hitValue">命中值：比如 45%概率命中就填寫45 </param>
        /// <returns></returns>
        public bool RandomHit(float hitValue)
        {
            // 创建一个随机数生成器
            Random random = new Random();

            // 生成一个0到100之间的随机整数
            int randomNumber = random.Next(0, 100);

            // 判断是否命中
            return randomNumber >= 0 && randomNumber <= hitValue;

        }


        /// <summary>
        /// 无关地区的解析浮点
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public float ParseFloatInvariantCulturet(string str)
        {
            return float.Parse(str, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 无关地区格式的解析double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public double ParseDoubleInvariantCulturet(string str)
        {
            return double.Parse(str, CultureInfo.InvariantCulture);
        }
    }
}


