using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JFramework
{
    public class AdsManager
    {
        /// <summary>
        /// 广告项目字典
        /// </summary>
        Dictionary<AdType, List<IAdItem>> _dicAds = new Dictionary<AdType, List<IAdItem>>();

        /// <summary>
        /// 注册广告平台
        /// </summary>
        /// <param name="platform"></param>
        public void InitializePlatform(AdPlatform platform, Action complete = null)
        {
            platform.Initialize(complete);
        }

        /// <summary>
        /// 注册广告项目
        /// </summary>
        /// <param name="adItem"></param>
        public void RegisterAdItem(IAdItem adItem)
        {
            if (adItem == null)
                throw new ArgumentNullException("不能注册空的广告项目");

            if (ExistAdItem(adItem))
                return;

            GetOrCreateAdList(adItem.AdType).Add(adItem);
        }

        /// <summary>
        /// 加载所有广告
        /// </summary>
        public void LoadAll(Action<string> loadCallBack)
        {
            var keys = _dicAds.Keys;

            foreach (var key in keys)
            {
                Load(key, loadCallBack);
            }
        }

        /// <summary>
        /// 加载指定类型的广告
        /// </summary>
        /// <param name="adType"></param>
        public void Load(AdType adType, Action<string> startLoadCallBack)
        {
            var ads = GetOrCreateAdList(adType);

            if (ads == null)
                return;

            foreach (var adItem in ads)
            {
                if (HasReady(adItem))
                    continue;

                Load(adItem, startLoadCallBack);
            }
        }

        /// <summary>
        /// 加载广告
        /// </summary>
        /// <param name="adCode"></param>
        /// <param name="startLoadCallBack"></param>
        public void Load(string adCode, Action<string> startLoadCallBack)
        {
            IAdItem ad = GetAdItem(adCode);

            Load(ad, startLoadCallBack);
        }

        /// <summary>
        /// 加载广告
        /// </summary>
        /// <param name="adItem"></param>
        /// <param name="startLoadCallBack">开始加载回调</param>
        public void Load(IAdItem adItem, Action<string> startLoadCallBack)
        {
            try
            {
                adItem.Load();
                startLoadCallBack?.Invoke(adItem.AdCode);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 是否已经存在广告对象
        /// </summary>
        /// <param name="adItem"></param>
        /// <returns></returns>
        public bool ExistAdItem(IAdItem adItem)
        {
            List<IAdItem> adList = GetOrCreateAdList(adItem.AdType);

            foreach (var item in adList)
            {
                if (item.AdCode.Equals(adItem.AdCode))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 指定的广告类型是否有已经加载完成的
        /// </summary>
        /// <param name="adType"></param>
        /// <returns></returns>
        public bool HasReady(AdType adType)
        {
            var list = GetOrCreateAdList(adType);

            if (list == null || list.Count == 0)
                return false;

            foreach (var adItem in list)
            {
                if (adItem.IsReady())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 指定广告对象是否加载完成
        /// </summary>
        /// <param name="adCode"></param>
        /// <returns></returns>
        public bool HasReady(string adCode)
        {
            var adItem = GetAdItem(adCode);

            if (adItem == null)
                return false;

            return adItem.IsReady();
        }

        /// <summary>
        /// 指定广告对象是否加载完成
        /// </summary>
        /// <param name="adItem"></param>
        /// <returns></returns>
        public bool HasReady(IAdItem adItem)
        {
            if (adItem == null)
                return false;

            return adItem.IsReady();
        }

        /// <summary>
        /// 显示指定的广告
        /// </summary>
        /// <param name="adType"></param>
        /// <param name="closeCallBack"></param>
        /// <param name="clickCallBack"></param>
        public IAdItem Show(AdType adType, Action<bool> closeCallBack = null, Action clickCallBack = null, Action<string> startLoadCallBack = null)
        {
            if (HasReady(adType))
            {
                IAdItem adItem = GetLoadedAd(adType);

                adItem.Show((result) => { Load(adItem, startLoadCallBack); closeCallBack?.Invoke(result); }, clickCallBack);

                return adItem;
            }

            return null;
        }

        /// <summary>
        /// 显示指定的广告
        /// </summary>
        /// <param name="adCode"></param>
        /// <param name="closeCallBack"></param>
        /// <param name="clickCallBack"></param>
        /// <returns></returns>
        public IAdItem Show(string adCode, Action<bool> closeCallBack = null, Action clickCallBack = null, Action<string> startLoadCallBack = null)
        {
            if (HasReady(adCode))
            {
                IAdItem adItem = GetLoadedAd(adCode);

                adItem.Show((result) => { Load(adItem, startLoadCallBack); closeCallBack?.Invoke(result); }, clickCallBack);

                return adItem;
            }

            return null;
        }

        /// <summary>
        /// 关闭广告
        /// </summary>
        /// <param name="adItem"></param>
        public void Close(IAdItem adItem)
        {
            adItem.Close(false);
        }

        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            CloseAll();

            _dicAds.Clear();
        }

        /// <summary>
        /// 关闭所有广告
        /// </summary>
        public void CloseAll()
        {
            var keys = _dicAds.Keys;

            foreach (var key in keys)
            {
                Close(key);
            }
        }

        /// <summary>
        /// 关闭指定类型的广告
        /// </summary>
        /// <param name="adType"></param>
        public void Close(AdType adType)
        {
            var ads = GetOrCreateAdList(adType);

            if (ads == null)
                return;

            foreach (var adItem in ads)
            {
                Close(adItem);
            }
        }

        ///// <summary>
        ///// 关闭并且重新加载一个广告
        ///// </summary>
        ///// <param name="adItem"></param>
        //public void Reload(IAdItem adItem)
        //{
        //    //Close(adItem);

        //    //重新加载
        //    Load(adItem);
        //}

        /// <summary>
        /// 获取指定类型的广告项目列表
        /// </summary>
        /// <param name="adType"></param>
        /// <returns></returns>
        public List<IAdItem> GetOrCreateAdList(AdType adType)
        {
            if (_dicAds.ContainsKey(adType))
            {
                return _dicAds[adType];
            }
            else
            {
                List<IAdItem> list = new List<IAdItem>();

                _dicAds.Add(adType, list);

                return list;
            }
        }

        /// <summary>
        /// 根据广告代码获取广告对象
        /// </summary>
        /// <param name="adCode"></param>
        /// <returns></returns>
        public IAdItem GetAdItem(string adCode)
        {
            foreach (var item in _dicAds)
            {
                List<IAdItem> list = item.Value;

                var ad = list.Where((p) => p.AdCode.Equals(adCode)).SingleOrDefault();

                if (ad != null)
                    return ad;
            }

            return null;
        }

        /// <summary>
        /// 从已经加载完的列表中获取一个指定类型的广告项
        /// </summary>
        /// <param name="adType"></param>
        /// <returns></returns>
        public IAdItem GetLoadedAd(AdType adType)
        {
            var adList = GetOrCreateAdList(adType);

            if (adList == null || adList.Count == 0)
                return null;

            return adList.Where((adItem) => HasReady(adItem)).SingleOrDefault();
        }

        /// <summary>
        /// 从已经加载完的列表中获取一个指定广告对象
        /// </summary>
        /// <param name="adCode"></param>
        /// <returns></returns>
        public IAdItem GetLoadedAd(string adCode)
        {
            var adItem = GetAdItem(adCode);

            if (adItem == null && !HasReady(adCode))
                return null;

            return adItem;
        }

        /// <summary>
        /// 获取已经注册的广告对象数量
        /// </summary>
        /// <returns></returns>
        public int GetAdItemsCount()
        {
            int result = 0;
            foreach (var lst in _dicAds.Values)
            {
                result += lst.Count;
            }
            return result;
        }

        /// <summary>
        /// 获取指定类型的道具数量
        /// </summary>
        /// <param name="adType"></param>
        /// <returns></returns>
        public int GetAdItemsCount(AdType adType)
        {
            if (!_dicAds.ContainsKey(adType))
                return 0;

            return _dicAds[adType].Count;
        }

    }
}
